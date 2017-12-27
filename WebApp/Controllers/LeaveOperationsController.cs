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
    public class LeaveOperationsController : BaseController
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
        public LeaveOperationsController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: LeaveOperations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadLeaveGrid(int MenuId, int pageSize, int skip)
        {
            //&& (a.isStarted && a.ActualNofDays > 1 || !a.isStarted )
            var query = _hrUnitOfWork.LeaveRepository.GetApprovedLeaveReq(CompanyId, Language).Where(a => a.ActualEndDate >= DateTime.Today).AsQueryable();
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);

            query = (IQueryable<LeaveReqGridViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);

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

        [HttpGet]
        public ActionResult Details(int Id, byte Btn)
        {
            //Btn: 1.Edit, 2.Cancel, 3.Break
            LeaveOpViewModel request = _hrUnitOfWork.LeaveRepository.GetLeaveOpReq(Id, Language);
            request.btn = Btn;

            ViewBag.CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); //for Calender
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId); //for Calender

            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            ViewBag.isSSMenu = _hrUnitOfWork.MenuRepository.Get(MenuId)?.SSMenu ?? false;

            return View(request);
        }

        [HttpPost]
        public ActionResult Details(LeaveOpViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveRequestForm",
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

            //Btn: 1.Edit, 2.Cancel, 3.Break
            string field = null;
            if (model.btn == 1 && model.EditedStartDate == null) field = "EditedStartDate";
            else if (model.btn == 2 && model.CancelReason == null) field = "CancelReason";
            else if (model.btn == 3 && model.BreakReturnDate == null) field = "BreakReturnDate";

            if (field != null)
            {
                ModelState.AddModelError(field, MsgUtils.Instance.Trls("Required"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);

            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";

            AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "LeaveRequest", Options = moreInfo, Transtype = TransType.Update };
            AutoMapper(parms);

            ///Btn: 1.Edit, 2.Cancel, 3.Break
            if (model.btn == 1)  //Edit
            {
                AddTrail(request.Id, "ActualStartDate", model.EditedStartDate.ToString(), request.ActualStartDate.ToString());
                AddTrail(request.Id, "ActualEndDate", model.EditedEndDate.ToString(), request.ActualEndDate.ToString());

                DateTime oldStartDate = request.ActualStartDate.GetValueOrDefault();
                request.ActualStartDate = model.EditedStartDate;
                request.ActualEndDate = model.EditedEndDate;

                _hrUnitOfWork.LeaveRepository.AddEditLeaveTrans(request, oldStartDate);

            }
            else if(model.btn == 2) //Cancel
            {
                AddTrail(request.Id, "ApprovalStatus", MsgUtils.Instance.Trls("Cancel after accepted"), MsgUtils.Instance.Trls("Approved"));
                request.ApprovalStatus = 8;

                ///Cancel change assign state
                _hrUnitOfWork.EmployeeRepository.CancelLeaveAssignState(request, UserName, version, Language); 
                ///Cancel LeaveTrans
                var msg = _hrUnitOfWork.LeaveRepository.AddCancelLeaveTrans(request, UserName, Language); 
                if (msg.Length > 0) return Json(msg);
            }
            else if(model.btn == 3)  //Break
            {
                AddTrail(request.Id, "ActualEndDate", model.BreakEndDate.ToString(), request.ActualEndDate.ToString());
                AddTrail(request.Id, "ActualNofDays", model.BreakNofDays.ToString(), request.ActualNofDays.ToString());
                float DiffDays = request.ActualNofDays.Value - model.BreakNofDays.Value;

                request.ActualEndDate = model.BreakEndDate;
                request.ActualNofDays = model.BreakNofDays;
                request.ReturnDate = model.BreakReturnDate;

                ///Brake Leave LeaveTrans
                _hrUnitOfWork.LeaveRepository.AddBreakLeaveTrans(request, DiffDays, UserName);
            }
            request.ModifiedUser = UserName;
            request.ModifiedTime = DateTime.Now;
            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }

        public ActionResult EditStars(int RequestId, DateTime EndDate)
        {
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(RequestId);
            LeaveType type = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId);

            GetStarsParamVM param = new GetStarsParamVM { EmpId = request.EmpId, StartDate = request.ActualStartDate ?? request.StartDate, EndDate = EndDate, RequestId = RequestId, ExDayOff = type.ExDayOff, ExHolidays = type.ExHolidays, PeriodId = request.PeriodId, ComapnyId = CompanyId };
            var result = _hrUnitOfWork.LeaveRepository.GetStars(param);

            return Json(result.Stars, JsonRequestBehavior.AllowGet);
        }

        private void AddTrail(int SourceId, string columnName, string ValAfter, string valBefore)
        {
            _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel
            {
                ObjectName = "LeaveRequest",
                CompanyId = CompanyId,
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["version"]),
                ColumnName = columnName,
                SourceId = SourceId.ToString(),
                ValueAfter = ValAfter,
                ValueBefore = valBefore
            });

        }

    }
}