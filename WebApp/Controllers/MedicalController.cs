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
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MedicalController : BaseController
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
        public MedicalController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Medical Request
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetMedicalRequest(int MenuId)
        {
            string culture = Language;

            var query = _hrUnitOfWork.MedicalRepository.GetMedicalRequest(CompanyId, Language);
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
        public ActionResult Details(int id = 0, byte Version = 0)
        {
            var Medical = _hrUnitOfWork.MedicalRepository.ReadMedical(id);
            ViewBag.BenefitId = _hrUnitOfWork.MedicalRepository.FillDDLBenefit(User.Identity.GetLanguage());
            if (User.Identity.IsSelfServiceUser())
            {
                int EmpId = User.Identity.GetEmpId();
                ViewBag.BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == EmpId).Select(p => new { id = p.Id, name = p.Name }).ToList();
                //  ViewBag.Services = _hrUnitOfWork.MedicalRepository.GetAllservice(EmpId,0);
            }
            if (Medical != null)
            {
                ViewBag.BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == Medical.EmpId).Select(p => new { id = p.Id, name = p.Name }).ToList();
        
                ViewBag.Services = _hrUnitOfWork.MedicalRepository.GetAllservice(Medical.EmpId, Medical.BenefitId, Medical.BeneficiaryId);
            }
            ViewBag.Providers = _hrUnitOfWork.MedicalRepository.GetAllProvider();
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            string culture = Language;
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("MedicalRequestsForm", CompanyId, Version);
            if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, Medical != null ? Medical.EmpId : 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).ToList();
            if (id == 0)
                return View(new MedicalRequestViewModel());

            return Medical == null ? (ActionResult)HttpNotFound() : View(Medical);
        }
        public ActionResult GetBeneficiary(int EmpId, short BenefitClass)
        {
            var Beneficiary = _hrUnitOfWork.MedicalRepository.GetBeneficiary(EmpId);
            var Service = _hrUnitOfWork.MedicalRepository.GetAllservice(EmpId, BenefitClass, 0).ToList();
            return Json(new { Ben = Beneficiary, serv = Service }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBeneficiaryService(int BeneficiaryId, int EmpId, short BenefitClass)
        {
            var loginEmployee = User.Identity.GetEmpId();
            var EmployeeId = EmpId == 0 ? loginEmployee : EmpId;
            var Service = _hrUnitOfWork.MedicalRepository.GetAllservice(EmployeeId, BenefitClass, BeneficiaryId);
            return Json(Service, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetCurrency(int ServiceId)
        {
            var query = _hrUnitOfWork.MedicalRepository.SetCurrency(ServiceId);
            var CompCost = query.Cost * (decimal)query.CompPercent / 100;
            var EmpCost = query.Cost * (decimal)query.EmpPercent / 100;

            return Json(new { EmployeeCost = EmpCost, CompanyCost = CompCost, Currency = query }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveMedical(MedicalRequestViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MedicalRequestsForm",
                        TableName = "MedicalRequests",
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
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(model.Id);
            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";
            var benefitplanId = _hrUnitOfWork.MedicalRepository.GetBenefitPlanId(model.ParentId, model.EmpId, model.BeneficiaryId);

            ///Save
            if (model.Id == 0)
            { /// New
                request = new BenefitRequest();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "MedicalRequestsForm", Options = moreInfo, Transtype = TransType.Insert };
                AutoMapper(parms);
                request.BenefitPlanId = benefitplanId;
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.CompanyId = CompanyId;
                _hrUnitOfWork.MedicalRepository.Add(request);
            }
            else if (model.ApprovalStatus == 3 || model.ApprovalStatus == 1)
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "MedicalRequestsForm", Options = moreInfo, Transtype = TransType.Update };
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
                request.BenefitPlanId = benefitplanId == 0 ? model.BenefitPlanId : benefitplanId;
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus); //1- New, 2- Submit
                request.ModifiedUser = UserName;
                request.ModifiedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                _hrUnitOfWork.MedicalRepository.Attach(request);
                _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            if (model.submit)
            {
                var chkWorkFlow = _hrUnitOfWork.Repository<Workflow>().Where(w => w.Source == "Medical").Select(a => a.IsRequired).FirstOrDefault();
                if (chkWorkFlow == true)
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "Medical",
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
                        _hrUnitOfWork.MedicalRepository.Attach(request);
                        _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
                        message = wf.WorkFlowStatus;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }
            }
            else
            {
                 request.ApprovalStatus = 1;
                _hrUnitOfWork.MedicalRepository.Attach(request);
                _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
            }

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult DeleteMedical(int id)
        {
            var message = "OK";
            List<Error> errors = new List<Error>();
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(id);
            if (request.ApprovalStatus == 1)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = request,
                    ObjectName = "MedRequest",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.MedicalRepository.Remove(request);
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

        #region Medical Follow Up
        public ActionResult FollowUpIndex()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(CompanyId, Language).Select(m => new { id = m.Id, name = m.Name });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        //  Read Medical Follow Up
        public ActionResult ReadMedicalFollowUp(int MenuId)
        {
            //2-submit
            var query = _hrUnitOfWork.MedicalRepository.GetMedicalReqFollowUp(CompanyId, Language);
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
        public ActionResult FollowupDetails(int Id=0)
        {
            ViewBag.BenefitId = _hrUnitOfWork.MedicalRepository.FillDDLBenefit(User.Identity.GetLanguage());
            ViewBag.Beneficiary = _hrUnitOfWork.MedicalRepository.GetAllBeneficiary();
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.Providers = _hrUnitOfWork.MedicalRepository.GetAllProvider();
            ViewBag.RejectReason = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedRejectReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            BenefitRequestFollowUp request = _hrUnitOfWork.MedicalRepository.GetRequest(Id, Language);
            if (request == null)
                request = new BenefitRequestFollowUp();
            int BenficiaryId = request.BeneficiaryId == null ? 0 : request.BeneficiaryId.Value;
            ViewBag.Services = _hrUnitOfWork.MedicalRepository.GetAllservice(request.EmpId, request.BenefitId, BenficiaryId);
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, request.EmpId, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).ToList();
            return View(request);
        }
        //Send to function
        public ActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(Id);
            string error = "";
            if (Send == 1)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "MedicalReqFollowUpForm",
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
                    ObjectName = "MedicalReqFollowUpForm",
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

            _hrUnitOfWork.MedicalRepository.Attach(request);
            _hrUnitOfWork.MedicalRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }
            return Json("Ok");
        }
        private string AddWFTrans(BenefitRequest request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Medical",
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
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
            {
                request.ApprovalStatus = 6;
            }
            else
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }

        //Cancel Request
        public ActionResult CancelReq(int Id, short? Reason, string Desc)
        {
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "MedicalReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedCancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MedCancelReason", Language).Where(a => a.CodeId == request.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "MedicalReqFollowUpForm",
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
                ObjectName = "MedicalReqFollowUpForm",
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

            _hrUnitOfWork.MedicalRepository.Attach(request);
            _hrUnitOfWork.MedicalRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Json("Ok");
        }

        public ActionResult SaveFollowupDetails(BenefitRequestFollowUp model, OptionsViewModel moreInfo)
        {
            List<Error> Errors = new List<Error>();
            int periodId = 0;
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MedicalReqFollowUpForm",
                        TableName = "MedicalRequests",
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

            if (model.IssueDate != null)
            {
                var period = _hrUnitOfWork.MedicalRepository.GetMedicalPeriodId(request.BenefitPlanId, (DateTime)model.IssueDate);
                //if (period != 0)
                //{
                periodId = period;
                //}
                //else
                //{
                //    ModelState.AddModelError("IssueDate", MsgUtils.Instance.Trls("Periodnotdefined"));
                //    return Json(Models.Utils.ParseFormErrors(ModelState));
                //}
            }
            AutoMapper(new Models.AutoMapperParm
            {
                Destination = request,
                Source = model,
                ObjectName = "MedicalReqFollowUpForm",
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
            if (periodId != 0)
            {
                request.SubPeriodId = periodId;
            }

            if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
            {
                string msg = AddWFTrans(request, null, null);
                if (msg.Length > 0)
                    return Json(msg);
            }
            _hrUnitOfWork.MedicalRepository.Attach(request);
            _hrUnitOfWork.MedicalRepository.Entry(request).State = EntityState.Modified;

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("OK");
        }

        #endregion

        #region Service Settelement
        public ActionResult ServiceSettlement()
        {
            return View();
        }
        #endregion

        #region Benefit
        public ActionResult AcceptedMedical()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadAcceptedMedical(int MenuId)
        {
            //6-accepted
            var query = _hrUnitOfWork.MedicalRepository.GetApprovedMedicalReq(CompanyId, Language);
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

        public ActionResult AcceptedMedicalDetails(int Id = 0)
        {
            ViewBag.BenefitId = _hrUnitOfWork.MedicalRepository.FillDDLBenefit(User.Identity.GetLanguage());
            ViewBag.Beneficiary = _hrUnitOfWork.MedicalRepository.GetAllBeneficiary();
            ViewBag.Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.Providers = _hrUnitOfWork.MedicalRepository.GetAllProvider();
            ViewBag.RejectReason = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedRejectReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);
            ViewBag.CancelReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            if (Id == 0)
            {
                return View(new BenefitRequestFollowUp());
            }
            BenefitRequestFollowUp request = _hrUnitOfWork.MedicalRepository.GetRequest(Id, Language);
            int BenficiaryId = request.BeneficiaryId == null ? 0 : request.BeneficiaryId.Value;
            ViewBag.Services = _hrUnitOfWork.MedicalRepository.GetAllservice(request.EmpId, request.BenefitId, BenficiaryId);
            return View(request);
        }
        public ActionResult CancelAccepted(int Id, short? Reason, string Desc)
        {
            string message = "Ok";
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "MedicalReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedCancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MedCancelReason", Language).Where(a => a.CodeId == request.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "MedicalReqFollowUpForm",
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
                ObjectName = "MedicalReqFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel after approved"),
                ValueBefore = MsgUtils.Instance.Trls("Accepted")
            });
            request.CancelDesc = Desc;
            request.CancelReason = Reason;
            request.ApprovalStatus = 8;

            _hrUnitOfWork.MedicalRepository.Attach(request);
            _hrUnitOfWork.MedicalRepository.Entry(request).State = EntityState.Modified;
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("Ok");
        }
        public ActionResult SaveBenefitSettlement(BenefitRequestFollowUp model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "acceptedMedical",
                        TableName = "BenefitRequests",
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
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(model.Id);
            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";
            var benefitplanId = _hrUnitOfWork.MedicalRepository.GetBenefitPlanId(model.ParentId, model.EmpId, model.BeneficiaryId);

            if (request == null)
            { /// New
                request = new BenefitRequest();
                AutoMapperParm parms = new AutoMapperParm()
                {
                    Source = model,
                    Destination = request,
                    Version = version,
                    ObjectName = "acceptedMedical",
                    Options = moreInfo,
                    Transtype = TransType.Insert
                };
                AutoMapper(parms);
                request.ApprovalStatus = 6;
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.BenefitPlanId = benefitplanId;
                request.CompanyId = CompanyId;
                if (model.IssueDate != null)
                {
                    var Subperiod = _hrUnitOfWork.MedicalRepository.GetMedicalPeriodId(benefitplanId, (DateTime)model.IssueDate);
                    request.SubPeriodId = Subperiod;
                }
                _hrUnitOfWork.MedicalRepository.Add(request);
            }
            else if (request != null)
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm()
                {
                    Source = model,
                    Destination = request,
                    Version = version,
                    ObjectName = "MedicalRequestsForm",
                    Options = moreInfo,
                    Transtype = TransType.Update
                };
                AutoMapper(parms);
                request.ApprovalStatus = model.ApprovalStatus;
                request.ModifiedUser = UserName;
                request.ModifiedTime = DateTime.Now;
                request.RequestDate = model.RequestDate;
                if (request.SubPeriodId == null)
                {
                    var coverageAmount = _hrUnitOfWork.Repository<BenefitPlan>().Where(a => a.Id == request.BenefitPlanId).Select(a => a.CoverAmount).FirstOrDefault();
                    var coverAmount = coverageAmount == null ? 0 : coverageAmount;

                }
                else
                {

                }
                _hrUnitOfWork.MedicalRepository.Attach(request);
                _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json(message);
        }
        public ActionResult SetCost(decimal ServiceCost, int ParentId, DateTime issueDate, int EmpId, int BeneficiaryId)
        {
            int benefitplanId = _hrUnitOfWork.MedicalRepository.GetBenefitPlanId(ParentId, EmpId, BeneficiaryId);
            int SubperiodId = _hrUnitOfWork.MedicalRepository.GetMedicalPeriodId(benefitplanId, issueDate);
            BenefitPlan benefitPlanObj = _hrUnitOfWork.Repository<BenefitPlan>().Where(a => a.Id == benefitplanId).FirstOrDefault();
            decimal? Dragged = 0;
            if (benefitPlanObj.CompAmount != 0 && benefitPlanObj.EmpAmount != 0)
            {
                
                if (SubperiodId > 0)                
                    Dragged = _hrUnitOfWork.Repository<BenefitRequest>().Where(a => a.SubPeriodId == SubperiodId && a.EmpId == EmpId && a.BeneficiaryId == BeneficiaryId && a.BenefitId ==benefitPlanObj.BenefitId).Sum(a => a.CompanyCost);
                else
                    Dragged = _hrUnitOfWork.Repository<BenefitRequest>().Where(a => a.SubPeriodId == null && a.EmpId == EmpId && a.BeneficiaryId == BeneficiaryId && a.BenefitId == benefitPlanObj.BenefitId).Sum(a => a.CompanyCost);

                var credit = (decimal)benefitPlanObj.CoverAmount - Dragged;

                if (credit > benefitPlanObj.CompAmount)
                    return Json(new { EmployeeCost = ServiceCost - benefitPlanObj.CompAmount < 0 ? 0 : ServiceCost - benefitPlanObj.CompAmount, CompanyCost = benefitPlanObj.CompAmount  > ServiceCost ? ServiceCost :benefitPlanObj.CompAmount }, JsonRequestBehavior.AllowGet);

                else if (credit < benefitPlanObj.CompAmount)
                    return Json(new { EmployeeCost = ServiceCost - credit, CompanyCost = credit }, JsonRequestBehavior.AllowGet);
            }
            else if (benefitPlanObj.CompPercent != 0 && benefitPlanObj.EmpPercent != 0)
            {
                if (SubperiodId > 0)                
                    Dragged = _hrUnitOfWork.Repository<BenefitRequest>().Where(a => a.SubPeriodId == SubperiodId && a.EmpId == EmpId && a.BeneficiaryId == BeneficiaryId && a.BenefitId == benefitPlanObj.BenefitId).Sum(a => a.CompanyCost);               
                else               
                    Dragged = _hrUnitOfWork.Repository<BenefitRequest>().Where(a => a.SubPeriodId == null && a.EmpId == EmpId && a.BeneficiaryId == BeneficiaryId && a.BenefitId == benefitPlanObj.BenefitId).Sum(a => a.CompanyCost);              
                var CompCost = benefitPlanObj.CoverAmount * (decimal)benefitPlanObj.CompPercent / 100;
                var EmpCost = benefitPlanObj.CoverAmount * (decimal)benefitPlanObj.EmpPercent / 100;

                var credit = (decimal)benefitPlanObj.CoverAmount - Dragged;
                if (credit < CompCost)
                    return Json(new { EmployeeCost = ServiceCost - credit, CompanyCost = credit }, JsonRequestBehavior.AllowGet);

                else if (credit > CompCost)
                    return Json(new { EmployeeCost = ServiceCost - CompCost < 0 ? 0 : ServiceCost - CompCost, CompanyCost = CompCost > ServiceCost ? ServiceCost : CompCost }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}