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
    public class ComplaintController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        public ComplaintController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
     
        #region ComplaintRequest 
        public List<FormList> ApprovelStutesList()
        {
            List<FormList> ApprovelStytes = new List<FormList>();
            ApprovelStytes.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Darft") });
            ApprovelStytes.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Submit") });
            ApprovelStytes.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Employee Review") });
            ApprovelStytes.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("Cancel before accepted") });
            ApprovelStytes.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Rejected") });
            ApprovelStytes.Add(new FormList { id = 6, name = MsgUtils.Instance.Trls("Accepted") });
            ApprovelStytes.Add(new FormList { id = 7, name = MsgUtils.Instance.Trls("Cancel after accepted") });
            ApprovelStytes.Add(new FormList { id = 8, name = MsgUtils.Instance.Trls("Manager Review") });
            return ApprovelStytes;
        }
        [ResponseType(typeof(ComplaintIndexViewModel)), HttpGet]
        [Route("api/Complaint/GetComplaintRequests")]
        public IHttpActionResult GetComplaintRequests(int MenuId)
        {
            List<FormList> ComplaintType = new List<FormList>();
            ComplaintType.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Complaint") });
            ComplaintType.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Grievance") });
            ComplaintType.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Enquiry") });
            List<FormList> Against = new List<FormList>();
            Against.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Employee") });
            Against.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Manager") });
            Against.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Procedure") });
            Against.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("Decision") });
            Against.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Other") });

            List<FormList> ApprovelStytes = ApprovelStutesList();

            var empId = User.Identity.GetEmpId();
            var query = _hrUnitOfWork.ComplaintRepository.GetComplaintRequest(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            whecls = "(EmpId=1027)";
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.AsQueryable().Where(whecls);
                  
                }
                catch (Exception ex)
                {
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            foreach (var item in query)
            {
                item.AginstName = Against.Where(a => a.id == item.Against).Select(a => a.name).FirstOrDefault();
                item.ComplaintTypeName = ComplaintType.Where(a => a.id == item.ComplainType).Select(a => a.name).FirstOrDefault();
                item.ApproveName = ApprovelStytes.Where(a => a.id == item.ApprovalStatus).Select(a => a.name).FirstOrDefault();
            }
            return Ok(query);
        }

        [ResponseType(typeof(ComplaintRequestViewModel)), HttpGet]
        [Route("api/Complaint/GetComplaint")]
        public IHttpActionResult GetComplaint(int id = 0)
        {
            List<FormList> ComplaintType = new List<FormList>();
            ComplaintType.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Complaint") });
            ComplaintType.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Grievance") });
            ComplaintType.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Enquiry") });

            List<FormList> Against = new List<FormList>();
            Against.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Employee") });
            Against.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Manager") });
            Against.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Procedure") });
            Against.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("Decision") });
            Against.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Other") });

            var AgainstLst = Against;
            var ComplainTypeLst = ComplaintType;

            ComplaintRequestViewModel request;
            if (id == 0)
                request = new ComplaintRequestViewModel();
            else
                request = _hrUnitOfWork.ComplaintRepository.ReadComplaint(id);
            if (request == null)
                return NotFound();
            return Ok(new { request = request, ComplainTypeLst = ComplainTypeLst, AgainstLst = AgainstLst });
        }
        [HttpDelete]
        [Route("api/Complaint/DeleteComplaint")]
        public IHttpActionResult DeleteComplaint(int id)
        {
            string message = "Ok";
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(id);
            if (request.ApprovalStatus == 1)
            {
                _hrUnitOfWork.ComplaintRepository.Remove(request);
                message = "Ok";
            }
            else
                message = "CantDelete";
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;
            return Ok(message);
        }



        [ResponseType(typeof(ComplaintRequestViewModel)), HttpPost]
        [Route("api/Complaint/SaveComplaint")]
        public IHttpActionResult SaveComplaint(ComplaintRequestViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();
            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));

            //  return Ok(ModelState);

            model.EmpId = User.Identity.GetEmpId();
            model.EmpId = 1027;
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(model.Id);
            string message = "Ok";
            ///Save
            if (model.Id == 0)
            { /// New
                request = new ComplainRequest();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request};
                AutoMapper(parms);
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                request.CreatedUser = User.Identity.Name;
                request.CreatedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.CompanyId = User.Identity.GetDefaultCompany();
                _hrUnitOfWork.ComplaintRepository.Add(request);
            }
            else if (model.ApprovalStatus == 3 || model.ApprovalStatus == 1)
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request };
                AutoMapper(parms);
                if (model.submit)
                    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    {
                        ColumnName = "ApprovalStatus",
                        CompanyId = User.Identity.GetDefaultCompany(),
                        ObjectName = "ComplainRequests",
                        SourceId = request.Id.ToString(),
                        UserName = User.Identity.Name,
                        Version = 0,
                        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                        ValueBefore = MsgUtils.Instance.Trls("Darft")
                    });

                request.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus); //1- New, 2- Submit
                request.ModifiedUser = User.Identity.Name;
                request.ModifiedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                _hrUnitOfWork.ComplaintRepository.Attach(request);
                _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;
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
                    Source = "Complaint" + model.Against,
                    SourceId = User.Identity.GetDefaultCompany(),
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 2,
                    CreatedUser = User.Identity.Name,
                };
                var wfTrans = _hrUnitOfWork.ComplaintRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;
                    message = wf.WorkFlowStatus;
                }
                else if (wfTrans != null)
                    _hrUnitOfWork.LeaveRepository.Add(wfTrans);
            }

            Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Ok(message);

        }



        #endregion

        #region ComplaintFollowUp

        public IHttpActionResult ReadComplaintFollowUp(int MenuId)
        {
            int CompanyId = User.Identity.GetDefaultCompany();
            string Lang = User.Identity.GetLanguage();
            var query = _hrUnitOfWork.ComplaintRepository.GetComplaintReqFollowUp(CompanyId, Lang);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    Models.Utils.LogError(ex.Message);
                }
            }
            return Ok(query);
        }


        #endregion
    }
}
