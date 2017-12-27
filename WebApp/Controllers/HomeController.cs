using Interface.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {
        IHrUnitOfWork _hrUnitOfWork;
        private string Language { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
                Language = requestContext.HttpContext.User.Identity.GetLanguage();

        }

        public HomeController(IHrUnitOfWork unitOfWork)
          : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        [HttpPost]
        public ActionResult Changecompany(string Id)
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst("DefaultCompany"));
            Identity.AddClaim(new Claim("DefaultCompany", Id));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties { IsPersistent = false });
            _hrUnitOfWork.EmployeeRepository.GetPersonSetup(int.Parse(Id));
            return Json("Success");
        }
        public ActionResult Getchanges(ChangesObject obj)
        {
            List<ChangesViewmodel> chan = new List<ChangesViewmodel>();

           
                var ls = _hrUnitOfWork.Repository<AudiTrail>().Where(a => (a.ColumnName == obj.columnname) && (a.CompanyId == obj.companyId) && (a.ObjectName == obj.objectname) && (a.SourceId == obj.sourceId) && (a.Version == obj.version)).OrderBy(b => b.ModifiedTime).Take(5).ToList();

                foreach (var item in ls)
                {
                    chan.Add(new ChangesViewmodel { Value = item.ValueAfter, ModifiedTime = item.ModifiedTime, ModifiedUser = item.ModifiedUser });

                }
                if (ls.Count > 0)
                    chan.Add(new ChangesViewmodel { Value = ls.FirstOrDefault().ValueBefore, ModifiedUser = obj.CreatedUser, ModifiedTime = obj.CreatedTime });
            return PartialView("changes", chan);
        }
        private LayOutViewModel LayOut()
        {
            var userId = User.Identity.GetUserId();
            int CompanyId = User.Identity.GetDefaultCompany();
            var db = new UserContext();
    
            var result = new LayOutViewModel
            {
                Companies = (User.Identity.Name.ToLower() == "hradmin" ? db.Database.SqlQuery<LayOutCompanyViewModel>("SELECT c.Id, ISNULL(dbo.fn_TrlsName(c.Name,'" + Language + "') , c.Name) Name from Companies c").ToList() : db.Database.SqlQuery<LayOutCompanyViewModel>("SELECT c.Id  ,ISNULL(dbo.fn_TrlsName(c.Name,'"+Language+"'),c.Name) Name from Companies c inner join ApplicationUserCompanies ac on c.Id = ac.Company_Id where ac.ApplicationUser_Id='"+userId+"'").ToList()),
                Menus = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT Q1.Id, Q1.Name,Q1.ParentId, Q1.NodeType, Q1.Url, Q1.Sort, Q1.Icon, Q1.RoleId, Q1.Title, Q1.DataLevel,Q1.Version, Q1.SSMenu FROM (SELECT Menus.Id,Menus.Version, Menus.Name,Menus.ParentId ,Menus.NodeType, Menus.Url, Menus.Sort, Menus.Icon, Menus.SSMenu, RoleMenus.RoleId, ISNULL(NamesTbl.Title, Menus.Name) Title, ISNULL(RoleMenus.DataLevel,0) DataLevel FROM Menus INNER JOIN RoleMenus ON Menus.Id = RoleMenus.MenuId INNER JOIN AspNetUserRoles ON RoleMenus.RoleId = AspNetUserRoles.RoleId LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = Menus.Name + CAST(Menus.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "') WHERE (AspNetUserRoles.UserId = N'" + userId + "') AND (Menus.CompanyId = " + CompanyId + ") AND (Menus.IsVisible = 1) AND (Menus.NodeType < 2 )) AS Q1, (SELECT RoleMenus.MenuId, RoleMenus.RoleId, RoleMenus.DataLevel, ROW_NUMBER() OVER (PARTITION BY RoleMenus.MenuId ORDER BY RoleMenus.DataLevel DESC) RNK FROM RoleMenus inner join AspNetUserRoles ON AspNetUserRoles.RoleId = RoleMenus.RoleId and AspNetUserRoles.UserId = N'" + userId + "') AS Q2 WHERE Q1.RoleId = Q2.RoleId And Q1.Id = Q2.MenuId And Q2.RNK = 1 ORDER BY Q1.Sort").ToList(),
                Tabs = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT Menus.Id, Menus.Name,Menus.Version, Menus.Url, Menus.Icon, RoleMenus.RoleId, ISNULL(NamesTbl.Title, Menus.Name) Title, ISNULL(RoleMenus.DataLevel,0) DataLevel, Menus.ParentId FROM Menus INNER JOIN RoleMenus ON Menus.Id = RoleMenus.MenuId INNER JOIN AspNetUserRoles ON RoleMenus.RoleId = AspNetUserRoles.RoleId LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = Menus.Name + CAST(Menus.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "') WHERE (AspNetUserRoles.UserId = N'" + userId + "') AND (Menus.CompanyId = " + CompanyId + ") AND (Menus.IsVisible = 1) AND (Menus.NodeType = 2) ORDER BY Menus.Sort").ToList()
            };

            return result;
        }
        public JsonResult GetLayOut()
        {
            return Json(LayOut(), JsonRequestBehavior.AllowGet);   
        }

        public ActionResult Refresh()
        {
            MsgUtils.Instance.Refresh();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var oldAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == User.Identity.GetEmpId() && a.CompanyId == User.Identity.GetDefaultCompany() && (a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date)).FirstOrDefault();
            if (oldAssignment != null)
            {
                Session["DepartmentId"] = oldAssignment.DepartmentId;
                Session["LocationId"] = oldAssignment.LocationId;
                Session["PositionId"] = oldAssignment.PositionId;
                Session["IsDepManager"] = oldAssignment.IsDepManager.ToString();
                Session["BranchId"] = oldAssignment.BranchId;
                Session["SectorId"] = oldAssignment.SectorId;
                Session["JobId"] = oldAssignment.JobId;
            }
            else 
                Session["JobId"] = "0";



            return View();
        }
       
        public ActionResult SessionOff()
        {
            return View();
        }


        public ActionResult test()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Here we change sample information";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ChangeName(string userName)
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            //var confirmResult = await userManager.ConfirmEmailAsync(userId, code);
            //Rename user name and change email then confirm email
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext("HrContext")));
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());

            // change username and email
            user.UserName = "Administrator";
            user.Email = "waleedhashem@gmail.com";

            // Persiste the changes
            await userManager.UpdateAsync(user);

            // generage email confirmation code
            //var emailConfirmationCode = await userManager.GenerateEmailConfirmationTokenAsync(user.Name);

            // generate url for page where you can confirm the email
            var callbackurl = "http://example.com/ConfirmEmail";

            // append userId and confirmation code as parameters to the url
            //Bcallbackurl += System.String.Format("?userId={0}&code={1}", user.Name, System.Web.HttpUtility.UrlEncode(emailConfirmationCode));

            var htmlContent = System.String.Format(
                    @"Thank you for updating your email. Please confirm the email by clicking this link: 
                    <br><a href='{0}'>Confirm new email</a>",
                    callbackurl);

            // send email to the user with the confirmation link
            await userManager.SendEmailAsync(user.Id, subject: "Email confirmation", body: htmlContent);

            return RedirectToAction("Index");
        }

        public ActionResult EmployeeNavBar()
        {
            int EmpId = User.Identity.GetEmpId();
            int CompanyId = User.Identity.GetDefaultCompany();
            var oldAssignment = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == EmpId && a.CompanyId == CompanyId && a.Status == 1).FirstOrDefault();
            if (oldAssignment == null)
                EmpId = 0;
            else
                EmpId = oldAssignment.EmpId;
            var tasks = _hrUnitOfWork.CheckListRepository.ReadEmployeeTasks(CompanyId, EmpId, Language);
            var msgs = _hrUnitOfWork.MessageRepository.ReadEmpMessages(CompanyId, EmpId, Language);
            var Emp = _hrUnitOfWork.PeopleRepository.ReadPerson(EmpId, Language);
            var Notify = _hrUnitOfWork.NotificationRepository.GetAllNotifications(User.Identity.Name, Language, CompanyId).Take(5).ToList();
            string empImage = (Emp != null && Emp.HasImage ? Emp.Id + ".jpeg" : (Emp != null && Emp.Gender == 2 ? "Fnoimage.jpg" : "noimage.jpg"));
            return Json(new { tasks, msgs, Notify, empImage }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MegaMenuContent()
        {

            var userId = User.Identity.GetUserId();
            int CompanyId = User.Identity.GetDefaultCompany();
            var db = new UserContext();

            var Menus = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT Q1.Id, Q1.Name,Q1.ParentId, Q1.NodeType, Q1.Url, Q1.Sort, Q1.Icon, Q1.RoleId, Q1.Title, Q1.DataLevel,Q1.Version, Q1.SSMenu FROM (SELECT Menus.Id,Menus.Version, Menus.Name,Menus.ParentId ,Menus.NodeType, Menus.Url, Menus.Sort, Menus.Icon, Menus.SSMenu, RoleMenus.RoleId, ISNULL(NamesTbl.Title, Menus.Name) Title, ISNULL(RoleMenus.DataLevel,0) DataLevel FROM Menus INNER JOIN RoleMenus ON Menus.Id = RoleMenus.MenuId INNER JOIN AspNetUserRoles ON RoleMenus.RoleId = AspNetUserRoles.RoleId LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = Menus.Name + CAST(Menus.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "') WHERE (AspNetUserRoles.UserId = N'" + userId + "') AND (Menus.CompanyId = " + CompanyId + ") AND (Menus.IsVisible = 1) AND (Menus.NodeType < 2 )) AS Q1, (SELECT RoleMenus.MenuId, RoleMenus.RoleId, RoleMenus.DataLevel, ROW_NUMBER() OVER (PARTITION BY RoleMenus.MenuId ORDER BY RoleMenus.DataLevel DESC) RNK FROM RoleMenus inner join AspNetUserRoles ON AspNetUserRoles.RoleId = RoleMenus.RoleId and AspNetUserRoles.UserId = N'" + userId + "') AS Q2 WHERE Q1.RoleId = Q2.RoleId And Q1.Id = Q2.MenuId And Q2.RNK = 1 ORDER BY Q1.Sort").ToList();

            var result = Menus.Where(m => m.ParentId == null && m.Url == null)
                .Select(m => new MegaMenu
                {
                    ModuleId = m.Id,
                    ModuleName = m.Name,
                    ModuleTitle = m.Title,
                    SubModules = Menus.Where(m1 => m1.ParentId == m.Id)
                    .Select(m2 => new SubModule {
                        ModuleId = m2.ParentId.Value,
                        SubId = m2.Id,
                        SubName = m2.Title,
                        MenuObj=m2,
                        MenuItems = Menus.Where(m3 => m3.ParentId == m2.Id).Select(m4 => m4).ToList()
                    } ).ToList()
                });






            return PartialView(result);
        }
    }
}