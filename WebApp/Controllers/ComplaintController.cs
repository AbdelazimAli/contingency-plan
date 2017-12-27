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
    public class ComplaintController : BaseController
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
        public ComplaintController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: Complaint
        #region Create Compliant
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        //GetComplaintRequest
        public ActionResult GetComplaintRequest(int MenuId)
        {
            var query = _hrUnitOfWork.ComplaintRepository.GetComplaintRequest(CompanyId,Language).AsQueryable();
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id = 0, byte Version = 0,int MenuId=0)
        {
            var Complaint = _hrUnitOfWork.ComplaintRepository.ReadComplaint(id);
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("ComplainRequestsForm", CompanyId, Version);
            if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, Complaint!=null ?Complaint.EmpId:0, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            if (id == 0)
                return View(new ComplaintRequestViewModel());

            return Complaint == null ? (ActionResult)HttpNotFound() : View(Complaint);
        }

        public ActionResult SaveComplaint(ComplaintRequestViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ComplainRequests",
                        TableName = "ComplainRequests",
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

            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(model.Id);          

            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";

            ///Save
            if (model.Id == 0)
            { /// New
                request = new ComplainRequest();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "ComplainRequests", Options = moreInfo ,Transtype=TransType.Insert};
                AutoMapper(parms);
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.CompanyId = CompanyId;
                _hrUnitOfWork.ComplaintRepository.Add(request);
            }
            else if (model.ApprovalStatus == 3 || model.ApprovalStatus == 1)
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "ComplainRequests", Options = moreInfo ,Transtype=TransType.Update };
                AutoMapper(parms);
                if (model.submit)
                    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    {
                        ColumnName = "ApprovalStatus",
                        CompanyId = CompanyId,
                        ObjectName = "ComplainRequests",
                        SourceId = request.Id.ToString(),
                        UserName = UserName,
                        Version = 0,
                        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                        ValueBefore = MsgUtils.Instance.Trls("Darft")
                    });

                request.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus); //1- New, 2- Submit
                request.ModifiedUser = UserName;
                request.ModifiedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.CompanyId = CompanyId;
                _hrUnitOfWork.ComplaintRepository.Attach(request);
                _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            if (model.submit)
            {
                var chkWorkFlow = _hrUnitOfWork.Repository<Workflow>().Where(w => w.Source == "Complaint" + model.Against).Select(a => a.IsRequired).FirstOrDefault();
                if (chkWorkFlow == true)
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "Complaint" + model.Against,
                        SourceId = CompanyId,
                        DocumentId = request.Id,
                        RequesterEmpId = request.EmpId,
                        ApprovalStatus = 2,
                        CreatedUser = UserName,
                    };

                    var wfTrans = _hrUnitOfWork.ComplaintRepository.AddWorkFlow(wf, Language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        request.ApprovalStatus = 1;
                        message = wf.WorkFlowStatus;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }
                else
                {
                    request.ApprovalStatus = 1;
                    _hrUnitOfWork.ComplaintRepository.Attach(request);
                    _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;
                }
            }

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            else
                message += "," + ((new JavaScriptSerializer()).Serialize(request));

            return Json(message);
        }
      
        public ActionResult DeleteComplaint(int id)
        {
            var message = "OK";
            List<Error> errors = new List<Error>();
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(id);
            if (request.ApprovalStatus == 1)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = request,
                    ObjectName = "ComplainRequests",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.ComplaintRepository.Remove(request);
            }
            
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            else
                return Json(message);
        }
        #endregion

        #region Complaint Follow Up
        public ActionResult FollowUpIndex()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CompCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(CompanyId,Language).Select(m => new { id = m.Id, name = m.Name });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadComplaintFollowUp(int MenuId)
        {
            //2-submit
            var query = _hrUnitOfWork.ComplaintRepository.GetComplaintReqFollowUp(CompanyId, Language);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                }
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FollowupDetails(int Id, byte Version)
        {
            ViewBag.RejectReason = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CompRejectReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            //ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);
            ComplaintRequestViewModel request = _hrUnitOfWork.ComplaintRepository.GetRequest(Id, Language);
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language,request.EmpId, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).ToList();
            return View(request);
        }
        public ActionResult SaveFollowupDetails(ComplaintRequestViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> Errors = new List<Error>();
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ComplaintReqFollowUpForm",
                        TableName = "ComplainRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (Errors.Count() > 0)
                    {
                        foreach (var e in Errors)
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
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            AutoMapper(new Models.AutoMapperParm
            {
                Destination = request,
                Source = model,
                ObjectName = "ComplaintReqFollowUpForm",
                Version = Convert.ToByte(Request.Form["Version"]),
                Options = moreInfo,
                Transtype = TransType.Update
            });

            if (request.ApprovalStatus != 9)
            {
                request.RejectDesc = null;
                request.RejectReason = null;
            }

            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = UserName;

            if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
            {
                string msg = AddWFTrans(request, null, null);
                if (msg.Length > 0)
                    return Json(msg);
            }
            _hrUnitOfWork.ComplaintRepository.Attach(request);
            _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("OK");
        }
        private string AddWFTrans(ComplainRequest request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Complaint"+request.Against,
                SourceId = CompanyId,
                DocumentId = request.Id,
                RequesterEmpId = request.EmpId,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = UserName,
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans != null)
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
        public ActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(Id);
            string error = "";
            if (Send == 1)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "ComplaintReqFollowUpForm",
                    SourceId = Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
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
                    CompanyId = CompanyId,
                    ObjectName = "ComplaintReqFollowUpForm",
                    SourceId = Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
                    ValueAfter = MsgUtils.Instance.Trls("ManagerReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });
                request.ApprovalStatus = 4;
                error = AddWFTrans(request, ManagerId, null);
            }
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.ComplaintRepository.Attach(request);
            _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }
            return Json("Ok");
        }
        public ActionResult CancelReq(int Id, short? Reason, string Desc)
        {
            ComplainRequest request = _hrUnitOfWork.ComplaintRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "ComplaintReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CompCancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("CompCancelReason", Language).Where(a => a.CodeId == request.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "ComplaintReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = Desc,
                ValueBefore = request.CancelDesc
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "ApprovalStatus",
                CompanyId = CompanyId,
                ObjectName = "ComplaintReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel before approved"),
                ValueBefore = MsgUtils.Instance.Trls("Submit")
            });
            request.CancelDesc = Desc;
            request.CancelReason = Reason;
            request.ApprovalStatus = 7;

            string error = AddWFTrans(request, null, null);
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.ComplaintRepository.Attach(request);
            _hrUnitOfWork.ComplaintRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Json("Ok");
        }


        #endregion



    }
}