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
    public class LeavePostingController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        public LeavePostingController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public List<FormList> ApprovelStutesList()
        {
            List<FormList> ApprovelStytes = new List<FormList>();
            ApprovelStytes.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Submit") });
            ApprovelStytes.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Accepted") });
            ApprovelStytes.Add(new FormList { id = 6, name = MsgUtils.Instance.Trls("Approved") });
            ApprovelStytes.Add(new FormList { id = 9, name = MsgUtils.Instance.Trls("Rejected") });
            return ApprovelStytes;
        }

        #region Accepted Leaves

        [ResponseType(typeof(LeaveReqGridViewModel)), HttpGet]
        [Route("api/LeavePosting/AcceptedLeaves")]
        public IHttpActionResult AcceptedLeaves(int MenuId)
        {
            List<FormList> ApprovelStytes = ApprovelStutesList();
            var query = _hrUnitOfWork.LeaveRepository.GetApprovedLeaveReq(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    foreach (var item in query)
                    {
                        item.ApproveName = ApprovelStytes.Where(a => a.id == int.Parse(item.ApprovalStatus)).Select(a => a.name).FirstOrDefault();
                    }
                    query = query.AsQueryable().Where(whecls);

                }
                catch (Exception ex)
                {
                    //TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            return Ok(query);
        }
        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/LeavePosting/GetAcceptedLeave")]
        public IHttpActionResult GetAcceptedLeave(int id)
        {
            LeaveReqViewModel request = _hrUnitOfWork.LeaveRepository.GetRequest(id, User.Identity.GetLanguage());
            var CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCode(User.Identity.GetLanguage(), "LeaveCancelReason");
            //  List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("LeaveReqFollowUpForm", User.Identity.GetDefaultCompany(), Version);
            //if (columns.FirstOrDefault(fc => fc == "ReplaceEmpId") == null)
            //    ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            List<FormList> ApprovelStytes = ApprovelStutesList();
            var LeaveTypes = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(User.Identity.GetEmpId(), User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()).Select(t => new { id = t.Id, name = t.Name });
            var ReqReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCode(User.Identity.GetLanguage(), "LeaveReason");
            var Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            var CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); 
            var Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(User.Identity.GetDefaultCompany());

            return Ok( new { request = request, Calender = Calender, CalcOptions = CalcOptions ,Employees = Employees ,
                LeaveTypes = LeaveTypes , ReqReasons= ReqReasons ,ApprovalStutes= ApprovelStytes,CanselReasonLst= CanselReasons
            });
        }

        #endregion


        //public ActionResult FollowupDetails(LeaveReqViewModel model, OptionsViewModel moreInfo)
        //{
        //    List<Error> Errors = new List<Error>();
        //    LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);
        //    if (ModelState.IsValid)
        //    {
        //        if (ServerValidationEnabled)
        //        {
        //            Errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
        //            {
        //                CompanyId = User.Identity.GetDefaultCompany(),
        //                ObjectName = "LeaveReqFollowUpForm",
        //                TableName = "LeaveRequests",
        //                Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
        //                ParentColumn = "Id",
        //                Culture = User.Identity.GetLanguage()
        //            });

        //            if (Errors.Count() > 0)
        //            {
        //                foreach (var e in Errors)
        //                {
        //                    foreach (var errorMsg in e.errors)
        //                    {
        //                        ModelState.AddModelError(errorMsg.field, errorMsg.message);
        //                    }
        //                }
        //                return Json(Models.Utils.ParseFormErrors(ModelState));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Json(Models.Utils.ParseFormErrors(ModelState));
        //    }

        //    AutoMapper(new Models.AutoMapperParm
        //    {
        //        Destination = request,
        //        Source = model,
        //        ObjectName = "LeaveRequest",
        //        Version = Convert.ToByte(Request.Form["Version"]),
        //        Options = moreInfo,
        //        Transtype = TransType.Update
        //    });

        //    if (request.ApprovalStatus != 9)
        //    {
        //        request.RejectDesc = null;
        //        request.RejectReason = null;
        //    }

        //    if (request.ApprovalStatus != 5)
        //    {
        //        request.ActualStartDate = null;
        //        request.ActualNofDays = null;
        //        request.ActualEndDate = null;
        //    }
        //    request.ModifiedTime = DateTime.Now;
        //    request.ModifiedUser = User.Identity.Name;

        //    if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
        //    {
        //        string msg = AddWFTrans(request, null, null);
        //        if (msg == "Success") ChangeAssignmentStatus(request);
        //        else if (msg.Length > 0)
        //            return Json(msg);

        //        // change here to 6
        //        if (request.ApprovalStatus == 6) _hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, User.Identity.Name);
        //    }

        //    _hrUnitOfWork.LeaveRepository.Attach(request);
        //    _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

        //    Errors = SaveChanges(User.Identity.GetLanguage());
        //    if (Errors.Count > 0)
        //    {
        //        var message = Errors.First().errors.First().message;
        //        return Json(message);
        //    }

        //    return Json("OK");
        //}




    }
}






