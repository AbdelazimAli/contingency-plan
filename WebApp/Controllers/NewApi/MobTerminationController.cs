using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Controllers.Api;

namespace WebApp.Controllers.NewApi
{
    public class MobTerminationController : BaseApiController
    {
        // GET: Termination

        protected IHrUnitOfWork hrUnitOfWork { get; private set; }
        public MobTerminationController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }

        public class TerminationListVm
        {
            public int EmpId { get; set; }
            public string Culture { get; set; }
            public int CompanyId { get; set; }
        }
        public class TerminationFormVM
        {
            public int Id { get; set; } // Request No
            public DateTime? PlanedDate { get; set; } // Planned Date
            public int CompanyId { get; set; }
            public string Culture { get; set; }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/MobTermination/GetTerminationList")]
        public IHttpActionResult GetTerminationList(TerminationListVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var list = hrUnitOfWork.TerminationRepository.ReadEmpTermination(model.EmpId, model.Culture,model.CompanyId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/MobTermination/PostTerminationRequest")]
        public IHttpActionResult PostTerminationRequest(TerminationFormVM model)
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
            var request = hrUnitOfWork.TerminationRepository.Get(model.Id);

            AutoMapper(new Models.AutoMapperParm
            {
                Destination = request,
                Source = model,
                ObjectName = "TermRequestForm",
                Version = 0,
                Options = null,
                Transtype = TransType.Update
            });
            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = UserName;
            request.CompanyId = model.CompanyId;
            //request.ApprovalStatus = 2;
            request.PlanedDate = model.PlanedDate;


            hrUnitOfWork.TerminationRepository.Attach(request);
            hrUnitOfWork.TerminationRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(model.Culture);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            if (request.ApprovalStatus == 2)
            {
                string error = AddWFTrans(request, null, null, model);
                if (error.Length > 0)
                    return Json(error);

                var checklist = hrUnitOfWork.CheckListRepository.GetTermCheckLists(model.CompanyId);
                if (checklist != null)
                {
                    EmpChkList EmpList = hrUnitOfWork.CheckListRepository.AddEmpChlst(checklist, UserName, request.EmpId, model.CompanyId);
                    hrUnitOfWork.CheckListRepository.Add(EmpList);
                    var checkTask = hrUnitOfWork.CheckListRepository.ReadCheckListTask(checklist.Id).ToList();
                    if (checkTask.Count > 0)
                    {
                        hrUnitOfWork.CheckListRepository.AddEmpTask(checkTask, UserName, EmpList);
                    }

                }
            }
            Errors = SaveChanges(model.Culture);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            return Ok(model);
        }

        private string AddWFTrans(Termination request, int? ManagerId, bool? backToEmp, TerminationFormVM model)
        {
            var UserName = HttpContext.Current.User.Identity.Name;
            WfViewModel wf = new WfViewModel()
            {
                Source = "Termination",
                SourceId = model.CompanyId,
                DocumentId = request.Id,
                RequesterEmpId = request.EmpId,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = UserName
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.Culture);
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
                request.ApprovalStatus = 6;
            else if (wfTrans != null)
                hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
    }
}