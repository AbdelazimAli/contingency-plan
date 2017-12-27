using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.Controllers.Api;
using WebApp.Models;

namespace WebApp.Controllers.NewApi
{
    public class EmpLeavesVm
    {
        public int CompanyId { get; set; }
        public string Culture { get; set; }
        public int EmpId { get; set; }
    }
    public class GetLeaveTypesData
    {
        public GetLeaveTypesData()
        {
            LeaveTypeList = new List<DropDownList>();
            ChartData = new List<LeaveTransCounts>();
            Replacements = new List<FormList>();
            LeaveReasonList = new List<LookupCodesViewModel>();
        }
        public IList<DropDownList> LeaveTypeList { get; set; }
        public IEnumerable<LeaveTransCounts> ChartData { get; set; }
        public IEnumerable<FormList> Replacements { get; set; }
        public IEnumerable<LookupCodesViewModel> LeaveReasonList { get; set; }
    }

    public class RequestData
    {
        public int CompanyId { get; set; }
        public int TypeId { get; set; }
        public int EmpId { get; set; }
        public int RequestId { get; set; }
        public string Culture { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class ReturnedLeaveData
    {
        public LeaveType LeaveType { get; set; }
        public CalenderViewModel Calender { get; set; }
        public RequestValidationViewModel requestVal { get; set; }

    }
    public class DeleteRequest
    {
        [Required]
        public int id { get; set; }
        public string Language { get; set; }
    }
    public class LeaveReqVM
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public short? ReqReason { get; set; } // lookup code LeaveReason
        public float? BalBefore { get; set; }
        public float? BalanceBefore { get; set; }
        public bool submit { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int? ReplaceEmpId { get; set; }
        public float? NofDays { get; set; } = 0;
        public byte? DayFraction { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public string Culture { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public byte ApprovalStatus { get; set; } = 1;
        [MaxLength(250)]
        public string ReasonDesc { get; set; }


    }


    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LeavesController : BaseApiController
    {
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public LeavesController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/GetEmpLeaves")]
        public IHttpActionResult GetEmpLeaves(EmpLeavesVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                var list = hrUnitOfWork.LeaveRepository.ReadLeaveRequests(model.CompanyId, model.Culture).Where(l => l.EmpId == model.EmpId).ToList();
                if (list == null)
                {
                    return NotFound();
                   
                }
                return Ok(list);
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/GetLeaveTypes")]
        public IHttpActionResult GetLeaveTypes(EmpLeavesVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var LeaveTypeList = hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(model.EmpId, model.CompanyId, model.Culture).ToList();
            var ChartData = hrUnitOfWork.LeaveRepository.AnnualLeavesProgress(model.EmpId, DateTime.Now, model.Culture).ToList();
            var Replacements = hrUnitOfWork.LeaveRepository.GetReplaceEmpList(model.EmpId, model.Culture).ToList();
            var LeaveReasons = hrUnitOfWork.LookUpRepository.GetLookUpCodes("LeaveReason", model.Culture).ToList();

            var res = new GetLeaveTypesData() { ChartData = ChartData, LeaveTypeList = LeaveTypeList, LeaveReasonList = LeaveReasons, Replacements = Replacements };
            if (LeaveTypeList == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/GetRequestLeaveData")]
        public IHttpActionResult GetRequestLeaveData(RequestData model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var LeaveType = hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);
            var Calender = hrUnitOfWork.LeaveRepository.GetHolidays(model.CompanyId);
            RequestValidationViewModel requestVal = new RequestValidationViewModel() { ExDayOff = LeaveType.ExDayOff, ExHolidays = LeaveType.ExHolidays, MaxDays = LeaveType.MaxDays };
            ReqDaysParamVM param = new ReqDaysParamVM() { EmpId = model.EmpId, culture = model.Culture, RequestId = model.RequestId, type = LeaveType, StartDate = model.StartDate };
            hrUnitOfWork.LeaveRepository.GetLeaveBalance(ref requestVal, param);
            ReturnedLeaveData Konafa = new ReturnedLeaveData() { Calender = Calender, LeaveType = LeaveType, requestVal = requestVal };
            return Ok(Konafa);

        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/PostLeave")]
        public IHttpActionResult PostLeave(LeaveReqVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }
            //string Language = HttpContext.

            var UserName = HttpContext.Current.User.Identity.Name;
            var type = hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);

            //if (type.AllowFraction && (model.FractionDays != 0 && model.FractionDays != null))
            //{
            //    model.NofDays = Math.Abs(model.FractionDays.GetValueOrDefault());

            //}

            if (type.AllowFraction && (model.DayFraction != 0 ))
            {
                switch (model.DayFraction)
                {
                    case 1:
                    case 3: model.NofDays = 0.25f; break;
                    case 2:
                    case 4: model.NofDays = 0.5f; break;
                }
            }

            model.BalanceBefore = model.BalBefore;
            LeaveRequest request = new LeaveRequest();
            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "LeaveRequest", Options = null, Transtype = TransType.Insert };
            AutoMapper(parms);

            request.CompanyId = model.CompanyId;
            request.PeriodId = hrUnitOfWork.LeaveRepository.GetLeaveRequestPeriod(type.CalendarId, request.StartDate, model.Culture);
            request.ApprovalStatus = (byte)(model.submit ? (type.AbsenceType == 8 ? 6 : 2) : 1); //ApprovalStatus 1- New, 2- Submit 6- Approved //AbsenceType 8- Casual
            model.ApprovalStatus = request.ApprovalStatus;
            request.CreatedUser = UserName;
            request.CreatedTime = DateTime.Now;
            request.ReasonDesc = model.ReasonDesc;
            request.EndDate = model.EndDate;
            request.ReturnDate = model.ReturnDate;
            request.NofDays = (float)model.NofDays;
            request.DayFraction = (byte)model.DayFraction;
            if (type.AbsenceType == 8 && model.submit)
            {
                request.ActualStartDate = model.StartDate;
                request.ActualEndDate = model.EndDate;
                request.ActualNofDays = model.NofDays;
            }

            hrUnitOfWork.LeaveRepository.Add(request);
            if (model.submit && type.AbsenceType == 8)
                hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, UserName);


            var Errors = SaveChanges(model.Culture);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
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
                var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.Culture);
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;

                    hrUnitOfWork.LeaveRepository.Attach(request);
                    hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    hrUnitOfWork.LeaveRepository.Add(wfTrans);
                Errors = SaveChanges(model.Culture);
                if (Errors.Count > 0)
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }
            model.Id = request.Id;
            return Created(new Uri(Request.RequestUri + "/" + model.Id), model);


        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/PutLeave")]
        public IHttpActionResult PutLeave(LeaveReqVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (model == null)
            {
                return NotFound();
            }
            var UserName = HttpContext.Current.User.Identity.Name;


            var type = hrUnitOfWork.LeaveRepository.GetLeaveType(model.TypeId);

            LeaveRequest request = hrUnitOfWork.LeaveRepository.Get(model.Id);
            //if (type.AllowFraction && (model.FractionDays != 0 && model.FractionDays != null))
            //{
            //    model.NofDays = Math.Abs(model.FractionDays.GetValueOrDefault());

            //}
            if (type.AllowFraction && (model.DayFraction != 0))
            {
                switch (model.DayFraction)
                {
                    case 1:
                    case 3: model.NofDays = 0.25f; break;
                    case 2:
                    case 4: model.NofDays = 0.5f; break;
                }
            }
            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "LeaveRequest", Options = null, Transtype = TransType.Update };
            AutoMapper(parms);

            if (model.submit)
                hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = model.CompanyId,
                    ObjectName = "LeaveRequest",
                    SourceId = request.Id.ToString(),
                    UserName = UserName,
                    Version = 0,
                    ValueAfter = MsgUtils.Instance.Trls("Submit"),
                    ValueBefore = MsgUtils.Instance.Trls("Darft")
                });

