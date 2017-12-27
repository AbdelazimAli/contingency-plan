using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Extensions;
using WebApp.CustomFilters;
using Interface.Core;
using System.Net.Http;
using System.Collections.Generic;
using Model.Domain.Notifications;
using System.Web.Hosting;
using System.IO;
using System.Configuration;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        UserContext dbc = new UserContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IHrUnitOfWork _hrUnitOfWork;
        string serverMapPath = "~/Files/uploadercash/";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }

        public AccountController()
        {
        }
        public AccountController(IHrUnitOfWork hrUnitOfWork) : base(hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public void CloseUser()
        {
            var db = new UserContext();

            var getid = Session["getUserId"].ToString();

            var user = db.UserLogs.Where(b => b.UserId == getid).Select(a => a).OrderByDescending(b => b.Id).FirstOrDefault();


            var duration = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).Subtract(Convert.ToDateTime(user.StartTime));
            user.EndTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            user.Duration = duration;
            db.SaveChanges();
            AuthenticationManager.SignOut();
        }

        //
        // GET: /Account/Login

        public ActionResult ErrorMessage()
        {

            return View();
        }

        [AllowAnonymous]
        public async Task<bool> DoesPasswordValid(string userName, string id)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(userName);

            if (!(UserManager.CheckPassword(user, id)))
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> BlockUser(string name)
        {

            ApplicationUser user = await UserManager.FindByNameAsync(name);
            user.Locked = true;
            IdentityResult result = await UserManager.UpdateAsync(user);
            return Json(result);

        }

        //Account/GetUserObj

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateUserObj(string id, string reset, string resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (resetPassword == "true" && reset != null)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(reset);
                user.ResetPassword = false;
                UserManager.Update(user);

                var model = new LoginViewModel() { UserName = user.UserName, Password = reset, RememberMe = false };
                return await Login(model, null);

            }

            return RedirectToAction("Login", "Account");
        }


        [AllowAnonymous]
        public ActionResult GetUserObj(string id)
        {

            var db = new UserContext();

            var user = (from p in db.Users
                        where p.UserName == id
                        select new
                        {
                            p.UserName,
                            p.Locked,
                            p.Id,
                            p.ResetPassword,
                            p.PasswordHash,
                            p.Language,
                            p.DefaultCompany
                        }).SingleOrDefault();

            int numberofBlock = user == null ? 5 : _PersonSetup.MaxPassTrials;
            string Language = user?.Language != null ? user.Language : "en-GB";
            string invalidusername = Db.MsgUtils.Instance.Trls(Language, "InvalidUserName");
            string invalidPassword = Db.MsgUtils.Instance.Trls(Language, "InvalidPassword");
            string Block = Db.MsgUtils.Instance.Trls(Language, "Block");
            string Notmatches = Db.MsgUtils.Instance.Trls(Language, "passandconfirmntmatch");
            return Json(new { user = user, invalidname = invalidusername, invalidpass = invalidPassword, Block = Block, Notmatches = Notmatches, NumberBlock = numberofBlock }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if (Session["getUserId"] != null)
                CloseBrowser();
            return PartialView("Loginform");
            //return View();
        }

        public List<string> WriteEmpFile(int EmpId, int CompanyId)
        {
            var comdocs = _hrUnitOfWork.PeopleRepository.GetEmpDocsView(EmpId, CompanyId);
            
            var urlDic = new List<string>();
            string fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                if (comdocs["ComoanyLogo"] != null)
                {
                    string ComoanyLogofullpath = Server.MapPath(serverMapPath + comdocs["ComoanyLogo"].name);
                    if (!System.IO.File.Exists(ComoanyLogofullpath))
                    {
                        System.IO.File.WriteAllBytes(ComoanyLogofullpath, comdocs["ComoanyLogo"].file_stream);
                    }
                    urlDic.Add(serverMapPath + comdocs["ComoanyLogo"].name);

                }
                else
                {
                    urlDic.Add("");
                }
                if (comdocs["EmployeePic"] != null)
                {
                    string EmployeePicfullpath = Server.MapPath(serverMapPath + comdocs["EmployeePic"].name);
                    if (!System.IO.File.Exists(EmployeePicfullpath))
                    {
                        System.IO.File.WriteAllBytes(EmployeePicfullpath, comdocs["EmployeePic"].file_stream);
                    }
                    urlDic.Add(serverMapPath + comdocs["EmployeePic"].name);
                }
                else
                {
                    urlDic.Add("");
                }


            }
            return urlDic;
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [RedirectOnError]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            model.Culture = model.Culture == "ar-EG" ? "ar-EG" : "en-GB";
            if (!ModelState.IsValid)
            {
                
                foreach (var item in ModelState)
                {
                    if (ModelState["UserName"].Errors.Count > 0)
                    {
                        var req = item.Value.Errors.SingleOrDefault(a => a.ErrorMessage == "The UserName field is required.");
                        item.Value.Errors.Remove(req);
                        ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("userNameRequired", model.Culture));
                        

                    }
                    else if (ModelState["Password"].Errors.Count > 0 )
                    {
                        var req = item.Value.Errors.SingleOrDefault(a => a.ErrorMessage == "The Password field is required.");
                        item.Value.Errors.Remove(req);
                        ModelState.AddModelError("Password", MsgUtils.Instance.Trls("passWordRequired", model.Culture));
                    }
                }
                return PartialView("Loginform", model);
            }

           // #region License
           // string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"~/App_Data/App.txt"));
           // #endregion

            var db = new UserContext();
            

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("Password", MsgUtils.Instance.Trls("incorrectError", model.Culture));
                return PartialView("Loginform", model);
            }

            var excluded = new string[] { "admin", "hradmin" };

            // Check employee is active
            if (!excluded.Contains(model.UserName.ToLower()))
            {
                if (user.EmpId == null)
                {
                    ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("assignedError", model.Culture));
                    return PartialView("Loginform", model);
                }
                else
                {
                    int active = db.Database.SqlQuery<int>("select 1 from Assignments A where A.EmpId = " + user.EmpId + " and (CONVERT(date, GetDate()) BETWEEN A.AssignDate And A.EndDate) And A.sysAssignStatus = 1").FirstOrDefault();
                    if (active != 1)
                    {
                        ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("activateError", model.Culture) ); //"Employee is not active"
                        return PartialView("Loginform", model);
                    }
                }
            }




            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    if (user.ResetPassword == true)
                    {
                        string vCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        if (vCode != string.Empty)
                        {
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            AuthenticationManager.SignOut();
                            Session.Abandon();
                            Session.Clear();
                            return PartialView("ResetPassword", new ResetPasswordViewModel { UserName = model.UserName, Code = vCode, OldPassword = model.Password });
                        }
                        ModelState.AddModelError("Password", MsgUtils.Instance.Trls("incorrectError", user.Language)); //"Incorrect UserName Or Password");
                        return PartialView("Loginform", model);

                    }
                    Session["getUserId"] = user.Id.ToString();
                    Response.Cookies["userName"].Expires = DateTime.Now.AddDays(-1);
                    var domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    Response.Cookies["password"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(new HttpCookie("userName", user.UserName) { Expires = DateTime.Now.AddDays(7) });
                    if (model.RememberMe == true)
                    {
                        Response.Cookies.Add(new HttpCookie("password", model.Password) { Expires = DateTime.Now.AddDays(7) });
                       
                    }
                    if (user.EmpId != null && user.DefaultCompany != null)
                    {
                        var locaName = _hrUnitOfWork.PeopleRepository.GetEpmLocalname((int)user.EmpId, user.Language);
                        //Response.Cookies.Add(new HttpCookie("userName", user.UserName) { Expires= DateTime.Now.AddDays(7)});
                        Response.Cookies.Add(new HttpCookie("culture", user.Language) { Expires = DateTime.Now.AddDays(7) });
                        Response.Cookies.Add(new HttpCookie("userEmpId", user.EmpId.ToString()) { Expires = DateTime.Now.AddDays(7) });
                        Response.Cookies.Add(new HttpCookie("userCompanyId", User.Identity.GetDefaultCompany().ToString()) { Expires = DateTime.Now.AddDays(7) });
                        Response.Cookies.Add(new HttpCookie("localName", locaName) { Expires = DateTime.Now.AddDays(7) });
                        if (user.LastLogin != null)
                        {
                            Response.Cookies.Add(new HttpCookie("lastLogin", user.LastLogin.Value.ToString("MM/dd/yyyy HH:mm:ss")) { Expires = DateTime.Now.AddDays(7) });
                        }
                        Response.Cookies.Add(new HttpCookie("welcomeMsg", Db.MsgUtils.Instance.Trls(user.Language, "Welcome")) { Expires = DateTime.Now.AddDays(7) });
                    }

                    //DateTime? data = Convert.ToDateTime(DateTime.Now.ToString());
                    UserLog loggedUser = new UserLog();
                    loggedUser.UserId = user.Id;
                    loggedUser.LogEvent = 1;
                    loggedUser.CompanyId = User.Identity.GetDefaultCompany();
                    loggedUser.StartTime = DateTime.Now; //Convert.ToDateTime(data.Value.ToString("H:mm"));

                    db.UserLogs.Add(loggedUser);
                    int res = await db.SaveChangesAsync();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("lockoutError", model.Culture));
                    return PartialView("Loginform", model);
                case SignInStatus.Failure:
                    
                    var msg = MsgUtils.Instance.Trls("incorrectError", model.Culture);
                    ModelState.AddModelError("Password", msg);

                    return PartialView("Loginform", model);
                default:
                    msg = MsgUtils.Instance.Trls("incorrectError", model.Culture);
                    ModelState.AddModelError("Password", msg);

                    return PartialView("Loginform", model);
            }


        }

        async Task<List<int>> CheckLicense(int cusomerId)
        {
            var features = new List<int>();

            using (var client = new HttpClient())
            {
                ///initialize HttpClient instance
                client.BaseAddress = new Uri("http://localhost:2550"); //$$##ToDo: Change URI
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ///return List of Customer Features
                ///Post
                HttpResponseMessage response = await client.PostAsJsonAsync("/CustFeatures/PostFeatureList", new { CustomerId = cusomerId });
                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                    features = await response.Content.ReadAsAsync<List<int>>();
                else
                    features = null;

                return features;
            }
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // WALEED 22-05-2015
                // first check culture file is exist
                string messages = "";

                if (!System.IO.File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Scripts\\cultures\\kendo.culture." + model.Culture + ".min.js")))
                {
                    ModelState.AddModelError("", "kendo.culture." + model.Culture + ".min.js" + " is not exist, check your system administrator");
                    return View(model);
                }

                // second check message file is exist
                if (System.IO.File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Scripts\\messages\\kendo.messages." + model.Culture + ".min.js")))
                    messages = model.Culture;
                // get default message file for this language
                else
                {
                    var files = System.IO.Directory.GetFileSystemEntries(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Scripts\\messages"), "kendo.messages." + model.Culture.Split('-')[0] + "-*.min.js");
                    if (files.Count() == 0)
                    {
                        ModelState.AddModelError("", "messages file is not defined this language, please select other language or contact with system administrator");
                        return View(model);
                    }

                    messages = files[0].Split('.')[2];
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Culture = model.Culture, Messages = messages };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindByEmailAsync(model.Email);
                var user = dbc.Users.FirstOrDefault(u => u.UserName == model.Username && u.Email == model.Email);
                var val = Request.Cookies["culture"];
                var culture = (HttpContext.Request.Cookies["culture"] != null) ? Request.Cookies["culture"].Value : "en-GB";

                //|| !(await UserManager.IsEmailConfirmedAsync(user.Id))
                if (user == null)
                {
                    ModelState.AddModelError("Email", MsgUtils.Instance.Trls("incorrectuser", culture));
                    return PartialView("ForgotPassword", model);
                    // Don't reveal that the user does not exist or is not confirmed
                    
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, UserName = user.UserName, code = code }, protocol: Request.Url.Scheme);
                EmailAccount emailAcc = HrUnitOfWork.Repository<EmailAccount>().FirstOrDefault();
                emailAcc.EnableSsl = false;
                string Send = Db.Persistence.Services.EmailService.SendEmail(emailAcc, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", user.Email, user.UserName, null, null);
                if (Send == "Error")
                {
                    return PartialView("Error");
                }

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }


            // If we got this far, something failed, redisplay form
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string UserName, string Code)
        {
            return Code == null ? PartialView("Error") : PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                
                return PartialView("ResetPassword", model);
            }
            //var db = new UserContext();
            var user = dbc.Users.FirstOrDefault(us => us.UserName == model.UserName); //await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (model.Password == model.OldPassword)
            {
                ModelState.AddModelError("Password", MsgUtils.Instance.Trls("dublicatePassword",user.Language));
                return PartialView("ResetPassword", model);
            }

            if (model.Password == model.ConfirmPassword)
            {
                //var res = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.Password);
                var res = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

                if (res.Errors.Count() > 0)
                {
                    var ErrList = res.Errors.ToList();
                    foreach (var item in ErrList)
                    {
                        ModelState.AddModelError("Password", item);
                    }
                    return PartialView("ResetPassword", model);
                }
                if (user.ResetPassword == true)
                {
                    user.ResetPassword = false;
                    int returnData = await dbc.SaveChangesAsync();
                    if (returnData <= 0)
                    {
                        ModelState.AddModelError("Password", MsgUtils.Instance.Trls("erroroccurred",user.Language));
                        return PartialView("ResetPassword", model);
                    }
                }

            }
            if (user.EmpId != null && user.DefaultCompany != null)
            {
                var locaName = _hrUnitOfWork.PeopleRepository.GetEpmLocalname((int)user.EmpId, user.Language);
                Response.Cookies.Add(new HttpCookie("userName", user.UserName) { Expires = DateTime.Now.AddDays(7) });
                Response.Cookies.Add(new HttpCookie("culture", user.Language) { Expires = DateTime.Now.AddDays(7) });
                Response.Cookies.Add(new HttpCookie("userEmpId", user.EmpId.ToString()) { Expires = DateTime.Now.AddDays(7) });
                Response.Cookies.Add(new HttpCookie("userCompanyId", User.Identity.GetDefaultCompany().ToString()) { Expires = DateTime.Now.AddDays(7) });
                Response.Cookies.Add(new HttpCookie("localName", locaName) { Expires = DateTime.Now.AddDays(7) });
                if (user.LastLogin != null)
                {
                    Response.Cookies.Add(new HttpCookie("lastLogin", user.LastLogin.Value.ToString("MM/dd/yyyy HH:mm:ss")) { Expires = DateTime.Now.AddDays(7) });
                }
                Response.Cookies.Add(new HttpCookie("welcomeMsg", Db.MsgUtils.Instance.Trls(user.Language, "Welcome")) { Expires = DateTime.Now.AddDays(7) });


            }
            return RedirectToAction("ResetPasswordConfirmation", "Account");
            
        }


        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public async Task<ActionResult> LogOff()
        {
            var db = new UserContext();
            var LoginUser = db.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
            if (LoginUser != null)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignOut();
                Session.Abandon();
                Session.Clear();

                var userLog = db.UserLogs.Where(b => b.UserId == LoginUser.Id && b.EndTime == null).OrderByDescending(b => b.Id).FirstOrDefault();
                if (userLog == null)
                {
                    View("Error");
                }

                userLog.EndTime = DateTime.Now;
                TimeSpan duration = userLog.EndTime.Value.Subtract(userLog.StartTime);
                userLog.Duration = duration;
                LoginUser.LastLogin = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                int res = await db.SaveChangesAsync();

              
            }
           
            return Json("Success");
        }
        [HttpGet]
        public ActionResult SessionOff()
        {
            if (User.Identity.IsAuthenticated)
                Session["getUserId"] = User.Identity.GetUserId();
            AuthenticationManager.SignOut();
            //return View("HrLogin");
            return View();
        }
        public ActionResult ServerSessionOff()
        {
            //return View("HrLogin");
            return View();
        }
        [HttpPost]
        public ActionResult CloseBrowser()
        {
            if (Session["getUserId"] == null) return Json("Success");

            var db = new UserContext();
            var getid = Session["getUserId"].ToString();
            var user = db.UserLogs.Where(b => b.UserId == getid).Select(a => a).OrderByDescending(b => b.Id).FirstOrDefault();
            var duration = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).Subtract(Convert.ToDateTime(user.StartTime));
            user.EndTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            user.Duration = duration;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Models.Utils.LogError(ex.Message);
            }
            AuthenticationManager.SignOut();
            Session.Clear();
            return Json("Success");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}