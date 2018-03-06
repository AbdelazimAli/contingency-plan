using Interface.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Model.Domain;
using Model.Domain.Notifications;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.CustomFilters;
using WebApp.Models;

namespace WebApp.Controllers.NewApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SecurityController : BaseApiController
    {
        UserContext db = new UserContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        public SecurityController()
        {
        }
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public SecurityController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }
        public SecurityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpPost]
        [MyCustomAuthorize]
        [Route("newApi/Security/Login")]
        public async Task<IHttpActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var appUser = db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
            if (appUser == null)
            {
                return NotFound();
            }

            if (appUser.EmpId == null)
            {
                return StatusCode(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                int active = db.Database.SqlQuery<int>("select 1 from Assignments A where A.EmpId = " + appUser.EmpId + " and (CONVERT(date, GetDate()) BETWEEN A.AssignDate And A.EndDate) And A.sysAssignStatus = 1").FirstOrDefault();
                if (active != 1)
                {
                    return StatusCode(HttpStatusCode.NotAcceptable);
                }
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    string Code = null;
                    if (appUser.ResetPassword)
                    {
                        Code = await UserManager.GeneratePasswordResetTokenAsync(appUser.Id);

                    }
                    var R = await UserManager.GetRolesAsync(appUser.Id.ToString());
                    var vv = hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.Source == "EmployeePic" && a.SourceId == appUser.EmpId.Value).Select(a => a.file_stream).FirstOrDefault();

                    string EImage = null;
                    if (vv != null)
                    {
                        EImage = "data:image/jpg;base64," + Convert.ToBase64String(vv);
                    }
                    vv =hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.Source == "CompanyLogo" && a.SourceId == appUser.DefaultCompany.Value).Select(a => a.file_stream).FirstOrDefault();
                    string CompanyLogo = null;
                    if (vv != null)
                    {
                        CompanyLogo = "data:image/jpg;base64,"+ Convert.ToBase64String(vv);

                    }
                    var locaName = hrUnitOfWork.PeopleRepository.GetEpmLocalname((int)appUser.EmpId, appUser.Language);
                    var lastLogin = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                    var user = db.Users.Where(u => u.UserName == model.UserName).Select(u => new
                    {
                        u.Id,
                        u.Culture,
                        CompanyId = u.DefaultCompany,
                        u.Email,
                        u.EmpId,
                        u.UserName,
                        User.Identity.AuthenticationType,
                        Name = User.Identity.Name,
                        u.ResetPassword,
                        IsAuthenticated = User.Identity.IsAuthenticated,
                        u.Language,
                        Roles = R,
                        Password = model.Password,
                        EmpImg =  EImage,
                        ComImg =  CompanyLogo,
                        LocalName=locaName,
                        LastLogin= lastLogin,
                        Code = Code
                    }).SingleOrDefault();
                    UserLog loggedUser = new UserLog();
                    loggedUser.UserId = user.Id;
                    loggedUser.LogEvent = 1;
                    loggedUser.CompanyId = appUser.DefaultCompany.Value;
                    loggedUser.StartTime = DateTime.Now; //Convert.ToDateTime(data.Value.ToString("H:mm"));
                    appUser.AccessFailedCount = 0;
                    appUser.LastLogin = DateTime.Now;
                    db.UserLogs.Add(loggedUser);
                    int res = await db.SaveChangesAsync();
                    return Ok(user);

                case SignInStatus.LockedOut:
                    return StatusCode(HttpStatusCode.ExpectationFailed);
                case SignInStatus.Failure:
                    appUser.AccessFailedCount = appUser.AccessFailedCount + 1;
                    int rest = db.SaveChanges();

                    return StatusCode(HttpStatusCode.Forbidden);

                default:
                    return StatusCode(HttpStatusCode.Forbidden);

            }


        }
        [HttpPost]
        [Route("newApi/Security/Reset")]
        public async Task<IHttpActionResult> Reset([FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            var user = db.Users.FirstOrDefault(us => us.UserName == model.UserName);
            if (user == null)
            {
                return NotFound();
            }

            if (model.Password == model.OldPassword)
            {
                return Conflict();
            }

            if (model.Password == model.ConfirmPassword)
            {
                var res = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

                if (res.Errors.Count() > 0)
                    return StatusCode(HttpStatusCode.NotModified);
            }
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            if (user.ResetPassword == true)
            {
                user.ResetPassword = false;
                int returnData = db.SaveChanges();
                if (returnData <= 0)
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }


            var R = await UserManager.GetRolesAsync(user.Id.ToString());

            return Ok(new
            {
                Id = user.Id,
                Culture = user.Culture,
                CompanyId = user.DefaultCompany,
                Email = user.Email,
                EmpId = user.EmpId,
                UserName = user.UserName,
                User.Identity.AuthenticationType,
                Name = User.Identity.Name,
                ResetPassword = user.ResetPassword,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Language = user.Language,
                Roles = R,
                Password = model.Password,

            });
        }

        [HttpPost]
        [Route("newApi/Security/ResetLanguage")]
        public async Task<IHttpActionResult> ResetLanguage([FromBody]NewLangViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Equals(SignInStatus.Success))
            {
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var appUser = db.Users.Where(u => u.UserName == model.UserName).SingleOrDefault();
                appUser.Language = model.Language;
                int res = await db.SaveChangesAsync();
                if (res > 0)
                {

                    return Ok(res);
                }
                return StatusCode(HttpStatusCode.NotModified);

            }
            return StatusCode(HttpStatusCode.Forbidden);
        }
        [HttpPost]
        [Route("newApi/Security/ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordMobileViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = db.Users.FirstOrDefault(u => u.UserName == model.Username && u.Email == model.Email);
            if (user == null)
                return NotFound();

            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            if (code == null)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            if (model.WithEmail == true)
            {
                var callbackUrl = Url.Link("LogPattern", new { Controller = "Account", Action = "ResetPassword", userId = user.Id, UserName = user.UserName, code = code });
                EmailAccount emailAcc = HrUnitOfWork.Repository<EmailAccount>().FirstOrDefault();
                emailAcc.EnableSsl = false;
                string Send = Db.Persistence.Services.EmailService.SendEmail(emailAcc, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", user.Email, user.UserName, null, null);
                if (Send == "Error")
                {
                    return BadRequest();
                }
                return Ok(new ForgetDataViewModel() { UserName = user.UserName, UserId = user.Id, Code = null });

            }
            else
            {
                return Ok(new ForgetDataViewModel() { UserName = user.UserName, UserId = user.Id, Code = code });

            }


        }
    }

    public class NewLangViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Language { get; set; }

    }

    public class ForgetDataViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string OldPassword { get; set; }

    }

    public class ForgotPasswordMobileViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
        public string Culture { get; set; }
        public bool WithEmail { get; set; }
        public string TempUrl { get; set; }
    }
}