            request.ApprovalStatus = (byte)(model.submit ? (type.AbsenceType == 8 ? 6 : 2) : model.ApprovalStatus); //1- New, 2- Submit 6- Approved //AbsenceType 8- Casual
            model.ApprovalStatus = request.ApprovalStatus;
            request.ModifiedUser = UserName;
            request.ModifiedTime = DateTime.Now;
            request.ReasonDesc = model.ReasonDesc;
            request.NofDays = (float)model.NofDays;
            request.DayFraction = (byte)model.DayFraction;
            if (type.AbsenceType == 8 && model.submit)
            {
                request.ActualStartDate = request.StartDate;
                request.ActualEndDate = request.EndDate;
                request.ActualNofDays = request.NofDays;
            }

            hrUnitOfWork.LeaveRepository.Attach(request);
            hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
            if (model.submit && type.AbsenceType == 8)
                hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, UserName);

            var Errors = SaveChanges(model.Culture);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
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
                var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.Culture);
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;

                    hrUnitOfWork.LeaveRepository.Attach(request);
                    hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    hrUnitOfWork.LeaveRepository.Add(wfTrans);
                Errors = SaveChanges(model.Culture);
                if (Errors.Count > 0)
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }
            return Ok(model);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/DeleteLeave")]
        public IHttpActionResult DeleteLeave(DeleteRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            LeaveRequest request = hrUnitOfWork.LeaveRepository.Get(model.id);
            hrUnitOfWork.LeaveRepository.DeleteRequest(request, model.Language);
            var Errors = SaveChanges(model.Language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            return Ok(Errors);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/ValidateLeaveRequest")]
        public IHttpActionResult ValidateLeaveRequest(ValidateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            var validate = hrUnitOfWork.LeaveRepository.CheckLeaveRequestApi(model.TypeId, model.EmpId, model.StartDate, model.EndDate, model.Culture, model.Id, true, model.CompanyId, model.ReplaceEmpId);
            return Ok(validate);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/CancelLeave")]
        public IHttpActionResult CancelLeave(CancelVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            List<Error> errors = new List<Error>();

            var leaveReq = hrUnitOfWork.LeaveRepository.Get(model.RequestId);


            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = leaveReq, Version = 0, ObjectName = "LeaveRequest", Options = null, Transtype = TransType.Update };
            AutoMapper(parms);


            var UserName = HttpContext.Current.User.Identity.Name;
            //AddTrail(leaveReq.Id, "ApprovalStatus", MsgUtils.Instance.Trls("Cancel after accepted"), MsgUtils.Instance.Trls("Approved"));

            hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = model.CompanyId,
                UserName = UserName,
                Version = 0,
                ColumnName = "ApprovalStatus",
                SourceId = leaveReq.Id.ToString(),
                ValueAfter = MsgUtils.Instance.Trls("Cancel after accepted"),
                ValueBefore = MsgUtils.Instance.Trls("Approved")
            });
            leaveReq.ApprovalStatus = 8;
            ///Cancel change assign state
            hrUnitOfWork.EmployeeRepository.CancelLeaveAssignState(leaveReq, UserName, 0, model.Language);
            ///Cancel LeaveTrans
            var msg = hrUnitOfWork.LeaveRepository.AddCancelLeaveTrans(leaveReq, UserName, model.Language);
            if (msg.Length > 0)
            {
                return Ok(msg);
            }
            leaveReq.ModifiedUser = UserName;
            leaveReq.ModifiedTime = DateTime.Now;
            hrUnitOfWork.LeaveRepository.Attach(leaveReq);
            hrUnitOfWork.LeaveRepository.Entry(leaveReq).State = EntityState.Modified;

            var Errors = SaveChanges(model.Language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return Ok(Errors);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/BreakLeave")]
        public IHttpActionResult BreakLeave(BreakVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            List<Error> errors = new List<Error>();

            var request = hrUnitOfWork.LeaveRepository.Get(model.RequestId);
            var type = hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId);

            var UserName = HttpContext.Current.User.Identity.Name;

            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "LeaveRequest", Options = null, Transtype = TransType.Update };
            AutoMapper(parms);
           
            hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = model.CompanyId,
                UserName = UserName,
                Version = 0,
                ColumnName = "ActualEndDate",
                SourceId = request.Id.ToString(),
                ValueAfter = model.BreakEndDate.ToString(),
                ValueBefore = request.ActualEndDate.ToString()
            });
            hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = model.CompanyId,
                UserName = UserName,
                Version = 0,
                ColumnName = "ActualNofDays",
                SourceId = request.Id.ToString(),
                ValueAfter = model.BreakNofDays.ToString(),
                ValueBefore = request.ActualNofDays.ToString()
            });
            float DiffDays = request.ActualNofDays.Value - model.BreakNofDays.Value;

            request.ActualEndDate = model.BreakEndDate;
            request.ActualNofDays = model.BreakNofDays;
            request.ReturnDate = request.StartDate.AddDays((double)model.BreakNofDays );
            request.NofDays = (float)model.BreakNofDays;
          //  if (type.AbsenceType == 8)
          //  {
                request.EndDate = (DateTime)request.ActualEndDate;           
           // }
            ///Brake Leave LeaveTrans
            hrUnitOfWork.LeaveRepository.AddBreakLeaveTrans(request, DiffDays, UserName);
            request.ModifiedUser = UserName;
            request.ModifiedTime = DateTime.Now;
            hrUnitOfWork.LeaveRepository.Attach(request);
            hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(model.Language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return Ok(Errors);

        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/EditLeave")]
        public IHttpActionResult EditLeave(EditVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }

            List<Error> errors = new List<Error>();

            var request = hrUnitOfWork.LeaveRepository.Get(model.RequestId);
            var type = hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId);
            var UserName = HttpContext.Current.User.Identity.Name;

            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "LeaveRequest", Options = null, Transtype = TransType.Update };
            AutoMapper(parms);

            hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = model.CompanyId,
                UserName = UserName,
                Version = 0,
                ColumnName = "ActualStartDate",
                SourceId = request.Id.ToString(),
                ValueAfter = model.EditedStartDate.ToString(),
                ValueBefore = request.ActualEndDate.ToString()
            });
            hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = model.CompanyId,
                UserName = UserName,
                Version = 0,
                ColumnName = "ActualEndDate",
                SourceId = request.Id.ToString(),
                ValueAfter = model.EditedEndDate.ToString(),
                ValueBefore = request.ActualNofDays.ToString()
            });


            request.ActualStartDate = model.EditedStartDate;
            request.ActualEndDate = model.EditedEndDate;
            request.EndDate = (DateTime)request.ActualEndDate;
            request.ReturnDate = model.EditedReturnDate;
            request.ModifiedUser = UserName;
            request.ModifiedTime = DateTime.Now;
            hrUnitOfWork.LeaveRepository.Attach(request);
            hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(model.Language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return Ok(Errors);
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("newApi/Leaves/GetHolidays")]
        public IHttpActionResult GetHolidays(int? compId)
        {
            if (compId == null)
            {
                return BadRequest();
            }
            else
            {
                var weeKend = hrUnitOfWork.LeaveRepository.GetHolidays((int)compId);
                var Standard = hrUnitOfWork.LeaveRepository.ReadStanderedHolidays((int)compId).ToList();
                var customs = hrUnitOfWork.LeaveRepository.ReadCustomHolidays((int)compId).ToList();
                var request = new { weeKend1 = weeKend.weekend1, weekend2=weeKend.weekend2, WorkStartTime= weeKend.WorkStartTime,WorkHours = weeKend.WorkHours, Standard = Standard, Customs = customs }; 
                if (request == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Leaves/GetLeavesTrans")]
        public IHttpActionResult GetLeavesTrans(LeavesTransVM Trans)
        {
            if (Trans == null)
            {
                return BadRequest();
            }
            else
            {
                
                var request = hrUnitOfWork.LeaveRepository.ReadEmpLeaveTrans(Trans.EmpId,Trans.StartDate,Trans.CompanyId,Trans.Culture);
                if (request == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
        }


    }

    public class LeavesTransVM
    {
        public int EmpId { get; set; }
        public DateTime StartDate { get; set; }
        public int CompanyId { get; set; }
        public string Culture { get; set; }
    }
    public class CancelVM
    {
        public int RequestId { get; set; }
        public string Language { get; set; }
        public int CompanyId { get; set; }
    }
    public class BreakVM
    {
        public int RequestId { get; set; }
        public string Language { get; set; }
        public int CompanyId { get; set; }
        public DateTime BreakEndDate { get; set; }
        public float? BreakNofDays { get; set; }
    }
    public class EditVM
    {
        public int RequestId { get; set; }
        public string Language { get; set; }
        public int CompanyId { get; set; }
        public DateTime EditedStartDate { get; set; }
        public DateTime EditedEndDate { get; set; }
        public DateTime EditedReturnDate { get; set; }
    }
    public class ValidateVM
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Culture { get; set; }
        public int? ReplaceEmpId { get; set; }

    }

}
