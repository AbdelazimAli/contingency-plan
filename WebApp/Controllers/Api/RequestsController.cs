using Interface.Core;
//using Microsoft.Office.Interop.Excel;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Web.Http.ModelBinding;
using System.Web;
using System.Linq.Dynamic;

namespace WebApp.Controllers.Api
{
    public class RequestsController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        public RequestsController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        #region LeaveRequst
        public List<FormList> ApprovelStutesList()
        {
            List<FormList> ApprovelStytes = new List<FormList>();
            ApprovelStytes.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Darft") });
            ApprovelStytes.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Submit") });
            ApprovelStytes.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Employee Review") });
            ApprovelStytes.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("ManagerReview") });
            ApprovelStytes.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Accepted") });
            ApprovelStytes.Add(new FormList { id = 6, name = MsgUtils.Instance.Trls("Approved") });
            ApprovelStytes.Add(new FormList { id = 7, name = MsgUtils.Instance.Trls("Cancel after accepted") });
            ApprovelStytes.Add(new FormList { id = 8, name = MsgUtils.Instance.Trls("Cancel after accepted") }); 
             ApprovelStytes.Add(new FormList { id = 9, name = MsgUtils.Instance.Trls("Rejected") });
            return ApprovelStytes;
        }
        [ResponseType(typeof(LeaveRequestViewModel)), HttpGet]
        [Route("api/Requests/GetLeaveRequests")]
        public IHttpActionResult GetLeaveRequests(int MenuId)
        {
            var query = _hrUnitOfWork.LeaveRepository.ReadLeaveRequests(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            List<FormList> ApprovelStytes = ApprovelStutesList();
            string whecls = GetWhereClause(MenuId);
            whecls = "(EmpId=1054)";
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }

            var ReturnQuery = query.ToList();
            foreach (var item in ReturnQuery)
            {
                item.ApprovalName = ApprovelStytes.Where(a => a.id == item.ApprovalStatus).Select(a => a.name).FirstOrDefault();
            }
            return Ok(ReturnQuery);
        }
        //RangeFilter
        [ResponseType(typeof(LeaveRequestViewModel)), HttpGet]
        [Route("api/Requests/FilterRangeLeaveRequests")]
        public IHttpActionResult FilterRangeLeaveRequests(int range)
        {
            DateTime startRange, endRange;
            DateTime Today = DateTime.Today;
            startRange = Today;
            endRange = Today;

            switch (range)
            {
                case 1: //This Month
                    startRange = new DateTime(Today.Year, Today.Month, 1);
                    //endRange = new DateTime(Today.Year, Today.Month,30);
                    endRange = startRange.AddMonths(1).AddDays(-1);
                    break;
                case 2: //Last Month
                    startRange = new DateTime(Today.Year, Today.Month - 1, 1);
                    endRange = startRange.AddMonths(1).AddDays(-1);
                    break;
                case 3: //This Year
                    startRange = new DateTime(Today.Year, 1, 1);
                    endRange = startRange.AddYears(1).AddDays(-1);
                    break;
                case 4://Last Year
                    startRange = new DateTime(Today.Year - 1, 1, 1);
                    endRange = startRange.AddYears(1).AddDays(-1);
                    break;
            }
            var query = _hrUnitOfWork.LeaveRepository.ReadLeaveRequests(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage())
                .Where(l => l.EmpId == 1027 && (l.StartDate >= startRange && l.StartDate <= endRange));
            return Ok(query);
        }
        [ResponseType(typeof(LeaveRequestViewModel)), HttpGet]
        [Route("api/Requests/FilterRangeCustomLeaveRequests")]
        public IHttpActionResult FilterRangeCustomLeaveRequests(DateTime from, DateTime to)
        {
            var query = _hrUnitOfWork.LeaveRepository.ReadLeaveRequests(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage())
                .Where(l => l.EmpId == 1027 && (l.StartDate >= from && l.StartDate <= to));
            return Ok(query);
        }
        [ResponseType(typeof(FormList)), HttpGet]
        [Route("api/Requests/GetLeaveType")]
        public IHttpActionResult GetLeaveType()
        {
            var list = _hrUnitOfWork.LeaveRepository.GetLeaveTypes(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Select(a => new FormList { id = a.Id, name = a.LocalName });
            return Ok(list);
        }

        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/Requests/GetLeave")]
        public IHttpActionResult GetLeave(int id = 0)
        {
            var LeaveTypes = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(1054, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Select(t => new { id = t.Id, name = t.Name });
            var Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Where(e => e.id != User.Identity.GetEmpId()).ToList();
            var ReqReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCode(User.Identity.GetLanguage(), "LeaveReason");
            var Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(User.Identity.GetDefaultCompany()); //for Calender

            LeaveReqViewModel request;
            LeaveType CalcOptions = null;
            if (id == 0)
                request = new LeaveReqViewModel();
            else
            {
                request = _hrUnitOfWork.LeaveRepository.GetRequest(id, User.Identity.GetLanguage());
                CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); //for Calender
            }
            if (request == null)
                return NotFound();

            return Ok(new { request = request, LeaveTypes = LeaveTypes, Employees = Employees, ReqReasons = ReqReasons, Calender = Calender, CalcOptions = CalcOptions });
        }

        [ResponseType(typeof(LeaveType)), HttpGet]
        [Route("api/Requests/GetCalcOptions")]
        public IHttpActionResult GetCalcOptions(int typeId)
        {
            return Ok(_hrUnitOfWork.LeaveRepository.GetLeaveType(typeId));
        }

        [HttpGet]
        [Route("api/Requests/CheckLeave")]
        public IHttpActionResult CheckLeave(int TypeId, float NofDays, DateTime StartDate, DateTime EndDate, int ReqId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.CheckLeaveRequest(TypeId, User.Identity.GetEmpId(), StartDate, EndDate, NofDays, User.Identity.GetLanguage(), ReqId, true, User.Identity.GetDefaultCompany()));
        }

        [ResponseType(typeof(LeaveReqViewModel)), HttpPost]
        [Route("api/Requests/SaveLeave")]
        public IHttpActionResult SaveLeave(LeaveReqViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();

            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));


            model.EmpId = User.Identity.GetEmpId();
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);
            var type = _hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);

            if (model.ReqReason == null && type.MustAddCause == true)
            {
                ModelState.AddModelError("ReqReason", MsgUtils.Instance.Trls("Required"));
                return Ok(ModelState);
            }

            string message = "Ok";

            ///Save
            if (model.Id == 0)
            { /// New
                request = new LeaveRequest();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request };
                AutoMapper(parms);
                request.PeriodId = _hrUnitOfWork.LeaveRepository.GetLeaveRequestPeriod(type.CalendarId, request.StartDate, User.Identity.GetLanguage(), out message);
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                request.CreatedUser = User.Identity.Name;
                request.CreatedTime = DateTime.Now;
                _hrUnitOfWork.LeaveRepository.Add(request);
            }
            else
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request }; //, ObjectName = "LeaveRequest"
                AutoMapper(parms);

                if (model.submit)
                    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    {
                        ColumnName = "ApprovalStatus",
                        CompanyId = User.Identity.GetDefaultCompany(),
                        ObjectName = "LeaveRequest",
                        SourceId = request.Id.ToString(),
                        UserName = User.Identity.Name,
                        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                        ValueBefore = MsgUtils.Instance.Trls("Darft")
                    });

                request.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus); //1- New, 2- Submit
                request.ModifiedUser = User.Identity.Name;
                request.ModifiedTime = DateTime.Now;
                _hrUnitOfWork.LeaveRepository.Attach(request);
                _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
            }

            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Ok(message);
            }

            if (model.submit)
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "Leave",
                    SourceId = request.TypeId,
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 2,
                    CreatedUser = User.Identity.Name,
                };
                var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;
                    message += "," + (new JavaScriptSerializer()).Serialize(new { model = request, error = wf.WorkFlowStatus });

                    _hrUnitOfWork.LeaveRepository.Attach(request);
                    _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    _hrUnitOfWork.LeaveRepository.Add(wfTrans);
            }

            Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;


            return Ok(message);
        }

        [HttpDelete]
        [Route("api/Requests/Delete")]
        public IHttpActionResult Delete(int id)
        {
            string message = "Ok";
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(id);
            var isDeleted = _hrUnitOfWork.LeaveRepository.DeleteRequest(request, User.Identity.GetLanguage());
            message = isDeleted ? "Ok" : "CantDelete";
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Ok(message);
        }

        #endregion

        #region LeaveTrans

        //[ResponseType(typeof(LeaveTransViewModel)), HttpGet]
        //[Route("api/Requests/GetLeaveTrans")]
        //public IHttpActionResult GetLeaveTrans(int MenuId)
        //{
        //    var query = _hrUnitOfWork.LeaveRepository.GetLeaveTrans(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
        //    string whecls = GetWhereClause(MenuId);
        //    if (whecls.Length > 0)
        //    {
        //        try
        //        {
        //            query = query.Where(whecls);
        //        }
        //        catch (Exception ex)
        //        {
        //            //TempData["Error"] = ex.Message;
        //            Models.Utils.LogError(ex.Message);
        //            return Ok("");
        //        }
        //    }
        //    return Ok(query);
        //}
        //[ResponseType(typeof(LeaveTransSummary)), HttpGet]
        //[Route("api/Requests/GetLeaveTransSummary")]
        //public IHttpActionResult GetLeaveTransSummary(int MenuId)
        //{
        //    var query = _hrUnitOfWork.LeaveRepository.GetLeaveTransSummary(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
        //    string whecls = GetWhereClause(MenuId);
        //    if (whecls.Length > 0)
        //    {
        //        try
        //        {
        //            query = query.Where(whecls);
        //        }
        //        catch (Exception ex)
        //        {
        //            //TempData["Error"] = ex.Message;
        //            Models.Utils.LogError(ex.Message);
        //            return Ok("");
        //        }
        //    }
        //    return Ok(query);
        //}



        #endregion
        #region Menu
        //GetColumns
        [ResponseType(typeof(UsersViewModel)), HttpGet]
        [Route("api/Requests/GetColumns")]
        public IHttpActionResult GetColumns(string objectName,int version,int companyId)
        {
            var q = _hrUnitOfWork.PagesRepository.GetColumns(objectName,companyId,version);
            return Ok(q);
        }
        //GetMenu
        [ResponseType(typeof(UsersViewModel)), HttpGet]
        [Route("api/Requests/GetMenu")]
        public IHttpActionResult GetMenu()
        {
            var q = _hrUnitOfWork.PagesRepository.GetMenu(User.Identity.GetLanguage(),User.Identity.GetDefaultCompany());
            return Ok(q);
        }
        #endregion

        #region LeaveFollowUp
        [ResponseType(typeof(LeaveRequestViewModel)), HttpGet]
        [Route("api/Requests/ReadLeaveFollowUp")]
        public IHttpActionResult ReadLeaveFollowUp(int MenuId)
        {
            int CompanyId = User.Identity.GetDefaultCompany();
            string Language = User.Identity.GetLanguage();
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveReqFollowUp(CompanyId, Language);
            List<FormList> ApprovelStytes = ApprovelStutesList();
            //ApprovalStatusName
            //string whecls = GetWhereClause(MenuId);
            //if (whecls.Length > 0)
            //{
            //    try
            //    {
            //        query = query.Where(whecls);
            //    }
            //    catch (Exception ex)
            //    {
            //        Models.Utils.LogError(ex.Message);
            //    }
            //}
            var ReturnQuery = query.ToList();
            foreach (var item in ReturnQuery)
            {
                item.ApproveName = ApprovelStytes.Where(a => a.id == int.Parse(item.ApprovalStatus)).Select(a => a.name).FirstOrDefault();
            }
            return Ok(ReturnQuery);
        }
        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/Requests/GetLeaveFollowUp")]
        public IHttpActionResult GetLeaveFollowUp(int id = 0)
        {
            string Language = User.Identity.GetLanguage();

            var LeaveTypes = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(1054, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Select(t => new { id = t.Id, name = t.Name });
            var Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Where(e => e.id != User.Identity.GetEmpId()).ToList();
            var ReqReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCode(User.Identity.GetLanguage(), "LeaveReason");
            var Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(User.Identity.GetDefaultCompany()); //for Calender
            var Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(User.Identity.GetDefaultCompany(), Language).Select(m => new { id = m.Id, name = m.Name });
            var CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("LeaveCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });

            var LeaveRejectLst = _hrUnitOfWork.LookUpRepository.GetLookUpCode(Language, "LeaveRejectReason");
            LeaveReqViewModel request = _hrUnitOfWork.LeaveRepository.GetRequest(id, Language);
            
            var  CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); //for Calender
            return Ok( new { request=request,LeaveTypesLst=LeaveTypes,ReqReasonsLst=ReqReasons,
                EmpLst =Employees ,LeaveRejectLst=LeaveRejectLst, MangerLst= Mangers,
                CanselReasonLst= CanselReasons
            });
        }

        [ResponseType(typeof(LeaveReqViewModel)), HttpPost]
        [Route("api/Requests/FollowupDetails")]
        public IHttpActionResult FollowupDetails(LeaveReqViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();
            string message = "Ok";
            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);
            model.PeriodId = request.PeriodId;
            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request };
            AutoMapper(parms);
            if (request.ApprovalStatus != 9)
            {
                request.RejectDesc = null;
                request.RejectReason = null;
            }

            if (request.ApprovalStatus != 5)
            {
                request.ActualStartDate = null;
                request.ActualNofDays = null;
                request.ActualEndDate = null;
            }
            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = User.Identity.Name;

            if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
            {
                string msg = AddWFTrans(request, null, null);

                // change here to 6
                if (request.ApprovalStatus == 6)
                {
                    //if msg == Success => msg = null || Error
                    if (msg == "Success") msg = ChangeAssignmentStatus(request);
                    if (!string.IsNullOrEmpty(msg))
                        return Json(msg); //ApprovalStatus = 5 because not SaveChanges yet

                    _hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, User.Identity.Name);
                }
            }

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            return Ok(message);
        }
        private string AddWFTrans(LeaveRequest request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Leave",
                SourceId = request.TypeId,
                DocumentId = request.Id,
                RequesterEmpId = request.EmpId,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = User.Identity.Name,
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
            {
                request.ApprovalStatus = 6;
                return wf.WorkFlowStatus;
            }
            else
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
        private string ChangeAssignmentStatus(LeaveRequest request)
        {
            LeaveType type;
            //if future assignment && leave type chanege assignment -> prevent
            string assignError = _hrUnitOfWork.LeaveRepository.CheckAssignStatus(request.EmpId, request.TypeId, out type, User.Identity.GetLanguage());
            if (!string.IsNullOrEmpty(assignError))
                return assignError;

            if (type.AssignStatus != null)
            {
                Assignment copy = _hrUnitOfWork.EmployeeRepository.Find(a => a.EmpId == request.EmpId && a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date).FirstOrDefault();
                short oldAssignStatus = copy.AssignStatus;
                short oldSysAssignStatus = copy.SysAssignStatus;

                copy.EndDate = request.ActualStartDate.Value.AddDays(-1);
                copy.ModifiedUser = User.Identity.Name;
                copy.ModifiedTime = DateTime.Now;
                _hrUnitOfWork.EmployeeRepository.Attach(copy);
                _hrUnitOfWork.EmployeeRepository.Entry(copy).State = EntityState.Modified;

                Assignment assignLeave = new Assignment();
                AutoMapper(new Models.AutoMapperParm() { Source = copy, Destination = assignLeave });
                assignLeave.AssignStatus = type.AssignStatus.Value;
                assignLeave.SysAssignStatus = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => a.CodeName == "Assignment" && a.CodeId == type.AssignStatus.Value).Select(a => a.SysCodeId).FirstOrDefault();
                assignLeave.AssignDate = request.ActualStartDate.Value;
                assignLeave.CreatedUser = User.Identity.Name;
                assignLeave.CreatedTime = DateTime.Now;

                if (type.AutoChangStat == 1) //1-Automatic return to original status before leave
                {
                    assignLeave.EndDate = request.ActualEndDate.Value;
                    _hrUnitOfWork.EmployeeRepository.Add(assignLeave); //i

                    Assignment returnAssign = new Assignment();
                    AutoMapper(new Models.AutoMapperParm() { Source = copy, Destination = returnAssign });
                    returnAssign.AssignStatus = oldAssignStatus;
                    returnAssign.SysAssignStatus = oldSysAssignStatus;
                    returnAssign.AssignDate = request.ActualEndDate.Value.AddDays(1);
                    returnAssign.EndDate = new DateTime(2099, 1, 1);
                    returnAssign.CreatedUser = User.Identity.Name;
                    returnAssign.CreatedTime = DateTime.Now;
                    _hrUnitOfWork.EmployeeRepository.Add(returnAssign); //ii
                }
                else
                {
                    assignLeave.EndDate = new DateTime(2099, 1, 1);
                    _hrUnitOfWork.EmployeeRepository.Add(assignLeave); //i
                }

                var assignmentCodes = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", User.Identity.GetLanguage());
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "AssignStatus",
                    CompanyId = User.Identity.GetDefaultCompany(),
                    ObjectName = "AssignmentsForm",
                    SourceId = copy.Id.ToString(),
                    UserName = User.Identity.Name,
                    ValueAfter = assignmentCodes.FirstOrDefault(a => a.CodeId == type.AssignStatus.Value).Name,
                    ValueBefore = assignmentCodes.FirstOrDefault(a => a.CodeId == oldAssignStatus).Name
                });
            }

            return null;
        }

        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/Requests/SendTo")]
        public IHttpActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(Id);
            string error = "";
            if (Send == 1)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = User.Identity.GetDefaultCompany(),
                    ObjectName = "LeaveRequest",
                    SourceId = Id.ToString(),
                    UserName = User.Identity.Name,
                    ValueAfter = MsgUtils.Instance.Trls("EmployeeReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });
                request.ApprovalStatus = 3;
                error = AddWFTrans(request, null, true);
            }
            else if (Send == 2)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = User.Identity.GetDefaultCompany(),
                    ObjectName = "LeaveRequest",
                    SourceId = Id.ToString(),
                    UserName = User.Identity.Name,
                    ValueAfter = MsgUtils.Instance.Trls("ManagerReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });
                request.ApprovalStatus = 4;
                error = AddWFTrans(request, ManagerId, null);
            }
            if (error.Length > 0)
                return Ok(error);

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Ok(message);
            }
            return Ok("Ok");
        }
        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/Requests/CancelReq")]
        public IHttpActionResult CancelReq(int Id, short? Reason, string Desc)
        {
            int CompanyId = User.Identity.GetDefaultCompany();
            string UserName = User.Identity.Name;
            string Language = User.Identity.GetLanguage();
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "LeaveRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("LeaveCancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("LeaveCancelReason", Language).Where(a => a.CodeId == request.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "LeaveRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                ValueAfter = Desc,
                ValueBefore = request.CancelDesc
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "ApprovalStatus",
                CompanyId = CompanyId,
                ObjectName = "LeaveRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                ValueAfter = MsgUtils.Instance.Trls("Cancel before approved"),
                ValueBefore = MsgUtils.Instance.Trls("Submit")
            });
            request.CancelDesc = Desc;
            request.CancelReason = Reason;
            request.ApprovalStatus = 7;
            // request.ReqStatus = 4;

            string error = AddWFTrans(request, null, null);
            if (error.Length > 0)
                return Ok(error);

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Ok("Ok");
        }
        #endregion


    }

}