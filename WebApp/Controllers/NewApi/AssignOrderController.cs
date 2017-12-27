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
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Controllers.Api;
using WebApp.Models;

namespace WebApp.Controllers.NewApi
{
    public class EmpAssignOrderVm
    {
        public int CompanyId { get; set; }
        public string Culture { get; set; }
        public int EmpId { get; set; }
    }
    public class EmpAssignDatesVm
    {
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
    }

    public class SpacificLeaves
    {
        public int CompanyId { get; set; }
        public string Culture { get; set; }
        public int EmpId { get; set; }
    }
    public class DeleteRequestVM
    {
        public int Id { get; set; }
        public string Culture { get; set; }
    }
    public class AssignOrderVM
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int ManagerId { get; set; }
        public byte Duration { get; set; } // 1-Full day  2- Quarter day   3-Half day
        public DateTime AssignDate { get; set; } // Assignment date
        public byte CalcMethod { get; set; } // Calculation Method 1-Monetary 2-Time compensation
        public int? LeaveTypeId { get; set; }
        public DateTime? ExpiryDate { get; set; } // Expiry date
        // Tasks /////////////////////////
        public string TaskDesc { get; set; }
        public int CompanyId { get; set; }
        public string language { get; set; }

        public string AuthPosName { get; set; }

