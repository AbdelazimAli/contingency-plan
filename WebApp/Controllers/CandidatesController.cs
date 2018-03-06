using Interface.Core;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class CandidatesController : BaseController
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
        public CandidatesController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: Candidates
        public ActionResult Index()
        {
            ViewBag.pageDiv = _hrUnitOfWork.PageEditorRepository.GetTablesHasCust(CompanyId, Language);
            return View();
        }

        public ActionResult GetCandidates(string tableName, int sourceId)
        {
            return Json(_hrUnitOfWork.EmployeeRepository.ReadCandidates(tableName, sourceId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpIdentical(int Id, string tableName, int sourceId)
        {
            return Json(_hrUnitOfWork.EmployeeRepository.ReadEmpIdentical(Id, tableName, sourceId, Language), JsonRequestBehavior.AllowGet);
        }


    }
}