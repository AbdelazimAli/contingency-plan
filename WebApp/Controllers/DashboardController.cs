using System.Web.Mvc;
using Interface.Core;
using WebApp.Extensions;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class DashboardController : BaseController
    {
        private IHrUnitOfWork _hrunitofwork;
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
        public DashboardController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrunitofwork = unitOfWork;
        }

        #region HR Dashboard
        // GET: Dashboard
        public ActionResult Index()
        {
            ViewBag.TurnOver = _hrunitofwork.EmployeeRepository.GetActiveByMonth(CompanyId);

            return View();
        }

        public ActionResult InitialData(byte range, int[] depts)
        {
            var LeaveStatus = _hrunitofwork.LeaveRepository.LeaveStatistics(range, CompanyId, Language);
            var EmpStatus = _hrunitofwork.EmployeeRepository.EmployeesStatus(depts, CompanyId, Language);
            var byBranches = _hrunitofwork.EmployeeRepository.CountEmpsByBranches(CompanyId, Language);

            return Json(new { LeaveStatus, EmpStatus, byBranches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LeaveDashboardData(byte range)
        {
            var LeaveStatus = _hrunitofwork.LeaveRepository.LeaveStatistics(range, CompanyId, Language);

            return Json(new { LeaveStatus = LeaveStatus }, JsonRequestBehavior.AllowGet);
        }

        #region load Actions
        public ActionResult EmpAgesChart()
        {
            return Json(_hrunitofwork.EmployeeRepository.EmployeesAges(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenderChart()
        {
            return Json(_hrunitofwork.EmployeeRepository.CountGenderByEmployment(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpsByBranchesChart()
        {
            return Json(_hrunitofwork.EmployeeRepository.CountEmpsByBranches(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NationalityChart()
        {
            return Json(_hrunitofwork.EmployeeRepository.CountNationalityByEmployment(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }


        public ActionResult AgesByDeptsChart(int[] depts)
        {
            return Json(_hrunitofwork.EmployeeRepository.AgesByDepts(depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpStatusChart(int[] depts)
        {
            return Json(_hrunitofwork.EmployeeRepository.EmployeesStatus(depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpsByDeptsChart(int[] depts)
        {
            return Json(_hrunitofwork.EmployeeRepository.CountEmpsByDepts(depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenderByDeptsChart(int[] depts)
        {
            return Json(_hrunitofwork.EmployeeRepository.GenderByDepts(depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LocsByDeptsChart(int[] depts)
        {
            return Json(_hrunitofwork.EmployeeRepository.BranchesByDepts(depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Grids

        public ActionResult GetLeaveStatusDetailsGrid(byte range, byte approvalStatus)
        {
            return Json(_hrunitofwork.LeaveRepository.LeaveStatisticsGrid(range, approvalStatus, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleNationality(int NationalityId)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleWithNational(NationalityId, Language, CompanyId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleBranch(int LocId)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleInBranch(LocId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleGenderAndPersonType(int gender, int PersonType)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleGenderPersonType(gender, PersonType, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleAge(string ageRange)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleAge(ageRange, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleDept(int DeptId, bool isDefault)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleDepts(isDefault, DeptId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleGenderDept(int DeptId, int GenderId, bool isDefault)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleGenderDepts(isDefault, DeptId, GenderId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleLocDept(int DeptId, int LocId, bool isDefault)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleLocDepts(isDefault, DeptId, LocId, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeopleAgeDepts(int DeptId, string AgeRange, bool isDefault)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetPeopleAgeDepts(isDefault, DeptId, AgeRange, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpStutes(int[] Depts, int Stutes)
        {
            return Json(_hrunitofwork.EmployeeRepository.GetEmpStutes(Depts, Stutes, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        public ActionResult EmpIndex()
        {
            ViewBag.YearTasks = _hrunitofwork.CheckListRepository.EmpTasksByPerid(User.Identity.GetEmpId(), CompanyId, Language);
            ViewBag.MonthesTasks = _hrunitofwork.CheckListRepository.EmpTasksBySubPeriod(User.Identity.GetEmpId(), CompanyId, Language);

            ViewBag.AnnualLeave = _hrunitofwork.LeaveRepository.LeavesCounts(User.Identity.GetEmpId(), CompanyId, Language);

            return View();
        }
        public ActionResult EmpPeriodLeavesGrid(int LeaveType, int Period)
        {
            return Json(_hrunitofwork.LeaveRepository.PeriodLeaveGrid(User.Identity.GetEmpId(), LeaveType,Period, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ManagerIndex()
        {
            ViewBag.ManagerEmployeeTask = _hrunitofwork.CheckListRepository.ManagerEmployeeTask(User.Identity.GetEmpId(), CompanyId, Language);
            return View();
        }

    }
}