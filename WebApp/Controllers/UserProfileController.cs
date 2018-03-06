using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApp.Extensions;
using WebApp.Models;
using Model.ViewModel;
using System.Web.Script.Serialization;
using System.IO;
using Model.ViewModel.Administration;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Db.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Model.Domain;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Db.Persistence.BLL;
using Model.ViewModel.Personnel;

namespace WebApp.Controllers
{
    //[OutputCache(VaryByParam = "*", Duration = 60)]
    public class UserProfileController : Controller
    {
        //public JsonResult ReadRemoteList(string tableName)
        //{
        //    UserContext db = new UserContext();
        //    string lang = User.Identity.GetLanguage().Substring(0, 2) == "ar" ? "Ar" : "";
        //    var result = db.Database.SqlQuery<FormDropDown>("select c.Id id, c.Name" + lang + " name from countries c").ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        // GET: UserProfile
        public UserProfileController()
        {

        }

        [HttpGet]
        public ActionResult ViewProfile()
        {
            UserContext db = new UserContext();
            var id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Where(a => a.Id == id).FirstOrDefault();
            ProfileViewModel model = new ProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Culture = user.Language,
                TimeZone = user.TimeZone,
                DefaultCompany = user.DefaultCompany ?? default(int),
                DefaultCountry = user.DefaultCountry,
                LastLogin = user.LastLogin,
                PhoneNumber = user.PhoneNumber,
                AllowInsertCode = user.AllowInsertCode,
                Infolog = user.Infolog,
                ExportExcel = user.ExportExcel,
                ExportTo = user.ExportTo,
                LogTooltip = user.LogTooltip,
                ShutdownInMin = user.ShutdownInMin,
                UploadDocs = user.UploadDocs
            };
            
