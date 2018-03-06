using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Interface.Core.Repositories
{
    public interface ILeaveRepository: IRepository<LeaveRequest>
    {
        AssignOrder GetAssignOrderByiD(int? ID);
        Dictionary<string, string> ReadMailEmpAssign(string Language, int Id);
        IQueryable<WorkFlowGridViewModel> ReadWorkFlow(string source, int sourceId, int requestId, int companyId, string culture);
        IQueryable<LeaveMoneyAdjustViewModel> GetLeaveCreaditBalance(int companyId, string culture);
        IQueryable<LeaveMoneyAdjustViewModel> GetLeaveMoneyTrans(int companyId, string culture);
        IQueryable<LeaveMoneyAdjustViewModel> GetLeaveRest(int companyId, string culture);
        void RangeFilter(byte range, out DateTime startRange, out DateTime endRange);
        void Add(LeaveTrans trans);
        void AddAssignOrder(AssignOrder order);
        void Add(LeavePosting trans);
        void Attach(LeavePosting leavePosting);
        DbEntityEntry<AssignOrder> EntryAssignOrder(AssignOrder AssignOrder);
        DbEntityEntry<LeavePosting> Entry(LeavePosting leavePosting);
        void AttachAssignOrder(AssignOrder assignOrder);
        string AddAssignOrdersLeaveTrans(AssignOrder assignOrder, string User, string culture);
        void Remove(LeaveTrans LeaveTrans);
        void Add(WfTrans WfTrans);
        void Add(WfRole Wfrole);
        void Attach(WfRole Wfrole);
        DbEntityEntry<WfRole> Entry(WfRole Wfrole);
        void Add(Workflow workflow);
        void Attach(Workflow workflow);
        DbEntityEntry<Workflow> Entry(Workflow workflow);    
        void Remove(WfRole Wfrole);
        IQueryable<WfRoleViewModel> GetWfRole(int RId);
        Dictionary<string, string> ReadMailEmpLeave(string Language, int Id);
        LeaveAdjust GetLeaveAction(int? id);
        IQueryable GetAcuralGridLeaveTypes(int companyId, string culture);
        IQueryable<FormList> GetDeptEmployees(int companyId, string culture, string Departments);
        RequestWfFormViewModel ReadRequestWF(int leaveId, string Source, string lang);
        IQueryable<LeaveActionViewModel> GetLeaveAction(string culture, int CompanyId);
        void Add(LeaveAdjust leaveAction);
        void Attach(LeaveAdjust leaveAction);
        DbEntityEntry<LeaveAdjust> Entry(LeaveAdjust leaveAction);
        void Remove(LeaveAdjust leaveAction);
        LeaveActionFormViewModel ReadleaveAction(int Id);
        IQueryable GetAccrualLeaveTypes(int companyId, string culture);
        IQueryable<PeriodListViewModel> GetOpenedLeavePeriods();
        IQueryable<LeaveTransSummary> GetLeaveTransSummary(int YearId, int companyId, string culture);
        IQueryable<LeaveTransViewModel> GetLeaveTrans(int TypeId, int PeriodId, int EmpId, string culture);
        IQueryable<AssignOrderViewModel> ReadAssignOrders(int companyId, byte Tab, byte Range, string Depts, DateTime? Start, DateTime? End, string culture);
        IQueryable<AssignOrderViewModel> ReadAssignOrders(int companyId, string culture);
        IQueryable<LeaveRequestViewModel> ReadLeaveRequests(int companyId, string culture);
        IQueryable<LeaveRequestViewModel> ReadLeaveRequestArchive(int companyId, byte Range, string Depts, DateTime? Start, DateTime? End, string culture);
        IQueryable<AssignOrderViewModel> ReadAssignOrdersArchieve(int companyId, byte Range, string Depts, DateTime? Start, DateTime? End, string culture);
        IQueryable<DateTime> GetEmpAssignDates(int companyId, int EmpId);
        PeopleGridViewModel GetFullEmpInfo(int companyId, int EmpId, string culture);
        int GetLastEmpCalcsMethod(int companyId, int EmpId);
        //IQueryable<AssignOrderViewModel> GetEmpAssignData(int companyId, int EmpId);
        IQueryable<AssignOrderViewModel> GetEmpAssignData(int companyId, int EmpId, string culture);
        IQueryable<LeaveRequestViewModel> ReadLeaveRequestTabs(int companyId, byte Tab, byte Range, string Depts, DateTime? Start, DateTime? End, string culture);
        string CheckAssignStatus(int empId, int typeId, out LeaveType leaveType, string culture);
        IList<DropDownList> GetEmpLeaveTypes(int empId, int compnayId, string culture);
        IQueryable<LeaveTransViewModel> ReadEmpLeaveTrans(int empId, DateTime startDate, int companyId, string culture);
        IEnumerable<LeaveTransCounts> AnnualLeavesProgress(int empId, DateTime startDate, string culture);
        int GetLeaveRequestPeriod(int calendar, DateTime startDate, string culture, out string message);
        int GetLeaveRequestPeriod(int calendar, DateTime startDate, string culture);

        RequestValidationViewModel CheckLeaveRequest(int TypeId, int EmpId, DateTime StartDate, DateTime EndDate, float NofDays, string culture, int RequestId, bool isSSUser, int companyId, int? replaceEmp = null);
        LeavePlanStarsVM GetStars(GetStarsParamVM param);
        List<string> CheckLeavePlan(GetStarsParamVM param, string culture, out int Stars, out int EmpStars);
        int GetLeaveBalance(ref RequestValidationViewModel requestVal, ReqDaysParamVM param);
        IQueryable GetLeaveTypesList(int companyId, string culture);
        //IEnumerable GetSpacificLeaveTypes(int companyId, string culture, int empId);
        IList<DropDownList> GetSpacificLeaveTypes(int companyId, string culture, int empId);
        CalenderViewModel GetHolidays(int compId);
        LeaveReqViewModel GetRequest(int requestId, string culture);
        //Leave Emp tasks
        IQueryable<EmpTasksViewModel> ReadLeaveEmpTasks(int EmpId, int subPeriodId, string culture);
        //replacement emp
        List<string> IsReplacement(int EmpId, DateTime StartDate, DateTime EndDate, int CompanyId, string culture);
        List<RequestValidationViewModel> HavePervRequests(List<int> EmpIds, int Id, DateTime StartDate, DateTime EndDate, int companyId, bool isReplace = true);
        IQueryable<FormList> GetReplaceEmpList(int empId, string culture);

        //upcpming
        IEnumerable<HolidayViewModel> GetUpcomingHolidays(int companyId);
        IEnumerable GetUpcomingLeaves(int companyId, string culture);
        //Holiday
        LeaveTypeFormViewModel ReadleaveType(int Id,string culture);
        IQueryable<HolidayViewModel> ReadStanderedHolidays(int companyId);
        IQueryable<HolidayViewModel> ReadCustomHolidays(int companyId);
        void Add(Holiday holiday);
        void Attach(Holiday holiday);
        DbEntityEntry<Holiday> Entry(Holiday holiday);
        void Remove(Holiday holiday);
        IQueryable<LeaveTypeViewModel> GetLeaveTypes(int companyId, string culture);
        void Remove(LeaveType leaveType);
        // LeaveTypeViewModel GetLeaveType(int id);
        LeaveType GetLeaveType(int? id);
        IQueryable<ExcelGridLeaveRangesViewModel> GetLeaveRange(int leaveTypeId);

        //Group Leave
        List<ErrorMessage> CheckGroupLeave(LeaveReqViewModel request, LeaveType type, string culture, out int PeriodId);
        IQueryable<GroupLeaveViewModel> CheckGroupLeaveGrid(LeaveReqViewModel request,int[] Depts, int companyId, string culture, out List<ErrorMessage> errors);
        void Add(GroupLeave GroupLeave);
        void Add(GroupLeaveLog GroupLeaveLog);


        DbEntityEntry<LeaveType> Entry(LeaveType LeaveType);
        void Attach(LeaveType LeaveType);
        void Add(LeaveType LeaveType);
        void Add(LeaveRange LeaveRange);
        DbEntityEntry<LeaveRange> Entry(LeaveRange LeaveRange);
        void Attach(LeaveRange LeaveRange);
        void Remove(LeaveRange leaveRange);
        Period GetLperiod(int? id);
        void Add(RequestWf requestWf);
        void Attach(RequestWf requestWf);
        void Remove(RequestWf requestWf);
        bool DeleteRequest(LeaveRequest request, string culture);
        bool DeleteAssignOrder(int id, string culture);
        DbEntityEntry<RequestWf> Entry(RequestWf requestWf);
        //followup
        IQueryable<LeaveReqGridViewModel> GetLeaveReqFollowUp( int companyId, string culture);
        IQueryable<LeaveReqGridViewModel> GetApprovedLeaveReq(int companyId, string culture);
        IList<FormList> GetLeavePeriods(int LeaveId);
        //string UnPostLeaveAction(LeaveAdjust action, string user, string culture);
        //void PostLeaveAction(LeaveAdjust action, string user);
        void AddAcceptLeaveTrans(LeaveRequest request, string user);
        void AddBreakLeaveTrans(LeaveRequest request, float diffDays, string user);
        string AddCancelLeaveTrans(LeaveRequest request, string user, string culture);
        void AddEditLeaveTrans(LeaveRequest request, DateTime startDate);
        void CancelLeaveRequests(int empId, string userName, int companyId, byte version, string culture);
        string CalcLeaveBalance(out IEnumerable<LeaveBalanceGridViewModel> Grids, int TypeId, int PeriodId, int SubPeriodId, int CompanyId, string culture);
        //leave operation
        LeaveOpViewModel GetLeaveOpReq(int id, string culture);
        //Dashboard
        IEnumerable<ChartViewModel> LeaveStatistics(byte range, int companyId, string culture);
        IEnumerable<LeaveReqViewModel> LeaveStatisticsGrid(byte range, byte status, int companyId, string culture);
        IEnumerable<LeaveTransCounts> LeavesCounts(int empId, int companyId, string culture);
        IQueryable<LeaveReqViewModel> PeriodLeaveGrid(int empId, int leaveTypeId, int periodId, int companyId, string culture);
        IQueryable<LeaveTransOpenBalanceViewModel> GetLeaveFirstTrans(int companyId, string culture, int LeaveType, DateTime FiscalYear, string Departments);
        IEnumerable GetAcuralRestLeaveTypes(int companyId, string culture);
        //API
        ValidationMessages CheckLeaveRequestApi(int TypeId, int EmpId, DateTime StartDate, DateTime EndDate, float NofDays, string culture, int RequestId, bool isSSUser, int companyId, int? replaceEmp = null);
        IEnumerable<WorkFlowObjectsViewModel> ReadWorkFlowObjects(int companyId, string culture);
        string CheckPeriods(DateTime Period, int LeaveId, string Culture);

    }
}
