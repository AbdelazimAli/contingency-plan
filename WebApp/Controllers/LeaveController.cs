using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LeaveController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        public LeaveController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: Leave
        public ActionResult Index()
        {
            
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            bool isSSMenu = false;
            if (MenuId != 0)
            {
                isSSMenu = _hrUnitOfWork.MenuRepository.Get(MenuId)?.SSMenu ?? false;
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            }
            ViewBag.isSSMenu = isSSMenu;

            //for date range (sun - sat)
            string[] weekDays = { MsgUtils.Instance.Trls("Sunday"), MsgUtils.Instance.Trls("Monday"), MsgUtils.Instance.Trls("Tuesday"), MsgUtils.Instance.Trls("Wednesday"), MsgUtils.Instance.Trls("Thursday"), MsgUtils.Instance.Trls("Friday"), MsgUtils.Instance.Trls("Saturday") };
            byte? weekend = _PersonSetup.Weekend2 ?? _PersonSetup.Weekend1;
            if (weekend != null)
            {
                byte firstDate = (byte)(weekend == 6 ? 0 : (weekend + 1));
                ViewBag.week = "(" + weekDays[firstDate] + " - " + weekDays[weekend.Value] + ")";
            }

            return View();
        }

        public ActionResult GetLeaves(int MenuId, byte Tab, int pageSize, int skip, byte? Range, string Depts, DateTime? Start, DateTime? End)
        {
            IQueryable<LeaveRequestViewModel> query;

            if (Tab == 4)
                query = _hrUnitOfWork.LeaveRepository.ReadLeaveRequestArchive(CompanyId, Range ?? 10, Depts, Start, End, Language);
            else
                query = _hrUnitOfWork.LeaveRepository.ReadLeaveRequestTabs(CompanyId, Tab, Range ?? 10, Depts, Start, End, Language);

            
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);

            query = (IQueryable<LeaveRequestViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);

            if (whecls.Length > 0 || filter.Length > 0)
            {
                try
                {
                    if (whecls.Length > 0 && filter.Length == 0)
                        query = query.Where(whecls);
                    else if (filter.Length > 0 && whecls.Length == 0)
                        query = query.Where(filter);
                    else
                        query = query.Where(filter).Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            var total = query.Count();

            if (Sorting.Length > 0 && skip > 0)
                query = query.Skip(skip).Take(pageSize).OrderBy(Sorting);
            else if (Sorting.Length > 0)
                query = query.Take(pageSize).OrderBy(Sorting);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }

        //validation 
        public ActionResult CheckLeave(int TypeId, int EmpId, float NofDays, DateTime StartDate, DateTime EndDate, int ReqId, int? ReplaceEmp)
        {
            return Json(_hrUnitOfWork.LeaveRepository.CheckLeaveRequest(TypeId, EmpId, StartDate, EndDate, NofDays, Language, ReqId, User.Identity.IsSelfServiceUser(), CompanyId, ReplaceEmp), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangEmployee(int EmpId)
        {
            var types = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(EmpId, CompanyId, Language).Select(t => new { id = t.Id, name = t.Name });
            var replaceEmps = _hrUnitOfWork.LeaveRepository.GetReplaceEmpList(EmpId, Language);

            return Json(new { types = types, replaceEmps = replaceEmps }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgress(int EmpId, DateTime StartDate)
        {
            IEnumerable<LeaveTransCounts> progress = null;
            if (StartDate != null)
                progress = _hrUnitOfWork.LeaveRepository.AnnualLeavesProgress(EmpId, StartDate, Language);

            return Json(progress, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCalcOptions(int typeId, int? empId, DateTime requestDate, int requestId)
        {
            LeaveType leaveType = _hrUnitOfWork.LeaveRepository.GetLeaveType(typeId);
            RequestValidationViewModel requestVal = null;
            if (empId != null)
            {
                requestVal = new RequestValidationViewModel();
                _hrUnitOfWork.LeaveRepository.GetLeaveBalance(ref requestVal, new ReqDaysParamVM { type = leaveType, StartDate = requestDate, EmpId = empId.Value, RequestId = requestId, culture = Language });
            }
            return Json(new { type = leaveType, days = requestVal}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeReplaceEmp(int replaceEmp, DateTime StartDate, DateTime EndDate)
        {
            var replaceFor = _hrUnitOfWork.LeaveRepository.HavePervRequests(new List<int> { replaceEmp }, 0, StartDate, EndDate, CompanyId, true);
            string message = "";
            if (replaceFor.Count > 0)
                message = MsgUtils.Instance.Trls("ReplaceEmployeeInLeave");

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id = 0, byte Version = 0)
        {
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("LeaveRequest", CompanyId, Version);
            if (columns.Where(fc => fc == "EmpId").FirstOrDefault() == null)
                ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);

            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId); //for Calender

            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            bool isSSMenu = false;
            if (MenuId != 0)
            {
                isSSMenu = _hrUnitOfWork.MenuRepository.Get(MenuId)?.SSMenu ?? false;
                ViewBag.isSSMenu = isSSMenu;
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            }
            if (id == 0)
            {
                if (isSSMenu)
                    fillViewBags(User.Identity.GetEmpId(), DateTime.Today);
                else
                    ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetLeaveTypesList(CompanyId, Language);

                return View(new LeaveReqViewModel() { CompanyId = CompanyId });
            }
            LeaveReqViewModel request = _hrUnitOfWork.LeaveRepository.GetRequest(id, Language);

            request.NofDays = (int)request.NofDays;

            ViewBag.CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); //for Calender
            fillViewBags(request.EmpId, request.StartDate);

            return request == null ? (ActionResult)HttpNotFound() : View(request);
        }

        private void fillViewBags(int EmpId, DateTime StartDate)
        {
            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(EmpId, CompanyId, Language).Select(t => new { id = t.Id, name = t.Name });
            ViewBag.Progress = _hrUnitOfWork.LeaveRepository.AnnualLeavesProgress(EmpId, StartDate, Language);
            ViewBag.RepEmps = _hrUnitOfWork.LeaveRepository.GetReplaceEmpList(EmpId, Language);
        }

        [HttpPost]
        public ActionResult Details(LeaveReqViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveRequest",
                        TableName = "LeaveRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));

            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);
            var type = _hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);

            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";

            if(type.AllowFraction && model.DayFraction != 0)
            {
                switch (model.DayFraction)
                {
                    case 1:
                    case 3: model.NofDays = 0.25f; break;
                    case 2:
                    case 4: model.NofDays = 0.5f; break;
                }
            }

            ///Save
            if (model.Id == 0)
            { /// New
                request = new LeaveRequest();
                model.BalanceBefore = model.BalBefore;

                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "LeaveRequest", Options = moreInfo, Transtype = TransType.Insert };
                AutoMapper(parms);
                request.CompanyId = CompanyId;
                request.PeriodId = _hrUnitOfWork.LeaveRepository.GetLeaveRequestPeriod(type.CalendarId, request.StartDate, Language, out message);
                request.ApprovalStatus = (byte)(model.submit ? (type.ExWorkflow ? 6 : 2) : 1); //ApprovalStatus 1- New, 2- Submit 6- Approved //AbsenceType 8- Casual

                if (type.ExWorkflow && model.submit)
                {
                    request.ActualStartDate =request.StartDate;
                    request.ActualEndDate =  request.EndDate;
                    request.ActualNofDays =  request.NofDays;
                }

                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                _hrUnitOfWork.LeaveRepository.Add(request);
            }
            else
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "LeaveRequest", Options = moreInfo, Transtype = TransType.Update };
                AutoMapper(parms);

                if (model.submit)
                    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    {
                        ColumnName = "ApprovalStatus",
                        CompanyId = CompanyId,
                        ObjectName = "LeaveRequest",
                        SourceId = request.Id.ToString(),
                        UserName = UserName,
                        Version = Convert.ToByte(Request.Form["Version"]),
                        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                        ValueBefore = MsgUtils.Instance.Trls("Darft"),
                        Transtype = (byte)TransType.Update
                    });

                request.ApprovalStatus = (byte)(model.submit ? (type.ExWorkflow ? 6 : 2) : model.ApprovalStatus); //1- New, 2- Submit 6- Approved //AbsenceType 8- Casual
                request.ModifiedUser = UserName;
                request.ModifiedTime = DateTime.Now;
                if (type.ExWorkflow && model.submit)
                {
                    request.ActualStartDate = request.StartDate;
                    request.ActualEndDate = request.EndDate;
                    request.ActualNofDays = request.NofDays;
                }
                _hrUnitOfWork.LeaveRepository.Attach(request);
                _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
            }

            if (model.submit && type.ExWorkflow && request.ApprovalStatus == 1)
                _hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, UserName);

            // #First save changes
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }


            if (model.submit && type.AbsenceType != 8) //Casual
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "Leave",
                    SourceId = request.TypeId,
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 2,
                    CreatedUser = UserName,
                };

                var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;
                    message += "," + (new JavaScriptSerializer()).Serialize(new { model = request, error = wf.WorkFlowStatus });

                    _hrUnitOfWork.LeaveRepository.Attach(request);
                    _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    _hrUnitOfWork.LeaveRepository.Add(wfTrans);

                // #Second Save changes
                Errors = SaveChanges(Language);
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

            }

            if(message == "OK")
                message += "," + ((new JavaScriptSerializer()).Serialize(new { model = request }));

            return Json(message);
        }

        public ActionResult DeleteLeaves(int id)
        {
            List<Error> errors = new List<Error>();

            string message = "OK";

            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = request,
                ObjectName = "LeaveRequest",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.LeaveRepository.DeleteRequest(request, Language);

            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }

        ////Employee Tasks
        public ActionResult ReadLeaveEmpTasks(int EmpId, DateTime StartDate)
        {
            int? subPeriod;
            string message = _hrUnitOfWork.CheckListRepository.GetTaskSubPeriod(CompanyId, StartDate, Language, out subPeriod);
            if(!string.IsNullOrEmpty(message))
                return Json(message, JsonRequestBehavior.AllowGet);

            return Json(_hrUnitOfWork.LeaveRepository.ReadLeaveEmpTasks(EmpId, subPeriod.GetValueOrDefault(), Language), JsonRequestBehavior.AllowGet);
        }

        //Perv Leave Requests
        public ActionResult PrevEmpLeaves(int empId, DateTime startDate)
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            ViewBag.EmpId = empId;
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            return PartialView("_PrevEmpLeaves");
        }

        public ActionResult ReadEmpPrevLeaves(int empId, DateTime startDate)
        {
            return Json(_hrUnitOfWork.LeaveRepository.ReadEmpLeaveTrans(empId, startDate, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        //Group Leave
        public ActionResult GroupIndex()
        {
            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetLeaveTypesList(CompanyId, Language);
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId); //for Calender
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult CheckGroup(LeaveReqViewModel model, int[] grid1)
        {
            //grid1 = Depts

            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "GroupLeave",
                        TableName = "LeaveRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));

            List<ErrorMessage> Msgs;
            var grid = _hrUnitOfWork.LeaveRepository.CheckGroupLeaveGrid(model, grid1, CompanyId, Language, out Msgs);
            if (grid == null && Msgs.Count > 0)
                return Json(Msgs.Select(a => new { Field = a.field, Message = a.message }));

            return Json("OK," + (new JavaScriptSerializer().Serialize(grid)));
        }

        [HttpPost]
        public ActionResult SaveGroup(LeaveReqViewModel model, IEnumerable<GroupLeaveViewModel> grid1, int[] grid2, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "GroupLeave",
                        TableName = "LeaveRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));

            string message = "OK";
            LeaveType type = _hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);
            int PeriodId;
            List<ErrorMessage> Errors = _hrUnitOfWork.LeaveRepository.CheckGroupLeave(model, type, Language, out PeriodId);
            if (Errors.Count() > 0)
                return Json(Errors.Select(a => new { Field = a.field, Message = a.message }));

            byte version;
            byte.TryParse(Request.Form["version"], out version);

            if (grid1 != null)
            {
                GroupLeave groupLeave = new GroupLeave();

                AutoMapper(new AutoMapperParm() { Source = model, Destination = groupLeave, Version = version, ObjectName = "GroupLeave", Options = moreInfo });
                groupLeave.PeriodId = PeriodId;
                groupLeave.ApprovalStatus = 6; //Approved
                groupLeave.CompanyId = CompanyId;
                groupLeave.CreatedUser = UserName;
                groupLeave.CreatedTime = DateTime.Now;
                _hrUnitOfWork.LeaveRepository.Add(groupLeave);

                foreach (var item in grid1)
                {
                    GroupLeaveLog leaveLog = new GroupLeaveLog();
                    leaveLog.GroupLeave = groupLeave;
                    leaveLog.EmpId = item.EmpId;
                    leaveLog.Approved = item.Approve;
                    leaveLog.Success = item.Success;
                    leaveLog.ReasonCode = item.ReasonCode;
                    leaveLog.Reason = item.Reason;

                    _hrUnitOfWork.LeaveRepository.Add(leaveLog);

                    if (item.Approve)
                    {//Leave Trans
                        LeaveRequest request = new LeaveRequest { EmpId = item.EmpId, TypeId = model.TypeId, PeriodId = PeriodId, CompanyId = CompanyId, ActualStartDate = model.StartDate, ActualNofDays = model.NofDays };
                        _hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, UserName);
                    }
                }

                errors = SaveChanges(Language);
                if (errors.Count > 0)
                    message = errors.First().errors.First().message;
            }
            return Json(message);
        }

    }

}
