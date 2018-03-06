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
            var result = new LayOutViewModel();
            try
            {
                result.Companies = (User.Identity.Name.ToLower() == "hradmin" ? db.Database.SqlQuery<LayOutCompanyViewModel>("SELECT c.Id, ISNULL(dbo.fn_TrlsName(c.Name,'" + Language + "') , c.Name) Name from Companies c").ToList() : db.Database.SqlQuery<LayOutCompanyViewModel>("SELECT c.Id  ,ISNULL(dbo.fn_TrlsName(c.Name,'" + Language + "'),c.Name) Name from Companies c inner join UserCompanyRoles ac on c.Id = ac.CompanyId where ac.UserId='" + userId + "'").ToList());
                //result.Menus = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT Q1.Id, Q1.Name,Q1.ParentId, Q1.NodeType, Q1.Url, Q1.Sort, Q1.Icon, Q1.RoleId, Q1.Title, Q1.DataLevel,Q1.Version, Q1.SSMenu FROM (SELECT Menus.Id,Menus.Version, Menus.Name,Menus.ParentId ,Menus.NodeType, Menus.Url, Menus.Sort, Menus.Icon, Menus.SSMenu, RoleMenus.RoleId, ISNULL(NamesTbl.Title, Menus.Name) Title, ISNULL(RoleMenus.DataLevel,0) DataLevel FROM Menus INNER JOIN RoleMenus ON Menus.Id = RoleMenus.MenuId INNER JOIN AspNetUserRoles ON RoleMenus.RoleId = AspNetUserRoles.RoleId LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = Menus.Name + CAST(Menus.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "') WHERE (AspNetUserRoles.UserId = N'" + userId + "') AND (Menus.CompanyId = " + CompanyId + ") AND (Menus.IsVisible = 1) AND (Menus.NodeType < 2 )) AS Q1, (SELECT RoleMenus.MenuId, RoleMenus.RoleId, RoleMenus.DataLevel, ROW_NUMBER() OVER (PARTITION BY RoleMenus.MenuId ORDER BY RoleMenus.DataLevel DESC) RNK FROM RoleMenus inner join AspNetUserRoles ON AspNetUserRoles.RoleId = RoleMenus.RoleId and AspNetUserRoles.UserId = N'" + userId + "') AS Q2 WHERE Q1.RoleId = Q2.RoleId And Q1.Id = Q2.MenuId And Q2.RNK = 1 ORDER BY Q1.Sort").ToList();
                result.Menus = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT M.Id, M.Name,M.ParentId, M.NodeType, M.Url, M.Sort, M.Icon, UR.RoleId  RoleId, ISNULL(NamesTbl.Title, M.Name) Title, ISNULL(RM.DataLevel,0) DataLevel, M.[Version], M.SSMenu FROM Menus M LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = M.Name + CAST(M.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "'), RoleMenus RM, UserCompanyRoles UR  WHERE UR.UserId = N'" + userId + "' AND UR.CompanyId = " + CompanyId + " AND UR.RoleId = RM.RoleId AND  M.Id = RM.MenuId AND M.CompanyId = " + CompanyId + " AND M.IsVisible = 1 AND M.NodeType < 2 ORDER BY M.Sort").ToList();
                result.Tabs = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT M.Id, M.Name,M.ParentId, M.NodeType, M.Url, M.Sort, M.Icon, UR.RoleId  RoleId, ISNULL(NamesTbl.Title, M.Name) Title, ISNULL(RM.DataLevel,0) DataLevel, M.[Version], M.SSMenu FROM Menus M LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = M.Name + CAST(M.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "'), RoleMenus RM, UserCompanyRoles UR  WHERE UR.UserId = N'" + userId + "' AND UR.CompanyId = " + CompanyId + " AND UR.RoleId = RM.RoleId AND  M.Id = RM.MenuId AND M.CompanyId = " + CompanyId + " AND M.IsVisible = 1 AND M.NodeType = 2 ORDER BY M.Sort").ToList();
                result.Functions = db.Database.SqlQuery<LayOutFunctionViewModel>("SELECT Rm.RoleMenu_MenuId MenuId, F.Name FROM Functions F, RoleMenuFunctions RM, Menus M, UserCompanyRoles UC WHERE UC.UserId = N'" + userId + "' And UC.CompanyId = " + CompanyId + " And UC.RoleId = RM.RoleMenu_RoleId And M.CompanyId = " + CompanyId + " And M.Id = RM.RoleMenu_MenuId And F.Id = RM.Function_Id").ToList();
            }
            catch
            {
                Redirect("~/Account/Login");
            }

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
            var empid = User.Identity.GetEmpId();
            var companyid = User.Identity.GetDefaultCompany();
            var oldAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == empid && a.CompanyId == companyid && (a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date)).FirstOrDefault();
            if (oldAssignment != null)
            {
                Session["DepartmentId"] = oldAssignment.DepartmentId;
                Session["BranchId"] = oldAssignment.BranchId;
                Session["PositionId"] = oldAssignment.PositionId;
                Session["IsDepManager"] = oldAssignment.IsDepManager.ToString();
                Session["BranchId"] = oldAssignment.BranchId;
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

        public ActionResult _UploadFile(string source, int sourceid)
        {
            //ViewBag.Source = source;
            ViewBag.Document = _hrUnitOfWork.PeopleRepository.GetDocument(source, sourceid);
            return PartialView();
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
            var db = new UserContext();
            int EmpId = User.Identity.GetEmpId();
            int CompanyId = User.Identity.GetDefaultCompany();

            //var tasks = _hrUnitOfWork.CheckListRepository.ReadEmployeeTasks(CompanyId, EmpId, Language);
            //var msgs = _hrUnitOfWork.MessageRepository.ReadEmpMessages(CompanyId, EmpId, Language);
            var Emp = _hrUnitOfWork.PeopleRepository.GetAllPeoples(Language).Where(a => a.Id == EmpId).Select(a => new { a.Gender, a.PicUrl }).FirstOrDefault();
            //var Notify = _hrUnitOfWork.NotificationRepository.GetAllNotifications(User.Identity.Name, Language, CompanyId).Take(5).ToList();
            var all = db.Database.SqlQuery<NavBarItemVM>($"select top 5 '3' [Source], l.[Id], l.[Subject] as [From], l.[Message], ISNULL('data:image/jpg;base64,' + dbo.fn_GetDoc('EmployeePic', N.EmpId),  '/SpecialData/Photos/' + (CASE WHEN p.Gender = 2 THEN 'F' ELSE '' END)  + 'noimage.jpg') PicUrl, l.MarkAsRead [Read], l.SentTime SentDate from WebMobLogs l, Notifications N, People P where l.NotificatId = N.Id and N.EmpId = P.Id and l.CompanyId = {CompanyId} and l.SentToUser = '{User.Identity.Name}' union all select top 5 '2', M.[Id], dbo.fn_TrlsName(P.Title + ' ' + P.FirstName + ' ' + P.Familyname, '{Language}') as [From], M.Title [Message], ISNULL('data:image/jpg;base64,' + dbo.fn_GetDoc('EmployeePic', P.Id),  '/SpecialData/Photos/' + (CASE WHEN p.Gender = 2 THEN 'F' ELSE '' END)  + 'noimage.jpg') PicUrl, ME.[Read] [Read], M.CreatedTime SentDate from MsgEmployees ME, [Messages] M, People P where ME.MessageId = M.Id and ME.FromEmpId = P.Id and M.CompanyId = {CompanyId} and ME.ToEmpId = {EmpId} union all select top 5 '1' [Source], ET.[Id], dbo.fn_GetLookUpCode('EmpTaskCat', ET.TaskCat, '{Language}') as [From], ET.[Description] [Message], ISNULL('data:image/jpg;base64,' + dbo.fn_GetDoc('EmployeePic', ET.ManagerId),  '/SpecialData/Photos/' + (CASE WHEN Manager.Gender = 2 THEN 'F' ELSE '' END)  + 'noimage.jpg') PicUrl, CAST(0 AS bit) [Read], ET.StartTime SentDate from EmpTasks ET left outer join EmpChkLists CL on ET.EmpListId = CL.Id left outer join People Manager on ET.ManagerId = Manager.Id where ET.CompanyId = 0 and ET.EmpId = {EmpId} order by 1,7 desc").ToList();

            string empImage = (Emp == null ? "/SpecialData/Photos/noimage.jpg" : (Emp.PicUrl == null ? "/SpecialData/Photos/" + (Emp.Gender == 2 ? "Fnoimage.jpg" : "noimage.jpg") : "data:image/jpg;base64," + Emp.PicUrl));
            return Json(new { tasks = all.Where(a => a.Source == "1"), msgs = all.Where(a => a.Source == "2"), Notify = all.Where(a => a.Source == "3"), empImage }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MegaMenuContent()
        {
            var userId = User.Identity.GetUserId();
            int CompanyId = User.Identity.GetDefaultCompany();
            var db = new UserContext();

            var Menus = db.Database.SqlQuery<LayOutMenuViewModel>("SELECT M.Id, M.Name,M.ParentId, M.NodeType, M.Url, M.Sort, M.Icon, UR.RoleId  RoleId, ISNULL(NamesTbl.Title, M.Name) Title, ISNULL(RM.DataLevel,0) DataLevel, M.[Version], M.SSMenu FROM Menus M LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = M.Name + CAST(M.Sequence AS VARCHAR(5)) AND NamesTbl.Culture = '" + Language + "'), RoleMenus RM, UserCompanyRoles UR  WHERE UR.UserId = N'" + userId + "' AND UR.CompanyId = " + CompanyId + " AND UR.RoleId = RM.RoleId AND  M.Id = RM.MenuId AND M.CompanyId = " + CompanyId + " AND M.IsVisible = 1 AND M.NodeType < 2 ORDER BY M.Sort").ToList();

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

        public ActionResult GetEmpSechedual(string Ids, DateTime? date ,string StartTime ,string EndTime, int id)
        {
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;
            var model = _hrUnitOfWork.MeetingRepository.GetEmployeeSchedual(Ids, date.Value, date.Value, "Meeting", id, Language).ToList();
            return PartialView("_SchedualBar", model);
          
        }
    }
}