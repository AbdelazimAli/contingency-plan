using Interface.Core;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class LogController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string Language { get; set; }
        private int CompanyId { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
            }
        }
        public LogController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult Log(string Id, string ObjectName)
        {
            ViewBag.id = Id;
            ViewBag.objectName = ObjectName;
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            string Menu = Request.QueryString["MenuId"]?.ToString();
            int MenuId = string.IsNullOrEmpty(Menu) ? 0 : int.Parse(Request.QueryString["MenuId"].ToString());
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            return View();
        }
        // GET: Log
        public ActionResult GetLog(string objectname, string Id, int pageSize, int skip) // comma seperated object names ex: People,Assignment,... or Single Object ex: Jobs
        {
            byte version;
            byte.TryParse(Request.QueryString["version"], out version);
            string filter = "";
            string Sorting = "";
            IQueryable<AuditViewModel> query;
            if (objectname == "")
            {
                int EmpId = Convert.ToInt32(Id);
                query = _hrUnitOfWork.PeopleRepository.EmployeesLog(CompanyId, version, EmpId, Language);
            }
            else
            {
                string[] objects = objectname.Split(',');
                query = _hrUnitOfWork.JobRepository.GetLog(CompanyId, objects, version, Language, Id);
            }
            query = (IQueryable<AuditViewModel>)Models.Utils.GetFilter(query, ref filter, ref Sorting);
            if (filter.Length > 0)
            {
                try
                {
                    if (filter.Length > 0)
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
                query = query.OrderBy(a => a.ObjectName).Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkFlow(string Source, int SourceId, int DocumentId)
        {
            return View(new WorkFlowGridViewModel { Source = Source, SourceId = SourceId, DocumentId = DocumentId });
        }

        public ActionResult ReadWorkFlow(string Source, int SourceId, int DocumentId)
        {
            if (DocumentId == 0)
                return null;
            return Json(_hrUnitOfWork.LeaveRepository.ReadWorkFlow(Source, SourceId, DocumentId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

    }
}