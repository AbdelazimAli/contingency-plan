using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model.ViewModel;
using System.Threading.Tasks;
using Interface.Core;
using System.DirectoryServices.AccountManagement;
using Model.Domain;
using WebApp.Extensions;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web;
using System;
using System.Linq.Dynamic;
using Model.ViewModel.Personnel;
using Model.ViewModel.Administration;
using WebApp.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Data.Entity;

namespace WebApp.Controllers
{
    public class UsersController : BaseController
    {
        UserContext db = new UserContext();
        
        private RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> RoleManager;
        private ApplicationUserManager _userManager;
        private IHrUnitOfWork _hrUnitOfWork;
  
        public UsersController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {

            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext("HrContext")));
           _hrUnitOfWork = unitOfWork;

        }

        #region Rols by Mamdouh
        public ActionResult Role(int id)
        {         
           return View();
        }
        public ActionResult ImportUsers()
        {         
            var context = new PrincipalContext(ContextType.Domain);

            DataSource<DomainUsersViewModel> Source = new DataSource<DomainUsersViewModel>();
            List<DomainUsersViewModel> Users = new List<DomainUsersViewModel>();
            GroupPrincipal specificgroup = GroupPrincipal.FindByIdentity(context,"Users");
            if (specificgroup.Members.Count >= 0)
            {
                foreach (var groupName in specificgroup.Members.Select(a => a.Name))
                {

                    GroupPrincipal KnownGroup = GroupPrincipal.FindByIdentity(context, groupName);

                    if (KnownGroup != null)
                    {
                        var Memberscollection = KnownGroup.Members.Select(a => new { a.DisplayName, a.SamAccountName, a.Name, a.UserPrincipalName, a.Guid, DomainName = a.Context.Name }).ToList(); //get All members data on this group

                        foreach (var User in Memberscollection)
                        {
                            DomainUsersViewModel usermodel = new DomainUsersViewModel()
                            {
                                Name = User.Name,
                                Email = User.UserPrincipalName,
                                AccountName = User.SamAccountName,
                                Id = User.Guid.ToString(),
                                DomainName = context.ConnectedServer

                            };
                            if (!db.Users.Select(a => a.Id).Contains(User.Guid.ToString()))
                                Users.Add(usermodel);
                        }
                    }
                }
            } else {
                List<Error> errors = new List<Error>();
                List<ErrorMessage> errorMessages = new List<ErrorMessage>();
                foreach (var errormessage in specificgroup.Members)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = "No Users In This Domain" };
                    errorMessages.Add(errormessa);
                }
                Error err = new Error() { errors = errorMessages };

                errors.Add(err);
                Source.Errors = errors;
                return Json(Source);
            }
              Source.Data = Users;
            
            return Json(Users,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveImportUsers(IEnumerable<DomainUsersViewModel> models)
        {
            DataSource<DomainUsersViewModel> Source = new DataSource<DomainUsersViewModel>();
            int MenuId = int.Parse(Request.QueryString["MenuId"].ToString());
            var Arr = GetWhereClause(MenuId).Split('=');
            List<DomainUsersViewModel>  Data = new List<DomainUsersViewModel>();
            Source.Errors = new List<Error>();
            int CompanyId = User.Identity.GetDefaultCompany();
            if (models.Count() > 0)
            {
                foreach (var item in models)
                {
                    if (item.Checked == true)
                    {
                        ApplicationUser user = new ApplicationUser()
                        {
                            Id = item.Id,
                            Email = item.Email,
                            UserName = item.AccountName,
                            DefaultCompany = CompanyId == 0 ? db.Companies.FirstOrDefault(a => a.Id != 0)?.Id : CompanyId,
                            ResetPassword = true
                        };
                        var result = await _userManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            _userManager.AddToRole(user.Id, "Employee");
                            Data.Add(item);
                        }
                        else
                        {
                            List<Error> errors = new List<Error>();
                            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
                            foreach (var errormessage in result.Errors)
                            {
                                ErrorMessage errormessa = new ErrorMessage() { message = errormessage.ToString() };
                                errorMessages.Add(errormessa);
                            }
                            Error err = new Error() { errors = errorMessages };
                            Source.Errors.Add(err);
                        }
                    }
                }
                Source.Data = Data;
                Source.Total = Data.Count;
               
                return Json(Source);
            }
            return RedirectToAction("Index");
        }
        public ActionResult ReadRoles()
        {
            var roles = db.Roles.Select(r => new RoleUserViewModel { Id = r.Id, Name = r.Name });
            return Json(roles, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateRole(IEnumerable<RoleUserViewModel> models)
        {
            var result = new List<IdentityRole>();
            var datasource = new DataSource<RoleUserViewModel>();
            datasource.Data = models;

            //  Iterate all updated rows which are posted by the PageDiv
            foreach (RoleUserViewModel m in models)
            {

                var role = new IdentityRole(m.Name);
                RoleManager.Create(role);
                result.Add(role);
            }

            datasource.Data = (from m in models
                               join r in result on m.Name equals r.Name
                               select new RoleUserViewModel
                               {
                                   Id = r.Id,
                                   Name = m.Name
                               })
                               .ToList();

            return Json(datasource.Data);
        }
        public ActionResult UpdateRole(IEnumerable<RoleUserViewModel> models)
        {

            var datasource = new DataSource<RoleUserViewModel>();
            datasource.Data = models;
            if (ModelState.IsValid)
            {
                foreach (RoleUserViewModel m in models)
                {
                    var Role = new IdentityRole
                    {
                        Id = m.Id,
                        Name = m.Name

                    };

                    RoleManager.Update(Role);

                }
            }

            return Json(datasource.Data);
        }
        public ActionResult DeleteRole(IEnumerable<RoleUserViewModel> models)
        {
            var datasource = new DataSource<RoleUserViewModel>();

            if (ModelState.IsValid)
            {
                foreach (RoleUserViewModel m in models)
                {
                    //  var role = new IdentityRole(m.Id);
                    var role = RoleManager.Roles.Where(r => r.Id == m.Id).Single();
                    RoleManager.Delete(role);

                }

            }

            datasource.Data = models;
            return Json(datasource.Data);
        }
        public ActionResult GetUserRoles(string id)
        {
            //bool InSSn;
            //IQueryable<UserRoleViewModel> UserRoles;


            //if (id != "")
            //{
            //    InSSn = db.Users.FirstOrDefault(a => a.Id == id);
            //    if (InSSn)
            //    {
            //        UserRoles = db.ApplicationRoles
            //        .Select(r => new UserRoleViewModel
            //        {
            //            Id = r.Id,
            //            Name = r.Name,
            //            IsChecked = r.Users.FirstOrDefault(u => u.UserId == id) != null
            //        });
            //    }
            //    else
            //    {

            //        UserRoles = db.Roles.Where(a=>a.Name != "Developer").Select(r => new UserRoleViewModel
            //        {
            //            Id = r.Id,
            //            Name = r.Name,
            //            IsChecked = r.Users.FirstOrDefault(u => u.UserId == id) != null
            //        });
            //    }


            //}
            //else
            //{
            //    if (SSRole != null && SSRole != "undefined")
            //        InSSn = Convert.ToBoolean(SSRole);
            //    else
            //        InSSn = true;

            //    if (InSSn)
            //    {
            //        UserRoles = db.ApplicationRoles.Where(a => a.SSRole == true)
            //        .Select(r => new UserRoleViewModel
            //        {
            //            Id = r.Id,
            //            Name = r.Name,
            //            IsChecked = r.Users.FirstOrDefault(u => u.UserId == id) != null
            //        });
            //    }
            //    else
            //    {
            //        UserRoles = db.Roles.Where(a => a.Name != "Developer").Select(r => new UserRoleViewModel
            //       {
            //           Id = r.Id,
            //           Name = r.Name,
            //           IsChecked = r.Users.FirstOrDefault(u => u.UserId == id) != null
            //       });
            //    }
            //}
           
            return Json(db.UserCompanyRoles.Where(a => a.UserId == id).Select(a => new UserRoleViewModel { Id = a.Role.Id, Name = a.Role.Name}).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadUserRoles()
        {
            return View();
        }
        public ActionResult GetCompanies(string id)
        {
            var Usercompany = db.UserCompanyRoles.Where(r => r.UserId == id)
                    .Select(r => new UserCompanyViewModel
                    {
                        Id = r.Id,
                        CompanyId = r.CompanyId,
                        RoleId = r.RoleId
                    });

            return Json(Usercompany, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadUserCompany()
        {
            return View();
        }
        #endregion
        #region User by Seham
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReadUsers(int MenuId)
        {
            //string whereclause = GetWhereClause(MenuId);
            int CompanyId = User.Identity.GetDefaultCompany();
            var query = from u in db.Users
                        where u.UserName != "admin" && u.UserName != "hradmin"
                        join c in db.Companies on u.DefaultCompany.Value equals c.Id into g1
                        from c in g1.DefaultIfEmpty()
                        join ul in db.UserLogs on new { p1 = u.Id, p2 = u.LastLogin.Value } equals new { p1 = ul.UserId, p2 = ul.StartTime } into g2
                        from ul in g2.DefaultIfEmpty()
                        select new UsersViewModel
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            Domain = u.NetworkDomain,
                            DefaultCompanyName = c.Name,
                            DefaultCompany = u.DefaultCompany.Value,
                            LastLogin = u.LastLogin,
                            DefaultLangauge = u.Culture,
                            Duration = ul.Duration == null ? "" : ul.Duration.Value.Hours.ToString() + ":" + ul.Duration.Value.Minutes.ToString() + ":" + ul.Duration.Value.Seconds.ToString()
                        };

            //var query = users;
            //if (whereclause.Length > 0)
            //{
            //    try
            //    {
            //        query = users.Where(whereclause);
            //    }
            //    catch (Exception ex)
            //    {
            //        TempData["Error"] = ex.Message;
            //        Models.Utils.LogError(ex.Message);
            //        return Json("", JsonRequestBehavior.AllowGet);
            //    }
            //}

            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteUser(string Id)
        {
            var User = db.Users.Where(a => a.Id == Id).FirstOrDefault();
            var userLog = db.UserLogs.Where(a => a.UserId == User.Id).ToList();
            if(userLog.Count()>0)
            {
                db.UserLogs.RemoveRange(userLog);
            }

            db.Users.Remove(User);
            db.SaveChanges();
            return Json("OK",JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckUsers(string UserName)
        {
            string message = "OK";
            if (UserName == User.Identity.Name || UserName == "hradmin")
                message = "Error";
            else
                message = "OK";
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        #endregion
        void FillViewBag()
        {
            string culture = User.Identity.GetLanguage();
            if (culture.Substring(0, 2) == "ar")
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.NameAr });
            else
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.Name });

            ViewBag.Culture = _hrUnitOfWork.AudiTrialRepository.ReadLanguage();
            ViewBag.Company = _hrUnitOfWork.CompanyRepository.CompanyList(culture).Select(c => new { id = c.Id, name = c.Name });
            ViewBag.Emp = _hrUnitOfWork.PeopleRepository.GetAllPeoples(User.Identity.GetLanguage()).Select(a => new { id = a.Id, name = a.Name, a.PicUrl, a.Icon });
        }
        #region Userform by Shaddad

        [HttpGet]
        public ActionResult Details(string id)
        {
            FillViewBag();
           
            //ViewBag.isSuper = true;
            ViewBag.loguser = User.Identity.GetUserId();

            if (id != "0")
            {
                ApplicationUser user = _userManager.FindById(id);
                UserViewModel model = MapViewModelWithModel(user);
                return View(model);
            }
            else
                return View(new UserViewModel());

        }

        [HttpGet]
        public ActionResult EmpUserProfile(int id)
        {
            string culture = User.Identity.GetLanguage();
            if (culture.Substring(0, 2) == "ar")
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.NameAr });
            else
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.Name });

            ViewBag.Culture = _hrUnitOfWork.AudiTrialRepository.ReadLanguage();
            ViewBag.Company = _hrUnitOfWork.CompanyRepository.CompanyList(culture).Select(c => new { value = c.Id, text = c.Name });
            ViewBag.Roles = db.Roles.Select(r => new { value = r.Id, text = r.Name });
            //ViewBag.isSuper = true;
            ViewBag.loguser = User.Identity.GetUserId();
            ViewBag.EmpId = id;

            ApplicationUser user = db.Users.Where(a => a.EmpId == id).FirstOrDefault();
            if (user != null)
            {
                UserViewModel model = MapViewModelWithModel(user);
                return View(model);
            }
   
            return View(new UserViewModel());
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
        public async Task<ActionResult> UserProfile(UserViewModel model, string Id, OptionsViewModel moreInfo, UserCompaniesVM grid1)
        {
            List<Error> Errors = new List<Error>();
         
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


                ApplicationUser user = _userManager.FindById(Id);
                IdentityResult res;
                var loginId = User.Identity.GetUserId();
                //ApplicationUser loginUser = _userManager.FindById(User.Identity.GetUserId());
                model.Messages = getMessage(model.Culture);

                //Update User
                if (user != null)
                {
                    //if (!loginUser.SuperUser)
                    //    model.SuperUser = user.SuperUser; //Keep Original Value
                    //else
                    //{
                    //    int supersCount = _userManager.Users.Where(u => u.SuperUser).Count();
                    //    if (user.Id == loginUser && !model.SuperUser && supersCount <= 1) 
                    //    {
                    //        ErrorMessage mess = new ErrorMessage()
                    //        {
                    //            field = "SuperUser",
                    //            message = MsgUtils.Instance.Trls("CantRemoveSuperUser")
                    //        };
                    //        Error er = new Error();
                    //        er.errors.Add(mess);
                    //        return Json(Errors);
                    //    }
                    //}

                    string OldCulture = user.Language;
                    string OldTimeZone = user.TimeZone;
                    int OldCompany = user.DefaultCompany.Value;
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = user,
                        Source = model,
                        ObjectName = "UserProfile",
                        Options = moreInfo
                    });

                    user.Language = model.Language == null ? "en-GB" : model.Language;
                    user.Messages = getMessage(model.Culture);
                    model.NewUser = false;
                    user.LockoutEnabled = model.LockoutEnabled;

                    res = await _userManager.UpdateAsync(user);

                    if ((user.Id == loginId) && (user.TimeZone != OldTimeZone) && (OldCulture != model.Language) && (user.Language != null) && (user.TimeZone != null))
                        ChangeAll(model.TimeZone, model.Language, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (user.Language != null) && (OldCulture != model.Language))
                        ChangeCulture(model.Language, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (user.TimeZone != null) && (OldTimeZone != model.TimeZone))
                        ChangeTimeZone(model.TimeZone, model.DefaultCompany.Value);
                    else if ((user.Id == loginId) && (OldCompany != model.DefaultCompany))
                        ChangeDefaultCompany(model.DefaultCompany.Value);
                }
                else //New User
                {
                    user = new ApplicationUser();
                    model.Id = user.Id;
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = user,
                        Source = model,
                        ObjectName = "UserProfile",
                        Options = moreInfo
                    });
                    user.Messages = getMessage(model.Culture);
                    user.Language = model.Language == null ? "en-GB" : model.Language;
                    model.NewUser = true;
                    user.LockoutEnabled = model.LockoutEnabled;
                    user.DefaultCompany = User.Identity.GetDefaultCompany();

                    if (model.Password == null)
                    {
                        user.ResetPassword = true;
                        res = await _userManager.CreateAsync(user);
                    }
                    else
                        res = await _userManager.CreateAsync(user, model.Password);

                    if (res.Errors.Count() > 0)
                    {

                        var err = res.Errors.FirstOrDefault().Split(' ')[0];
                        if (err == "Passwords")
                            ModelState.AddModelError("Password", MsgUtils.Instance.Trls("Passwordmustnotlest6"));
                        else if (err == "User")
                            ModelState.AddModelError("UserName", MsgUtils.Instance.Trls("Namemustcontaindigitorchar"));
                        else
                            ModelState.AddModelError("", MsgUtils.Instance.Trls(res.Errors.FirstOrDefault()));

                        return Json(Models.Utils.ParseFormErrors(ModelState));

                    }
                }

                //Errors = await SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), model);
                //if (Errors.Count > 0) return Json(Errors.First().errors.First().message);

                Errors = SaveGrid(grid1, ModelState.Where(a => a.Key.Contains("grid1")), user);
                if (Errors.Count > 0) return Json(Errors.First().errors.First().message);


                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    ErrorMessage mess = new ErrorMessage()
                    {
                        message = MsgUtils.Instance.Trls(ex.Message)
                    };
                    //Error er = new Error();
                    //er.errors.Add(mess);
                    //return Json(errors.First().errors.First().message);
                }

 

                //Validation
                //if (res.Errors.Count() > 0)
                //{
                //    foreach (var error in res.Errors)
                //    {
                //        ErrorMessage mess = new ErrorMessage()
                //        {
                //            field = "SuperUser",
                //            message = MsgUtils.Instance.Trls(error)
                //        };
                //        Error er = new Error();
                //        er.errors.Add(mess);
                //        return Json(Errors);
                //    }
                //}
                //else
                //{
                //    if (user.EmpId != null)
                //    {
                //        var person = _hrUnitOfWork.Repository<Person>().FirstOrDefault(a => a.Id == user.EmpId);
                //        person.WorkEmail = user.Email;
                //        person.WorkTel = user.PhoneNumber;
                //        _hrUnitOfWork.PeopleRepository.Attach(person);
                //        _hrUnitOfWork.PeopleRepository.Entry(person).State = System.Data.Entity.EntityState.Modified;
                //    }
                //    SaveChanges(user.Language);
                //}

                return Json("OK," + ((new JavaScriptSerializer()).Serialize(model)));
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        //private async Task<List<Error>> SaveGrid1(UserRoleVm grid1, IEnumerable<KeyValuePair<string, ModelState>> state, UserViewModel record)
        //{
        //    List<Error> errors = new List<Error>();


        //    IdentityResult re = new IdentityResult();
        //    // updated records
        //    if (grid1.updated != null)
        //    {
        //        foreach (var model in grid1.updated)
        //        {
        //            if (model.IsChecked)
        //            {
        //                if (!_userManager.IsInRole(record.Id, model.Name))
        //                    re = await _userManager.AddToRoleAsync(record.Id, model.Name);
        //            }
        //            else
        //            {
        //                if (_userManager.IsInRole(record.Id, model.Name))
        //                    re = await _userManager.RemoveFromRoleAsync(record.Id, model.Name);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (record.NewUser)
        //        {
        //            await _userManager.AddToRoleAsync(record.Id, "Employee");
        //        }
        //    }
        //    if (re.Errors.Count() > 0)
        //    {
        //        foreach (var error in re.Errors)
        //        {
        //            ErrorMessage mess = new ErrorMessage()
        //            {
        //                message = MsgUtils.Instance.Trls(error)
        //            };
        //            Error er = new Error();
        //            er.errors.Add(mess);
        //            return errors;
        //        }
        //    }

        //    return errors;
        //}
        private List<Error> SaveGrid(UserCompaniesVM grid, IEnumerable<KeyValuePair<string, ModelState>> state, ApplicationUser user)
        {
            List<Error> errors = new List<Error>();

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
                    AutoMapper(new Models.AutoMapperParm { Destination = companyrole, Source = model, Transtype = TransType.Update });

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
                    AutoMapper(new Models.AutoMapperParm { Destination = companyrole, Source = model, Transtype = TransType.Insert });
                    companyrole.User = user;
                    db.UserCompanyRoles.Add(companyrole);
                }
            }

            return errors;
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
            }

            base.Dispose(disposing);
        }

        #endregion
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

        #region
        [HttpGet]
        public ActionResult ViewProfile()
        {
            ApplicationUser user = _userManager.FindById(User.Identity.GetUserId());
            ProfileViewModel model = new ProfileViewModel() {
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
            //if (!user.SuperUser)
            //    ViewBag.isSuper = false;

            ViewBag.adminUser = User.Identity.Name == "Admin";

            TempData["id"] = user.Id;
            FillViewBag();
            ViewBag.UserCompanies = db.UserCompanyRoles.Where(c => c.UserId == user.Id).Select(c => new { id = c.CompanyId, name = c.Company.Name }).ToList();

            return model == null ? (ActionResult)HttpNotFound() : View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SaveProfile(ProfileViewModel profile, OptionsViewModel moreInfo)
        {
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
                AutoMapper(new AutoMapperParm() { ObjectName = "Profile", Source = profile, Destination = user, Options = moreInfo });
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
                    var CorrectoldPass = await _userManager.CheckPasswordAsync(user, profile.OldPassword);
                    if (CorrectoldPass)
                        result = await _userManager.ChangePasswordAsync(userId, profile.OldPassword, profile.NewPassword);
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
                    if (user.EmpId != null)
                    {
                        var person = _hrUnitOfWork.Repository<Person>().FirstOrDefault(a => a.Id == user.EmpId);
                        person.WorkEmail = user.Email;
                        person.WorkTel = user.PhoneNumber;
                        _hrUnitOfWork.PeopleRepository.Attach(person);
                        _hrUnitOfWork.PeopleRepository.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    }
                    SaveChanges(user.Language);
                   
                    if (Changed)
                        message = "OK,"+((new JavaScriptSerializer()).Serialize(profile));

                    return Json(message);
                }
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        protected void ChangeCulture(string changeLang,int changecompany)
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
        protected void ChangeTimeZone(string timeZone, int changecompany)
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
        protected void ChangeAll(string timeZone,string Culture, int changecompany)
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
        protected void ChangeDefaultCompany(int CompanyId)
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
            Identity.AddClaim(new Claim("DefaultCompany", CompanyId.ToString()));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });
        }
        #endregion

        #region User Log Index
        public ActionResult LogIndex()
        {
            return View();
        }
        public ActionResult ReadUserLog()
        {
            int CompanyId = User.Identity.GetDefaultCompany();
            var query= (from u in db.UserLogs
                      join l in db.Users on u.UserId equals l.Id
                      where l.DefaultCompany.Value==CompanyId
                      select new  UserLogViewModel
                      {
                          Id=u.Id,
                          Duration=u.Duration.ToString(),
                          UserName=l.UserName,
                          StartTime=u.StartTime,
                          EndTime=u.EndTime,
                          CompanyId=u.CompanyId               
                      }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //ReadLogUser
        public ActionResult UserLog(string id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult GetUserLog(string id, int pageSize, int skip)
        {
            string filter = "";
            string Sorting = "";

            if (string.IsNullOrEmpty(id))
                id = "";
              var query = (from u in db.UserLogs
                             where u.UserId == id
                             join l in db.Users on u.UserId equals l.Id
                             select new UserLogViewModel
                             {
                                 Id = u.Id,
                                 Duration = u.Duration.ToString(),
                                 UserName = l.UserName,
                                 StartTime = u.StartTime,
                                 EndTime = u.EndTime,
                                 CompanyId = u.CompanyId
                             });


            
            query = (IQueryable<UserLogViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
            if (filter.Length > 0)
            {
                try
                {
                    query = query.Where(filter);
                }
                catch (Exception ex)
                {

                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }

            var total = query.Count();

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting).Skip(skip).Take(pageSize);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}