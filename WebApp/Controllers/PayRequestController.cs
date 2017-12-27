using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Payroll;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class PayRequestController : BaseController
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
        public PayRequestController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Pay Request
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetPayRequests(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadPayRequestsGrid(CompanyId, Language);
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

        public ActionResult GetEmployees()
        {
            return Json(_hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, User.Identity.GetCulture()), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDeptsEmployees(int requestId, string emps, string depts)
        {
            if (requestId == 0 && String.IsNullOrEmpty(emps) && String.IsNullOrEmpty(depts))
                return Json(new DataSource<PayRequestEmpsViewModel>(), JsonRequestBehavior.AllowGet);

            return Json(_hrUnitOfWork.PayrollRepository.GetDeptsEmp(requestId, emps, depts, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id = 0, byte Version = 0)
        {
            List<string> columns = _hrUnitOfWork.PayrollRepository.GetAutoCompleteColumns("PayRequest", CompanyId, Version);
            if (columns.Where(fc => fc == "Requester").FirstOrDefault() == null)
                ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);

            ViewBag.Payroll = _hrUnitOfWork.PayrollRepository.GetPayrollList(CompanyId, Language);
            ViewBag.Formula = _hrUnitOfWork.PayrollRepository.GetFormulaList(CompanyId, Language);
            ViewBag.SalaryItems = _hrUnitOfWork.PayrollRepository.GetSalaryItemList(CompanyId, Language);

            ViewBag.BankId = _hrUnitOfWork.PayrollRepository.GetBankList().Select(a => new { value = a.id, text = a.name });

            if (id == 0)
            {
                int requestNo = _hrUnitOfWork.PayrollRepository.NextRequestNo(CompanyId);
                return View(new PayRequestViewModel() { ApprovalStatus = 1, PayPercent = 100, RequestNo = requestNo});
            }
            PayRequestViewModel request = _hrUnitOfWork.PayrollRepository.GetPayRequestVM(id, Language);
            return request == null ? (ActionResult)HttpNotFound() : View(request);
        }

        [HttpPost]
        public ActionResult Details(PayRequestViewModel model, IEnumerable<PayRequestEmpsViewModel> grid1, OptionsViewModel moreInfo, byte version)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PayrollRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayRequest",
                        TableName = "PayRequests",
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

            PayRequest payRequest = _hrUnitOfWork.PayrollRepository.GetRequest(model.Id);
            if (model.Id == 0)
            { //Add
                payRequest = new PayRequest();
                AutoMapper(new Models.AutoMapperParm { Source = model, Destination = payRequest, Version = version, ObjectName = "PayRequest", Options = moreInfo ,Transtype=TransType.Insert});
                payRequest.ApprovalStatus = (byte)(model.submit ? 2 : model.ApprovalStatus); //1. New, 2. Submit
                payRequest.PayPercent /= 100;
                payRequest.CreatedTime = DateTime.Now;
                payRequest.CreatedUser = UserName;
                _hrUnitOfWork.PayrollRepository.Add(payRequest);
            }
            else
            { //Edit
                AutoMapper(new Models.AutoMapperParm { Source = model, Destination = payRequest, Version = version, ObjectName = "PayRequest", Options = moreInfo ,Transtype=TransType.Update });
                payRequest.ApprovalStatus = (byte)(model.submit ? 2 : model.ApprovalStatus); //1. New, 2. Submit
                payRequest.PayPercent /= 100;
                payRequest.ModifiedTime = DateTime.Now;
                payRequest.ModifiedUser = UserName;

                _hrUnitOfWork.PayrollRepository.Attach(payRequest);
                _hrUnitOfWork.PayrollRepository.Entry(payRequest).State = EntityState.Modified;
            }

            errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), payRequest);
            if (errors.Count > 0) return Json(errors.First().errors.First().message);

            string message = "OK";
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            if (model.submit)
            {
                var wfErrors = AddWFTrans(payRequest, null, null);
                if (!String.IsNullOrEmpty(wfErrors))
                {
                    payRequest.ApprovalStatus = 1;
                    message += "," + (new JavaScriptSerializer()).Serialize(new { model = payRequest, error = wfErrors });

                    _hrUnitOfWork.PayrollRepository.Attach(payRequest);
                    _hrUnitOfWork.PayrollRepository.Entry(payRequest).State = EntityState.Modified;
                }
            }

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }

        private List<Error> SaveGrid1(IEnumerable<PayRequestEmpsViewModel> grid1, IEnumerable<KeyValuePair<string, ModelState>> state, PayRequest payRequest)
        {
            List<Error> errors = new List<Error>();
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayRequestEmployees",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            var allRequestDets = _hrUnitOfWork.Repository<PayRequestDet>().Where(p => p.RequestId == payRequest.Id);
            var deleted = allRequestDets.Where(p => grid1.Where(i => i.EmpId == p.EmpId).Count() == 0).ToList();
            
            foreach (PayRequestEmpsViewModel model in grid1)
            {
                PayRequestDet PayDet;

                if (model.Id == 0) // inserted records
                {
                    PayDet = new PayRequestDet();
                    AutoMapper(new Models.AutoMapperParm { Destination = PayDet, Source = model ,Transtype =TransType.Insert });
                    PayDet.Request = payRequest;
                    _hrUnitOfWork.PayrollRepository.Add(PayDet);
                }
                else if (model.dirty) // updated records
                {
                    PayDet = allRequestDets.Where(e => e.EmpId == model.EmpId).FirstOrDefault();
                    AutoMapper(new Models.AutoMapperParm { Destination = PayDet, Source = model ,Transtype=TransType.Update });
                    _hrUnitOfWork.PayrollRepository.Attach(PayDet);
                    _hrUnitOfWork.PayrollRepository.Entry(PayDet).State = EntityState.Modified;
                }
            }

            if(deleted.Count > 0)
            {
                foreach (PayRequestDet model in deleted)
                {
                    _hrUnitOfWork.PayrollRepository.Remove(model);
                }
            }

            return errors;
        }

        public ActionResult DeletePay(int id)
        {
            List<Error> errors = new List<Error>();

            string message = "OK";
            PayRequest request = _hrUnitOfWork.Repository<PayRequest>().Where(r => r.Id == id).FirstOrDefault();

            if (request.ApprovalStatus == 1)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = request,
                    ObjectName = "PayRequests",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PayrollRepository.Remove(request);
            }

            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }

        #endregion

        #region Follow Up
        public ActionResult FollowUpIndex()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("PayCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(CompanyId, Language).Select(m => new { id = m.Id, name = m.Name });

            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            return View();
        }

        public ActionResult GetPayFollowUp(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadPayFollowUpsGrid(CompanyId, Language);
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

        [HttpPost]
        public ActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            PayRequest request = _hrUnitOfWork.PayrollRepository.GetRequest(Id);
            string error = "";
            if (Send == 1)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "PayRequest",
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
                    ObjectName = "PayRequest",
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

            _hrUnitOfWork.PayrollRepository.Attach(request);
            _hrUnitOfWork.PayrollRepository.Entry(request).State = EntityState.Modified;

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
            PayRequest request = _hrUnitOfWork.PayrollRepository.GetRequest(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "PayRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("PayCancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PayCancelReason", Language).Where(a => a.CodeId == request.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "PayRequest",
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
                ObjectName = "PayRequest",
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

            _hrUnitOfWork.PayrollRepository.Attach(request);
            _hrUnitOfWork.PayrollRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Json("Ok");
        }


        [HttpGet]
        public ActionResult FollowUpDetails(int id = 0)
        {
            PayRequestViewModel request = _hrUnitOfWork.PayrollRepository.GetPayRequestVM(id, Language);
            ViewBag.Payroll = _hrUnitOfWork.PayrollRepository.GetPayrollList(CompanyId, Language);
            ViewBag.Formula = _hrUnitOfWork.PayrollRepository.GetFormulaList(CompanyId, Language);
            ViewBag.SalaryItems = _hrUnitOfWork.PayrollRepository.GetSalaryItemList(CompanyId, Language);

            ViewBag.BankId = _hrUnitOfWork.PayrollRepository.GetBankList().Select(a => new { value = a.id, text = a.name });

            return View(request);
        }

        [HttpPost]
        public ActionResult FollowUpDetails(PayRequestViewModel model, OptionsViewModel moreInfo, byte version)
        {
            List<Error> Errors = new List<Error>();
            PayRequest request = _hrUnitOfWork.PayrollRepository.GetRequest(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Errors = _hrUnitOfWork.PayrollRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayFollowUpDetails",
                        TableName = "PayRequests",
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
                ObjectName = "PayRequest",
                Version = Convert.ToByte(Request.Form["Version"]),
                Options = moreInfo,
                Transtype = TransType.Update
            });

            if (request.ApprovalStatus != 9)
            {
                request.RejectDesc = null;
                request.RejectReason = null;
            }
            request.PayPercent /= 100;
            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = UserName;

            if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
            {
                string msg = AddWFTrans(request, null, null);
                if (msg.Length > 0)
                    return Json(msg);
            }

            _hrUnitOfWork.PayrollRepository.Attach(request);
            _hrUnitOfWork.PayrollRepository.Entry(request).State = EntityState.Modified;

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("OK");
        }

        #endregion

        #region Approved Pays
        public ActionResult ApprovedPaysIndex()
        {
            ViewBag.BankId = _hrUnitOfWork.PayrollRepository.GetBankList().Select(a => new { value = a.id, text = a.name });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult ApprovedPays(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadApprovedPaysGrid(CompanyId, Language);
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

        public ActionResult UpdatePaid(IEnumerable<PayReqGridViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PayReqGridViewModel>();
            var result = new List<PayRequest>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PayrollRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ApprovedPays",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var payRequests = _hrUnitOfWork.Repository<PayRequest>().Where(r => models.Where(m => m.Id == r.Id).FirstOrDefault() != null).Select(r => r).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    PayRequest pay = payRequests.Where(r => r.Id == models.ElementAtOrDefault(i).Id).FirstOrDefault();

                    AutoMapper(new AutoMapperParm() { ObjectName = "PayRequest", Destination = pay, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update });
                    //Update
                    pay.PayPercent /= 100;
                    pay.ModifiedUser = UserName;
                    pay.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.PayrollRepository.Attach(pay);
                    _hrUnitOfWork.PayrollRepository.Entry(pay).State = EntityState.Modified;
                }
                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }
            datasource.Data = models.Where(m => !m.Paid);

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
    
        public ActionResult ApprovedPaysEmps(int MenuId, int RequestId)
        {
            var query = _hrUnitOfWork.PayrollRepository.GetDeptsEmp(RequestId, null, null, CompanyId, Language);
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
        #endregion

        private string AddWFTrans(PayRequest request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Pay",
                SourceId = CompanyId,
                DocumentId = request.Id,
                RequesterEmpId = request.Requester,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = UserName,
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.PayrollRepository.AddWorkFlow(wf, Language);
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
                request.ApprovalStatus = 6;
            else
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
    }
}