        public string AuthEmpName { get; set; }
    }
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AssignOrderController : BaseApiController
    {
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }
        public AssignOrderController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetEmpAssignOrders")]
        public IHttpActionResult GetEmpAssignOrders(EmpAssignOrderVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var list = hrUnitOfWork.LeaveRepository.ReadAssignOrders(model.CompanyId, model.Culture).Where(ord => ord.EmpId == model.EmpId && ord.ApprovalStatus == 6).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);

        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetMangerAssignOrders")]
        public IHttpActionResult GetMangerAssignOrders(EmpAssignOrderVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var list = hrUnitOfWork.LeaveRepository.ReadAssignOrders(model.CompanyId, model.Culture).Where(ord => ord.ManagerId == model.EmpId).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);

        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetEmployeeForManger")]
        public IHttpActionResult GetEmployeeForManger(EmpAssignOrderVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var emps = hrUnitOfWork.PeopleRepository.GetEmployeeManagedByManagerId(model.CompanyId, model.Culture, model.EmpId);
            if (emps == null)
            {
                return NotFound();

            }
            return Ok(emps);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetSpacificLeaveTypes")]
        public IHttpActionResult GetSpacificLeaveTypes(SpacificLeaves model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var emps = hrUnitOfWork.LeaveRepository.GetSpacificLeaveTypes(model.CompanyId, model.Culture, model.EmpId);
            if (emps == null)
            {
                return NotFound();

            }
            return Ok(emps);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/PostAssignOrder")]
        public IHttpActionResult PostAssignOrder(AssignOrderVM model)
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
            AssignOrder request;
            //= hrUnitOfWork.LeaveRepository.GetAssignOrderByiD(model.Id);

            request = new AssignOrder();
            AutoMapperParm parms = new Models.AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "AssignOrders", Options = null, Transtype = TransType.Insert };
            AutoMapper(parms);

            request.CompanyId = model.CompanyId;
            request.CreatedUser = UserName;
            request.CreatedTime = DateTime.Now;
            request.ApprovalStatus = 6;
            request.EmpId = model.EmpId;
            request.Duration = model.Duration;
            request.AssignDate = model.AssignDate;
            request.CalcMethod = model.CalcMethod;
            request.LeaveTypeId = model.LeaveTypeId;
            request.ExpiryDate = model.ExpiryDate;
            request.TaskDesc = model.TaskDesc;
            request.ManagerId = model.ManagerId;
            hrUnitOfWork.LeaveRepository.AddAssignOrder(request);

            var Errors = SaveChanges(model.language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            if (request.CalcMethod == 1) //monetary
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "AssignOrder1",
                    SourceId = request.CompanyId,
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 6,
                    CreatedUser = UserName,
                };
                var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.language);
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;

                    hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                    hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    hrUnitOfWork.LeaveRepository.Add(wfTrans);
            
            }
            else if (request.CalcMethod == 2) //time compensation
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "AssignOrder2",
                    SourceId = request.CompanyId,
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 6,
                    CreatedUser = UserName,
                };
                var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.language);
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;
                    hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                    hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                }
                else if (wfTrans != null)
                    hrUnitOfWork.LeaveRepository.Add(wfTrans);
            }
            Errors = SaveChanges(model.language);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            model.Id = request.Id;
            return Created(new Uri(Request.RequestUri + "/" + model.Id), model);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/DeleteAssignOrder")]
        public IHttpActionResult DeleteAssignOrder(DeleteRequestVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model == null)
            {
                return NotFound();
            }
            hrUnitOfWork.LeaveRepository.DeleteAssignOrder(model.Id, model.Culture);
            var Errors = SaveChanges(model.Culture);
            if (Errors.Count > 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            return Ok(Errors);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/EditAssignOrder")]
        public IHttpActionResult EditAssignOrder(AssignOrderVM model)
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
            AssignOrder request = hrUnitOfWork.LeaveRepository.GetAssignOrderByiD(model.Id);

            AutoMapperParm parms = new Models.AutoMapperParm() { Source = model, Destination = request, Version = 0, ObjectName = "AssignOrders", Options = null, Transtype = TransType.Update };
            AutoMapper(parms);

            if (model.Id != 0)
            {

                hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = model.CompanyId,
                    ObjectName = "AssignOrders",
                    SourceId = request.Id.ToString(),
                    UserName = UserName,
                    Version = 0,
                    ValueAfter = MsgUtils.Instance.Trls("Submit"),
                    ValueBefore = MsgUtils.Instance.Trls("Darft")
                });

                request.CompanyId = model.CompanyId;
                request.ModifiedUser = UserName;
                request.ModifiedTime = DateTime.Now;
                request.ApprovalStatus = 6;
                request.EmpId = model.EmpId;
                request.Duration = model.Duration;
                request.AssignDate = model.AssignDate;
                request.CalcMethod = model.CalcMethod;
                request.LeaveTypeId = model.LeaveTypeId;
                request.ExpiryDate = model.ExpiryDate;
                request.TaskDesc = model.TaskDesc;
                request.ManagerId = model.ManagerId;
                hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;

                var Errors = SaveChanges(model.language);
                if (Errors.Count > 0)
                {
                    return StatusCode(HttpStatusCode.Forbidden);
                }

                if (request.CalcMethod == 1) //monetary
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "AssignOrder1",
                        SourceId = request.CompanyId,
                        DocumentId = request.Id,
                        RequesterEmpId = request.EmpId,
                        ApprovalStatus = 6,
                        CreatedUser = UserName,
                    };
                    var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        request.ApprovalStatus = 1;

                        hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                        hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                    }
                    else if (wfTrans != null)
                        hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }
                else if (request.CalcMethod == 2) //time compensation
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "AssignOrder2",
                        SourceId = request.CompanyId,
                        DocumentId = request.Id,
                        RequesterEmpId = request.EmpId,
                        ApprovalStatus = 6,
                        CreatedUser = UserName,
                    };
                    var wfTrans = hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, model.language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        request.ApprovalStatus = 1;
                        hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                        hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                    }
                    else if (wfTrans != null)
                        hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }
                Errors = SaveChanges(model.language);
                if (Errors.Count > 0)
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }
            return Ok(model);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetEmpAssignDates")]
        public IHttpActionResult GetEmpAssignDates(EmpAssignDatesVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var dates = hrUnitOfWork.LeaveRepository.GetEmpAssignDates(model.CompanyId,model.EmpId);
            if (dates == null)
            {
                return NotFound();

            }
            return Ok(dates);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/AssignOrder/GetLastEmpCalcsMethod")]
        public IHttpActionResult GetLastEmpCalcsMethod(EmpAssignDatesVm model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var dates = hrUnitOfWork.LeaveRepository.GetLastEmpCalcsMethod(model.CompanyId, model.EmpId);
            return Ok(dates);
        }

    }
}