            ViewBag.adminUser = User.Identity.Name == "hradmin";
            ViewBag.isSuper = false;
            TempData["id"] = user.Id;
            return model == null ? (ActionResult)HttpNotFound() : View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SaveProfile(ProfileViewModel profile, OptionsViewModel moreInfo)
        {
            var _hrUnitOfWork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userbl = new UserBL();

            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = _userManager.FindById(userId);
                IdentityResult result;
                bool Changed = false;
                string message = "OK";
                string OldCulture = user.Language;
                string OldTimeZone = user.TimeZone;
                int OldCompany = user.DefaultCompany.Value;
                AutoMapper(new AutoMapperParm() { ObjectName = "Profile", Source = profile, Destination = user, Options = moreInfo }, _hrUnitOfWork);
                user.Language = profile.Culture == null ? "en-GB" : profile.Culture;
                user.Messages = getMessage(profile.Culture);
                user.DefaultCompany = OldCompany;
                //if (user.SuperUser)
                //{
                //    user.SuperUser = profile.SuperUser;

                //    int supersCount = _userManager.Users.Where(u => u.SuperUser).Count();
                //    if (!profile.SuperUser && supersCount <= 1)
                //    {
                //        ModelState.AddModelError("SuperUser", MsgUtils.Instance.Trls("CantRemoveSuperUser"));
                //        return Json(Models.Utils.ParseFormErrors(ModelState));
                //    }
                //}

                result = await _userManager.UpdateAsync(user);

                if ((profile.TimeZone != OldTimeZone) && (OldCulture != user.Language) && (profile.Culture != null) && (profile.TimeZone != null))
                {
                    ChangeAll(profile.TimeZone, profile.Culture, 0);
                    Changed = true;
                }
                else if ((profile.Culture != null) && (OldCulture != user.Language))
                {
                    ChangeCulture(profile.Culture, 0);
                    Changed = true;
                }
                else if ((profile.TimeZone != null) && (OldTimeZone != profile.TimeZone))
                {
                    ChangeTimeZone(profile.TimeZone, 0);
                    Changed = true;
                }
                if (result.Succeeded && profile.NewPassword != null && profile.OldPassword != null)
                {
                    if (!userbl.DuplicatePassword(ModelState, profile.NewPassword, profile.OldPassword, user.Language))
                        return Json(Models.Utils.ParseFormErrors(ModelState));

                    var CorrectoldPass = await _userManager.CheckPasswordAsync(user, profile.OldPassword);
                    if (CorrectoldPass)
                    {
                        result = await _userManager.ChangePasswordAsync(userId, profile.OldPassword, profile.NewPassword);
                        if (result.Errors.Count() > 0)
                        {
                            var ErrList = result.Errors.ToList();
                            foreach (var item in ErrList)
                            {
                                ModelState.AddModelError("ConfirmPassword", userbl.TranslateError(item, user.Language));
                            }
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", MsgUtils.Instance.Trls("notOldPassword"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
                else if (result.Succeeded && profile.NewPassword != null)
                {
                    ModelState.AddModelError("OldPassword", MsgUtils.Instance.Trls("Required") + ";");
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                else if (result.Succeeded && profile.OldPassword != null)
                {
                    ModelState.AddModelError("NewPassword", MsgUtils.Instance.Trls("Required") + ";");
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                //Validation
                if (result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                else
                {
                    //if (user.EmpId != null)
                    //{
                    //    var person = _hrUnitOfWork.Repository<Person>().FirstOrDefault(a => a.Id == user.EmpId);
                    //    person.WorkEmail = user.Email;
                    //    person.WorkTel = user.PhoneNumber;
                    //    _hrUnitOfWork.PeopleRepository.Attach(person);
                    //    _hrUnitOfWork.PeopleRepository.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    //}
                    _hrUnitOfWork.SaveChanges(user.Language);

                    if (Changed)
                        message = "OK," + ((new JavaScriptSerializer()).Serialize(profile));

                    return Json(message);
                }
            }

            userbl.CheckPasswordStrength(ModelState, User.Identity.GetLanguage());
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        [HttpGet]
        public JsonResult FillViewDropdown()
        {
            try
            {
                UserContext db = new UserContext();
                var Culture = db.Database.SqlQuery<string>("select LanguageCulture from Languages").ToList();
                var Country = db.Database.SqlQuery<FormDropDown>($"select id, (CASE WHEN '{User.Identity.GetLanguage().Substring(0, 2)}' = 'ar' THEN [NameAr] ELSE [Name] END) name from Countries").ToList();
                var TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(a => new { id = a.Id, name = a.DisplayName });
                return Json(new
                {
                    Result = true,
                    Culture = Culture,
                    Country = Country,
                    TimeZones = TimeZones
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult FillDropdownlists(string id)
        {
            try
            {
                UserContext db = new UserContext();
                var Culture = db.Database.SqlQuery<string>("select LanguageCulture from Languages").ToList();
                var Company = db.Companies.Select(r => new { value = r.Id, text = r.Name }).ToList();
                var Roles = db.Roles.Select(r => new { value = r.Id, text = r.Name }).ToList();
                var TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(a => new { id = a.Id, name = a.DisplayName });
                var Usercompany = db.UserCompanyRoles.Where(r => r.UserId == id).Select(r => new UserCompanyViewModel { Id = r.Id, CompanyId = r.CompanyId, RoleId = r.RoleId });

                return Json(new
                {
                    Result = true,
                    Culture = Culture,
                    Company = Company,
                    Roles = Roles,
                    TimeZones = TimeZones,
                    Usercompany = Usercompany
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EmpUserProfile(int id)
        {
            UserContext db = new UserContext();
            string culture = User.Identity.GetLanguage();

            //if (culture.Substring(0, 2) == "ar")
            //    ViewBag.Country = db.Database.SqlQuery<FormDropDown>("select c.Id id, c.NameAr name from countries c").ToList();
            //else
            //    ViewBag.Country = db.Database.SqlQuery<FormDropDown>("select c.Id id, c.Name name from countries c").ToList();


            ViewBag.loguser = User.Identity.GetUserId();
            ViewBag.EmpId = id;

            UserViewModel model = new UserViewModel();
            ApplicationUser user = db.Users.Where(a => a.EmpId == id).FirstOrDefault();
            if (user != null)
            {
                model = MapViewModelWithModel(user);
                //return View(model);
            }

            model.NotifyButtonLabel = MsgUtils.Instance.Trls("SendNotifyLetter", culture);
            return View(model);
        }

        private UserViewModel MapViewModelWithModel(ApplicationUser user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Infolog = user.Infolog,
                Language = user.Language,
                LastLogin = user.LastLogin,
                LogTooltip = user.LogTooltip,
                Messages = user.Messages,
                NetworkDomain = user.NetworkDomain,
                PhoneNumber = user.PhoneNumber,
                LockoutEnabled = user.LockoutEnabled,
                ResetPassword = user.ResetPassword,
                ShutdownInMin = user.ShutdownInMin,
                TimeZone = user.TimeZone,
                UploadDocs = user.UploadDocs,
                UserName = user.UserName,
                AllowInsertCode = user.AllowInsertCode,
                Culture = user.Culture,
                DefaultCompany = user.DefaultCompany,
                DefaultCountry = user.DefaultCountry,
                Email = user.Email,
                EmpId = user.EmpId,
                ExportExcel = user.ExportExcel,
                ExportTo = user.ExportTo,
                SmsNotify = user.SmsNotify,
                WebNotify = user.WebNotify,
                EmailNotify = user.EmailNotify,
                CanCustomize = user.CanCustomize,
                Developer = user.Developer
            };
        }

        [HttpPost]
        public async Task<ActionResult> UserProfile(UserViewModel model, string Id, int EmpId, OptionsViewModel moreInfo, UserCompaniesVM grid1)
        {
            var Errors = new List<Error>();
            var _hrUnitOfWork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));
            var ServerValidationEnabled = System.Configuration.ConfigurationManager.AppSettings["ServerValidationEnabled"] == "true";

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.')));
                    Errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = User.Identity.GetDefaultCompany(),
                        ObjectName = "UserProfile",
                        TableName = "AspNetUsers",
                        Columns = columns,
                        Culture = User.Identity.GetLanguage()
                    });

                    if (Errors.Count() > 0)
                    {
                        foreach (var e in Errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }

                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                var db = HttpContext.GetOwinContext().Get<UserContext>();
                var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                //var db = new UserContext();
                //var _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

                ApplicationUser user = _userManager.FindById(Id);
                IdentityResult res;
                var loginId = User.Identity.GetUserId();
                model.Messages = getMessage(model.Culture);

                //Update User
                if (user != null)
                {
                    string OldCulture = user.Language;
                    string OldTimeZone = user.TimeZone;
                    int OldCompany = user.DefaultCompany.Value;
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = user,
                        Source = model,
                        ObjectName = "UserProfile",
                        Options = moreInfo
                    }, _hrUnitOfWork);

                    user.Language = model.Language == null ? "en-GB" : model.Language;
                    user.Messages = getMessage(model.Culture);
                    model.NewUser = false;
                    user.LockoutEnabled = model.LockoutEnabled;

                    if ((user.Id == loginId) && (user.TimeZone != OldTimeZone) && (OldCulture != model.Language) && (user.Language != null) && (user.TimeZone != null))
                        ChangeAll(model.TimeZone, model.Language, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (OldCulture != null) && (OldCulture != model.Language))
                        ChangeCulture(model.Language, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (OldTimeZone != null) && (OldTimeZone != model.TimeZone))
                        ChangeTimeZone(model.TimeZone, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (OldCompany != User.Identity.GetDefaultCompany()))
                        ChangeDefaultCompany(model.DefaultCompany.Value);
                }
                else //New User
                {
                    user = new ApplicationUser { UserName = model.UserName, Email = model.Email, LockoutEnabled = model.LockoutEnabled };
                    model.NewUser = true;
                }

                // database transactions
                //var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var trans = db.Database.BeginTransaction();
                var status = (model.NewUser == true ? PersonStatus.UserProfile : 0);
                if (!model.NewUser)
                    res = await _userManager.UpdateAsync(user);
                else
                {
                    if (model.Password == null)
                    {
                        user.ResetPassword = true;
                        res = await _userManager.CreateAsync(user);
                    }
                    else
                        res = await _userManager.CreateAsync(user, model.Password);
                }

                moreInfo.VisibleColumns.Remove("Id");
                if (res.Errors.Count() > 0)
                {
                    var err = res.Errors.FirstOrDefault().Split(' ')[0];
                    if (err == "Passwords")
                        ModelState.AddModelError("Password", MsgUtils.Instance.Trls("Passwordmustnotlest6"));
                    else if (err == "User")
                        ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("Namemustcontaindigitorchar"));
                    else
                        ModelState.AddModelError("", MsgUtils.Instance.Trls(res.Errors.FirstOrDefault()));

                    trans.Rollback();
                    trans.Dispose();
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

                if (model.NewUser)
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = user,
                        Source = model,
                        ObjectName = "UserProfile",
                        Options = moreInfo
                    }, _hrUnitOfWork);

                    user.Messages = getMessage(model.Culture);
                    user.Language = model.Language == null ? "en-GB" : model.Language;
                    user.DefaultCompany = User.Identity.GetDefaultCompany();
                    user.EmpId = EmpId;
                    model.EmpId = EmpId;
                    model.DefaultCompany = user.DefaultCompany;
                    model.Id = user.Id;
                }

                SaveGrid(grid1, ModelState.Where(a => a.Key.Contains("grid1")), user, _hrUnitOfWork, db);
                try
                {
                    //if (model.NewUser)
                    //{
                    //    string Message;
                    //    SendNotifyLetterMethod(_hrUnitOfWork, user.UserName, EmpId, user.Id, out Message);
                    //}

                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    var message = _hrUnitOfWork.HandleDbExceptions(ex);
                    if (message == "Date Already Exists")
                        message = "UserHaveOnlyRole";
                    //scope.Dispose();
                    trans.Rollback();
                    trans.Dispose();
                    return Json(MsgUtils.Instance.Trls(message));
                }

                trans.Commit();
                trans.Dispose();

                if (status != PersonStatus.Done)
                {


                    var person = _hrUnitOfWork.PeopleRepository.GetPerson(model.EmpId);
                    person.Status = PersonStatus.Done;
                    model.Status = PersonStatus.Done;
                    _hrUnitOfWork.SaveChanges();
                }

                return Json("OK," + ((new JavaScriptSerializer()).Serialize(model)));
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        public JsonResult SendNotifyLetter(string UserName = "", int EmpID = 0, string ID = "")
        {
            if (string.IsNullOrEmpty(UserName) || EmpID == 0 || string.IsNullOrEmpty(ID))
            {
                return Json(new { Result = false });
            }

            var _hrUnitOfWork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));

            string Message;
            bool Result = SendNotifyLetterMethod(_hrUnitOfWork, UserName, EmpID, ID, out Message);

            return Json(new { Result = Result, Message = Message });
        }

        private bool SendNotifyLetterMethod(HrUnitOfWork _hrUnitOfWork, string UserName, int EmpID, string ID, out string ErrorMessage)
        {

            ErrorMessage = "";

            int CompanyID = User.Identity.GetDefaultCompany();
            string CurrentURL = HttpContext.Request.Url.Authority;
            string DownloadAPKUrl = System.Configuration.ConfigurationManager.AppSettings["DownloadAPKUrl"];
            DateTime Today = DateTime.Now.Date;
            string Language = HttpContext.User.Identity.GetLanguage();
            NotifyLetter NL = new NotifyLetter()
            {
                CompanyId = CompanyID,
                EmpId = EmpID,
                NotifyDate = Today,
                NotifySource = MsgUtils.Instance.Trls(Db.Persistence.Constants.Sources.UserProfile, Language),
                SourceId = ID,
                Sent = true,
                EventDate = Today,
                Description = MsgUtils.Instance.Trls("User Name", Language) + " : " + UserName + " " + MsgUtils.Instance.Trls("Website", Language) + " : " + CurrentURL + " " + MsgUtils.Instance.Trls("Download APK", Language) + " : " + DownloadAPKUrl

            };


            AddNotifyLetters AddNotifyLetters = new AddNotifyLetters(_hrUnitOfWork, NL, Language);
           bool Result= AddNotifyLetters.Run(out ErrorMessage);

            return Result;
        }
        public ActionResult GetCompanies(string id)
        {
            var db = new UserContext();
            var Usercompany = db.UserCompanyRoles.Where(r => r.UserId == id)
                    .Select(r => new UserCompanyViewModel
                    {
                        Id = r.Id,
                        CompanyId = r.CompanyId,
                        RoleId = r.RoleId
                    });

            return Json(Usercompany, JsonRequestBehavior.AllowGet);
        }

        private void SaveGrid(UserCompaniesVM grid, IEnumerable<KeyValuePair<string, ModelState>> state, ApplicationUser user, HrUnitOfWork unitOfWork, UserContext db)
        {
            // Deleted
            if (grid.deleted != null)
            {
                foreach (var model in grid.deleted)
                {
                    var companyrole = new UserCompanyRole
                    {
                        Id = model.Id
                    };

                    db.UserCompanyRoles.Remove(companyrole);
                }
            }

            // updated records
            if (grid.updated != null)
            {
                foreach (var model in grid.updated)
                {
                    var companyrole = new UserCompanyRole();
                    AutoMapper(new Models.AutoMapperParm { Destination = companyrole, Source = model, Transtype = TransType.Update }, unitOfWork);
                    companyrole.UserId = user.Id;
                    db.UserCompanyRoles.Attach(companyrole);
                    db.Entry(companyrole).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid.inserted != null)
            {
                foreach (var model in grid.inserted)
                {
                    var companyrole = new UserCompanyRole();
                    AutoMapper(new Models.AutoMapperParm { Destination = companyrole, Source = model, Transtype = TransType.Insert }, unitOfWork);
                    companyrole.User = user;
                    db.UserCompanyRoles.Add(companyrole);
                }
            }
        }
        private void AutoMapper(Models.AutoMapperParm parm, HrUnitOfWork unitOfWork)
        {
            Models.AutoMapper mapper = new Models.AutoMapper(parm, unitOfWork, User.Identity);
            mapper.Map();
        }

        #region private helper functions
        private string getMessage(string culture)
        {
            string message = culture;
            //if not exists
            if (!System.IO.File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Scripts\\messages\\kendo.messages." + culture + ".min.js")))
            {
                message = User.Identity.GetCulture().Split('-')[0];
                string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + @"\Scripts\messages", "kendo.messages." + message + "*.min.js");

                message = (files.Count() > 0 ? files.FirstOrDefault().ToString().Split('.')[2] : "ar-EG");
            }
            return message;
        }
        private void ChangeCulture(string changeLang, int changecompany)
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("Language"));
            Identity.RemoveClaim(Identity.FindFirst("Messages"));
            if (changecompany != 0)
            {
                Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
                Identity.AddClaim(new Claim("DefaultCompany", changecompany.ToString()));
            }
            Identity.AddClaim(new Claim("Messages", changeLang));
            Identity.AddClaim(new Claim("Language", changeLang));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });
        }
        private void ChangeTimeZone(string timeZone, int changecompany)
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("TimeZone"));
            if (changecompany != 0)
            {
                Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
                Identity.AddClaim(new Claim("DefaultCompany", changecompany.ToString()));
            }
            Identity.AddClaim(new Claim("TimeZone", timeZone));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });

        }
        private void ChangeAll(string timeZone, string Culture, int changecompany)
        {

            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("TimeZone"));
            Identity.RemoveClaim(Identity.FindFirst("Language"));
            Identity.RemoveClaim(Identity.FindFirst("Messages"));
            if (changecompany != 0)
            {
                Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
                Identity.AddClaim(new Claim("DefaultCompany", changecompany.ToString()));
            }
            Identity.AddClaim(new Claim("TimeZone", timeZone));
            Identity.AddClaim(new Claim("Language", Culture));
            Identity.AddClaim(new Claim("Messages", Culture));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });

        }
        private void ChangeDefaultCompany(int CompanyId)
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
            Identity.AddClaim(new Claim("DefaultCompany", CompanyId.ToString()));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });
        }

        #endregion
    }
}