using Interface.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApp.Models;

namespace WebApp.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : BaseApiController
    {
        UserContext db = new UserContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AccountController()
        {
        }
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public AccountController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }
        public AccountController(ApplicationSignInManager signInManager)
        {
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

        [HttpPost]
        [Route("api/Account/Login")]
        public HttpResponseMessage Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

            var result = SignInManager.PasswordSignIn(user.UserName, user.Password, false, false);
            if (result.Equals(SignInStatus.Success))
            {
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var UserEmpId = _userManager.FindByName(user.UserName).EmpId.GetValueOrDefault();

                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    var oldAssignment =hrUnitOfWork.EmployeeRepository.GetAll()
                       .Where(a => a.EmpId == UserEmpId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today))
                       .Select(b => new { b.DepartmentId, b.LocationId, b.IsDepManager, b.JobId, b.PositionId, b.BranchId, b.SectorId }).FirstOrDefault();

                    if (oldAssignment != null)
                    {
                        session.Add("DepartmentId", oldAssignment.DepartmentId);
                        session.Add("LocationId", oldAssignment.LocationId);
                        session.Add("PositionId", oldAssignment.PositionId);
                        session.Add("IsDepManager", oldAssignment.IsDepManager.ToString());
                        session.Add("BranchId", oldAssignment.BranchId);
                        session.Add("SectorId", oldAssignment.SectorId);
                        session.Add("JobId", oldAssignment.JobId);
                    }
                    session.Add("MenuId", oldAssignment.DepartmentId);
                    session.Add("RoleId", oldAssignment.LocationId);
                }
                var identity = User.Identity.Name;

                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        [ResponseType(typeof(UsersViewModel)), HttpGet]
        [Route("api/Account/ViewProfile")]
        public IHttpActionResult ViewProfile(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            var session = HttpContext.Current.Session;
            UsersViewModel userVM = new UsersViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
               // DefaultCompany = user.Companies.Where(c => c.Id == user.DefaultCompany).Select(c => c.Id).FirstOrDefault(),
               DefaultCompanyName=db.Companies.Where(c=> c.Id== user.DefaultCompany.Value).Select(a=>a.Name).FirstOrDefault(),
                LastLogin = user.LastLogin,
                PhoneNumber=user.PhoneNumber,
                 
            };
            if (user != null)
                return Ok(userVM);

            return NotFound();
        }


        [HttpPost]
        [Route("api/Account/PostFile")]
        public HttpResponseMessage PostFile()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);

            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);
            //}

            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);

            //try
            //{

            //    //read form data
            //    var res = await Request.Content.ReadAsMultipartAsync(provider);

            //    //get file names
            //    foreach (MultipartFileData file in provider.FileData)
            //    {
            //        var name = file.LocalFileName;

            //    }

            //    return Request.CreateResponse(System.Net.HttpStatusCode.OK);

            //}
            //catch (System.Exception e)
            //{
            //    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, e);
            //}


        }

        //GetUserRole
        [ResponseType(typeof(UsersViewModel)), HttpGet]
        [Route("api/Account/GetUserRole")]
        public IHttpActionResult GetUserRole(string userName, string password)
        {

            var roleList = db.Roles.ToList();
            var RoleId = db.Users.Where(a => a.UserName == userName).Select(a => a.Roles.Select(r => new { roleId = r.RoleId }).ToList()).ToList();
            List<string> RoleName = new List<string>();
            if (RoleId.Count > 0)
                foreach (var item in RoleId.FirstOrDefault())
                {

                    RoleName.Add(roleList.Where(a => a.Id == item.roleId).Select(a => a.Name).FirstOrDefault());
                }

            return Ok(RoleName);
        }
        [ResponseType(typeof(UsersViewModel)), HttpGet]
        [Route("api/Account/LogOff")]
        public IHttpActionResult LogOff()
        {
           
            var db = new UserContext();
            var userName = User.Identity.Name;
            var LoginUser = db.Users.Where(a => a.UserName == "seham").FirstOrDefault();
            var user = db.UserLogs.Where(b => b.UserId == LoginUser.Id && b.EndTime == null).OrderByDescending(b => b.Id).FirstOrDefault();
           // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TimeSpan duration = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).Subtract(Convert.ToDateTime(user.StartTime));
            //double getDuration = duration.TotalMinutes;
            user.Duration = duration;
            LoginUser.LastLogin = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            try
            {
                db.SaveChanges();

            }
            catch (Exception ex)
            {
               // TempData["Error"] = ex.Message;
                Models.Utils.LogError(ex.Message);
            }
           // AuthenticationManager.SignOut();
            //Session.Abandon();
            //Session.Clear();
            return Json("Success");
           // return RedirectToAction("Index", "Home");
        }

    }
}