using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Extensions;
using Microsoft.AspNet.Identity;
using Model.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Interface.Core;
using System;
using System.Linq.Dynamic;

namespace WebApp.Controllers
{
    public class RolesController : BaseController
    {
        private UserContext db;
        private RoleManager<IdentityRole> RoleManager;
        private UserManager<IdentityUser> UserManager;
        private IHrUnitOfWork _hrUnitOfWork;


        public RolesController(IHrUnitOfWork hrUnitOfWork) : base(hrUnitOfWork)
        {
            db = new UserContext();
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext("HrContext")));
            UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext("HrContext")));
            _hrUnitOfWork = hrUnitOfWork;
        }
        public ActionResult RolesIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult Index(bool isSSRole = false)
        {
            ViewBag.IsSSRole = isSSRole == false ? "SSRole" : "Roles";
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadRoles(string MenuName)
        {
            var companyId = User.Identity.GetDefaultCompany();
            var MenuId = db.Menus.Where(s => s.CompanyId == companyId && s.Name == MenuName).Select(c => c.Id).FirstOrDefault();
            string whereclause = GetWhereClause(MenuId);
            var excluded = new string[] { "Configuration", "Admin", "Developer" };
            var roles = db.ApplicationRoles.Where(a=>!excluded.Contains(a.Name)).Select(r => new RoleUserViewModel { Id = r.Id, Name = r.Name, SSRole = r.SSRole });
            var query = roles;
            if (whereclause.Length > 0)
            {
                try
                {
                    query = roles.Where(whereclause);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateRole(IEnumerable<RoleUserViewModel> models,string SSRole)
        {
            var result = new List<ApplicationRole>();
            var datasource = new DataSource<RoleUserViewModel>();
            datasource.Data = models;

            //  Iterate all updated rows which are posted by the PageDiv
            foreach (RoleUserViewModel m in models)
            {

                var role = new ApplicationRole(m.Name);

                // RoleManager.Create(role);
                role.SSRole = SSRole == "Roles" ? false : true;
                db.Roles.Add(role);
                result.Add(role);

            }

            db.SaveChanges();
            datasource.Data = (from m in models
                               join r in result on m.Name equals r.Name
                               select new RoleUserViewModel
                               {
                                   Id = r.Id,
                                   Name = m.Name,
                                   SSRole = SSRole == "Roles" ? false : true,
                               })
                               .ToList();

            return Json(datasource.Data);
        }
        public ActionResult CheckUsers(string Id)
        {
            string message = "OK";
            var Users = db.ApplicationRoles.Where(a => a.Id == Id).Select(a => new { UserList = a.Users.ToList() }).FirstOrDefault();
            if (Users.UserList.Count > 0)
            {
                message = "Error";
            }
            else
                message = "OK";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRole(IEnumerable<RoleUserViewModel> models)
        {

            var datasource = new DataSource<RoleUserViewModel>();
            datasource.Data = models;
            if (ModelState.IsValid)
            {
                foreach (RoleUserViewModel m in models)
                {
                    var Role = RoleManager.FindById(m.Id);

                    Role.Name = m.Name;
                    RoleManager.Update(Role);

                }
            }

            return Json(datasource.Data);
        }
        public ActionResult DeleteRole(string Id)
        {
            var datasource = new DataSource<RoleUserViewModel>();
            if (ModelState.IsValid)
            {
                var RoleMenuList = _hrUnitOfWork.MenuRepository.GetRoleMenue(Id); // RoleID
                var Users = db.Roles.Where(r => r.Id == Id).Select(a => new { userLst= a.Users.ToList() }).FirstOrDefault();
                if (Users.userLst.Count > 0)
                {
                    ModelState.AddModelError("Name", MsgUtils.Instance.Trls("CantDeletRole"));
                    datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                    return Json(datasource);
                }

                _hrUnitOfWork.MenuRepository.RemoveRange(RoleMenuList);
                //for (int i = 1; i < RoleMenuList.Count; i++)
                //{
                //    var rolemenu = RoleMenuList.Where(a => a.RoleId == Id).FirstOrDefault();
                //    _hrUnitOfWork.MenuRepository.RemoveRange(RoleMenu.ElementAtOrDefault(i).Functions);
                //    _hrUnitOfWork.MenuRepository.Remove(rolemenu);

                //}
                datasource.Errors = SaveChanges(User.Identity.GetLanguage());
                var role = RoleManager.Roles.Where(r => r.Id == Id).Single();
                RoleManager.Delete(role);
            }
            datasource.Total = 1;
            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        public ActionResult ReadRoleMenu(string RoleId, bool SSRole)
        {
            return Json(_hrUnitOfWork.MenuRepository.ReadRoleMenu(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage(), RoleId, SSRole), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRoleMenu(IEnumerable<RoleMenuViewModel> models)
        {
            var datasource = new DataSource<RoleMenuViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                _hrUnitOfWork.MenuRepository.UpdateRoleMenu(models, User.Identity.Name);
                datasource.Errors = SaveChanges(User.Identity.GetLanguage());
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public JsonResult GetRoleMenuFunction(int Id)
        {
            return Json(_hrUnitOfWork.MenuRepository.GetFunctions(Id), JsonRequestBehavior.AllowGet);
        }
        //CopyRole
        public JsonResult CopyRole(string name, string id)
        {
            string message = "Ok";
            var Roles = db.Roles.ToList();
            ApplicationRole role = (ApplicationRole)Roles.Where(a => a.Name == name).FirstOrDefault();
            string checkName = name;
            for (int i = 1; i <= Roles.Count; i++)
            {                
                checkName = string.Format("copy{0}_{1}", i, name);
                if (Roles.Where(a => a.Name == checkName).FirstOrDefault() == null)
                {
                    name = checkName;
                    break;
                }
            }
            //Add new record role with copy name         
            var RoleCopy = new ApplicationRole(checkName);
            RoleCopy.SSRole = role.SSRole;
            db.Roles.Add(RoleCopy);

            //Add copy Role Menu
            _hrUnitOfWork.MenuRepository.CopyRoleMenu(id, User.Identity.Name, RoleCopy.Id);

            db.SaveChanges();
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message,JsonRequestBehavior.AllowGet);
        }

    }
}