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
    public class LeavePostingController : BaseController
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
        public LeavePostingController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: Leave Posting
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralLeaveTypes(CompanyId, Language);
            ViewBag.Periods = _hrUnitOfWork.LeaveRepository.GetOpenedLeavePeriods();
            return View(new LeavePostingViewModel());
        }

        public ActionResult GetOpenPeriods()
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetOpenedLeavePeriods(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubPeriod(int PeriodId)
        {
            return Json(_hrUnitOfWork.BudgetRepository.GetSubPeriods(PeriodId).Where(s => s.Status == 1).ToList().Select(s => new { id = s.Id, name = s.Name }), JsonRequestBehavior.AllowGet); 
        }

        public ActionResult ReadLeaveBalance(int typeId, int periodId, int subPeriodId)
        {
            IEnumerable<LeaveBalanceGridViewModel> grids;
            var result = _hrUnitOfWork.LeaveRepository.CalcLeaveBalance(out grids, typeId, periodId, subPeriodId, CompanyId, Language);
            if (result != "Success")
                return Json(result, JsonRequestBehavior.AllowGet);

            return Json(grids, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult postLeave(IEnumerable<LeaveBalanceGridViewModel> models, bool closePeriod)
        {
            var datasource = new DataSource<LeaveBalanceGridViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveBalance",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                // bool postAll = true; // models.Where(m => m.selected).Count() == 0; //Not selected -- post all grid

                AddTrans(models, ref closePeriod);
                UpdatePeriod(models.FirstOrDefault(), closePeriod);

                datasource.Errors = SaveChanges(Language);
            }
            else
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
            {
                //if (datasource.Data.Where(d => d.selected).Count() > 0)
                //    return Json(datasource.Data.Where(d => !d.selected));
                //else
                return Json(datasource.Data); //.Where(d => d.PostAction == 3)
            }
        }

        private void AddTrans(IEnumerable<LeaveBalanceGridViewModel> models, ref bool closePeriod )
        {
            List<int> EmpIds = models.Select(m => m.EmpId).ToList();
            var leavePostingLst = _hrUnitOfWork.Repository<LeavePosting>().Where(lp => EmpIds.Contains(lp.EmpId) && !lp.Posted && lp.PeriodId == models.FirstOrDefault().PeriodId);
            ////2.Accrual Balance استحقاق الرصيد
            //var oldOpenTrans = _hrUnitOfWork.Repository<LeaveTrans>().Where(t => t.CompanyId == CompanyId && EmpIds.Contains(t.EmpId) && t.TransType == 2 && t.TransFlag == 1 && t.TypeId == models.FirstOrDefault().TypeId && t.PeriodId == models.FirstOrDefault().NewPeriodId);

            //Leave posting
            foreach (LeaveBalanceGridViewModel model in models)
            {
                ///post -> repalcementTrans -> openBalance
                LeaveTrans openTrans = new LeaveTrans()
                {
                    CompanyId = model.CompanyId,
                    EmpId = model.EmpId,
                    PeriodId = model.NewPeriodId,
                    TypeId = model.TypeId,
                    AbsenceType = model.AbsenceType,
                    TransDate = model.SubStartDate ?? DateTime.Parse(model.NewPeriodDate),
                    CreatedUser = UserName,
                    CreatedTime = DateTime.Now
                };

                //Event: 1-Open Balance  2-Post Balance 12-Balance Replacement
                //PostAction: 1-Success 2-Add Time compensation trans 3-Fail
                LeavePosting posting = leavePostingLst.Where(lp => lp.EmpId == model.EmpId).FirstOrDefault();
                if (model.PeriodId != model.NewPeriodId) //&& !fail //only if new Period & not fail
                {   
                    LeaveTrans postTrans = new LeaveTrans();
                    AutoMapper(new Models.AutoMapperParm { Source = openTrans, Destination = postTrans, Transtype = TransType.Insert });
                    if (model.PostBal != 0)
                    {
                        postTrans.TransType = 1; //Post Balance رصيد سابق مرحل
                        postTrans.TransFlag = 1;
                        postTrans.TransQty = model.PostBal;
                        postTrans.TransDate = DateTime.Parse(model.NewPeriodDate);

                        _hrUnitOfWork.LeaveRepository.Add(postTrans);
                    }

                    if (model.PostAction == 2) //2-Add Time compensation
                    {
                        LeaveTrans repalcementTrans = new LeaveTrans();
                        AutoMapper(new Models.AutoMapperParm { Source = openTrans, Destination = repalcementTrans, Transtype = TransType.Insert });
                        repalcementTrans.PeriodId = model.PeriodId;
                        repalcementTrans.TransType = 12;
                        repalcementTrans.TransQty = model.Compensations;
                        repalcementTrans.TransFlag = -1;
                        postTrans.TransDate = DateTime.Parse(model.NewPeriodDate);
                        _hrUnitOfWork.LeaveRepository.Add(repalcementTrans);
                    }
                }

                if (model.PeriodId != model.NewPeriodId) //Post only if new Period
                {
                    if (posting == null) //new
                    {
                        posting = new LeavePosting()
                        {
                            EmpId = model.EmpId,
                            PeriodId = model.PeriodId,
                            Posted = (model.PostAction == 3 && model.PostBal != 0), //PostAction 3-fail -- MinDays Errors
                            Reason = model.Reason,
                            CreatedUser = UserName,
                            CreatedTime = DateTime.Now
                        };
                        _hrUnitOfWork.LeaveRepository.Add(posting);
                    }
                    else //update
                    {
                        posting.Posted = (model.PostAction == 3 && model.PostBal != 0); //--MinDays Errors
                        posting.Reason = model.Reason;
                        posting.CreatedUser = UserName;
                        posting.CreatedTime = DateTime.Now;
                        _hrUnitOfWork.LeaveRepository.Attach(posting);
                        _hrUnitOfWork.LeaveRepository.Entry(posting).State = EntityState.Modified;
                    }

                }

                //for retrive post balanca first
                if (model.OpenBalance != 0)
                {
                    openTrans.TransType = 2; //Accrual Balance استحقاق الرصيد
                    openTrans.TransQty = model.OpenBalance;
                    openTrans.TransFlag = 1;
                    //if (posting != null)
                    //{
                    //    var oldTrans = oldOpenTrans.Where(o => o.EmpId == model.EmpId).LastOrDefault();
                    //    if(oldTrans != null) _hrUnitOfWork.LeaveRepository.Remove(oldTrans);
                    //}

                    _hrUnitOfWork.LeaveRepository.Add(openTrans);
                }
            }
        }
        private void UpdatePeriod(LeaveBalanceGridViewModel model, bool closePeriod)
        {
            if (model.NewSubPeriodId != null) //Open sub period
            {
                SubPeriod subPeriod = _hrUnitOfWork.JobRepository.GetSubPeriod(model.NewSubPeriodId);
                List<SubPeriod> prevSubPeriods = _hrUnitOfWork.Repository<SubPeriod>()
                    .Where(s => s.PeriodId == subPeriod.PeriodId && s.Status != 2 && s.StartDate <= subPeriod.StartDate).ToList();
                foreach (var item in prevSubPeriods)
                {
                    item.Status = 2; // تم استحقاقها
                    item.ModifiedUser = UserName;
                    item.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.JobRepository.Attach(item);
                    _hrUnitOfWork.JobRepository.Entry(item).State = EntityState.Modified;
                }
            }

            if (model.PeriodId != model.NewPeriodId) //open new Period
            {
                Period period = _hrUnitOfWork.LeaveRepository.GetLperiod(model.NewPeriodId);
                period.Status = 1; //Open
                period.ModifiedUser = UserName;
                period.ModifiedTime = DateTime.Now;
                _hrUnitOfWork.JobRepository.Attach(period);
                _hrUnitOfWork.JobRepository.Entry(period).State = EntityState.Modified;
            }

            if (closePeriod) //Closing Period, prev periods and sub periods if colsePeriod && All PostAction success
            {
                Period period = _hrUnitOfWork.LeaveRepository.GetLperiod(model.PeriodId);

                //close prev periods
                List<Period> prevPeriods = _hrUnitOfWork.Repository<Period>()
                    .Where(p => p.CalendarId == period.CalendarId && p.Status != 2 && p.StartDate <= period.StartDate).ToList();
                foreach (Period item in prevPeriods)
                {
                    item.Status = 2; //close
                    item.ModifiedUser = UserName;
                    item.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.JobRepository.Attach(item);
                    _hrUnitOfWork.JobRepository.Entry(item).State = EntityState.Modified;
                }

                List<int> Ids = prevPeriods.Select(p => p.Id).ToList();
                List <SubPeriod> subPeriods = _hrUnitOfWork.Repository<SubPeriod>().Where(a => Ids.Contains(a.PeriodId) && a.Status != 2).ToList();
                foreach (SubPeriod item in subPeriods)
                {
                    item.Status = 2;
                    item.ModifiedUser = UserName;
                    item.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.JobRepository.Attach(item);
                    _hrUnitOfWork.JobRepository.Entry(item).State = EntityState.Modified;
                }
            }
        }

        ///Leave Posting -- Job - Schedual
        public string AutoLeavePosting(int typeId)
        {
            int? periodId = _hrUnitOfWork.LeaveRepository.GetOpenedLeavePeriods().FirstOrDefault()?.id;
            if (periodId == null) return MsgUtils.Instance.Trls("NoNewPeriod"); //No data to calculate or posting

            int subPeriodId = _hrUnitOfWork.BudgetRepository.GetSubPeriods(periodId.Value).Where(s => s.Status == 0).FirstOrDefault()?.Id ?? 0;

            IEnumerable<LeaveBalanceGridViewModel> grids;
            var result = _hrUnitOfWork.LeaveRepository.CalcLeaveBalance(out grids, typeId, periodId.Value, subPeriodId, CompanyId, Language);
            if (result != "Success")
                return result;

            //Posting
            bool closePeriod = false;
            AddTrans(grids, ref closePeriod);
            UpdatePeriod(grids.FirstOrDefault(), closePeriod);

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                return Errors.FirstOrDefault().errors.FirstOrDefault().message;

            return "OK";
        }


        #region LeaveTrans
        public ActionResult ReadLeaveTrans(int PeriodId, int EmpId, int TypeId)
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveTrans(TypeId, PeriodId, EmpId, Language);
           
            float currentBalance = 0;
            var querylst = query.ToList();
            for (int i = 0; i < querylst.Count(); i++)
                querylst.ElementAt(i).Balance = currentBalance += querylst.ElementAt(i).Balance;

            return Json(querylst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadLeaveTransSummary(int YearId, int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveTransSummary(YearId, CompanyId, Language);
            string whecls = GetWhereClause(MenuId);

            string filter = "";
            string Sorting = "";
            query = (IQueryable<LeaveTransSummary>)WebApp.Models.Utils.GetFilter(query, ref filter, ref Sorting);

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
                    Models.Utils.LogError(ex.Message);
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
        public ActionResult LeaveTrans(LeavePostingViewModel model)
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            DateTime Today = DateTime.Today;
            var Years = _hrUnitOfWork.BudgetRepository.ReadFiscal().OrderBy(y => y.StartDate);
            ViewBag.Years = Years.Select(y => new FormList { id = y.Id, name = y.Name }).ToList();
            ViewBag.currentYear = Years.Where(y => y.StartDate <= Today && y.EndDate >= Today).Select(y => y.Id).FirstOrDefault();

            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        #endregion


        //CheckLeavePlan
        public ActionResult CheckLeavePlan(int requestId)
        {
            LeaveRequest leaveReq = _hrUnitOfWork.LeaveRepository.Get(requestId);
            LeaveType type = _hrUnitOfWork.LeaveRepository.GetLeaveType(leaveReq.TypeId);

            GetStarsParamVM param = new GetStarsParamVM() {
                ComapnyId = CompanyId, EmpId = leaveReq.EmpId, PeriodId = leaveReq.PeriodId,
                StartDate = leaveReq.ActualStartDate ?? leaveReq.StartDate,
                EndDate = leaveReq.ActualEndDate ?? leaveReq.EndDate,
                RequestId = requestId, ExHolidays = type.ExHolidays
            };

            var message = new List<string>();
            if (type.IncLeavePlan)
            {
                int Stars, EmpStars;
                message = _hrUnitOfWork.LeaveRepository.CheckLeavePlan(param, Language, out Stars, out EmpStars);
            }

            return Json(String.Join("</ br>", message) , JsonRequestBehavior.AllowGet);
        }

        #region FollowUp

        public ActionResult FollowUp()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("LeaveCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(CompanyId,Language).Select(m => new { id = m.Id, name = m.Name });

            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            return View();
        }

        public ActionResult ReadLeaveFollowUp(int MenuId)
        {
            //2-submit
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveReqFollowUp(CompanyId, Language);
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
            LeaveReqViewModel request = _hrUnitOfWork.LeaveRepository.GetRequest(Id, Language);

            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("LeaveReqFollowUpForm", CompanyId, Version);
            //if (columns.FirstOrDefault(fc => fc == "ReplaceEmpId") == null)
            ViewBag.Employees = _hrUnitOfWork.LeaveRepository.GetReplaceEmpList(request.EmpId, Language);

            ViewBag.CalcOptions = _hrUnitOfWork.LeaveRepository.GetLeaveType(request.TypeId); //for Calender
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId); //for Calender

            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            ViewBag.isSSMenu = _hrUnitOfWork.MenuRepository.Get(MenuId)?.SSMenu ?? false;

            return View(request);
        }

        [HttpPost]
        public ActionResult FollowupDetails(LeaveReqViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> Errors = new List<Error>();
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveReqFollowUpForm",
                        TableName = "LeaveRequests",
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
                ObjectName = "LeaveRequest",
                Version = Convert.ToByte(Request.Form["Version"]),
                Options = moreInfo,
                Transtype = TransType.Update
            });

            if (request.ApprovalStatus != 9)
            {
                request.RejectDesc = null;
                request.RejectReason = null;
            }

            if (request.ApprovalStatus == 5)
            {
                request.ActualStartDate = request.StartDate;
                request.ActualNofDays = request.NofDays;
                request.ActualEndDate = request.EndDate;
            }
            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = UserName;

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

                    _hrUnitOfWork.LeaveRepository.AddAcceptLeaveTrans(request, UserName);
                }
            }

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("OK");
        }

        [HttpPost]
        public ActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(Id);
            string error = "";
            if (Send == 1) {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "LeaveRequest",
                    SourceId =Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
                    ValueAfter = MsgUtils.Instance.Trls("EmployeeReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });
                request.ApprovalStatus = 3;
                error = AddWFTrans(request, null, true);
            }
            else if (Send == 2) {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "LeaveRequest",
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

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

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
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "LeaveRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
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
                Version = Convert.ToByte(Request.Form["Version"]),
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
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel before approved"),
                ValueBefore = MsgUtils.Instance.Trls("Submit")
            });
            request.CancelDesc = Desc;
            request.CancelReason = Reason;
            request.ApprovalStatus = 7;
           // request.ReqStatus = 4;

            string error = AddWFTrans(request, null, null);
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Json("Ok");
        }

        //In client because Auto Mapper
        private string ChangeAssignmentStatus(LeaveRequest request)
        {
            LeaveType type;
            //if future assignment && leave type chanege assignment -> prevent
            string assignError = _hrUnitOfWork.LeaveRepository.CheckAssignStatus(request.EmpId, request.TypeId, out type, Language);
            if (!string.IsNullOrEmpty(assignError))
                return assignError;

            if (type.AssignStatus != null )
            {
                Assignment copy = _hrUnitOfWork.EmployeeRepository.Find(a => a.EmpId == request.EmpId && a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date).FirstOrDefault();
                short oldAssignStatus = copy.AssignStatus;
                short oldSysAssignStatus = copy.SysAssignStatus;

                copy.EndDate = request.ActualStartDate.Value.AddDays(-1);
                copy.ModifiedUser = UserName;
                copy.ModifiedTime = DateTime.Now;
                _hrUnitOfWork.EmployeeRepository.Attach(copy);
                _hrUnitOfWork.EmployeeRepository.Entry(copy).State = EntityState.Modified;

                Assignment assignLeave = new Assignment();
                AutoMapper(new Models.AutoMapperParm() { Source = copy, Destination = assignLeave });
                assignLeave.AssignStatus = type.AssignStatus.Value;
                assignLeave.SysAssignStatus = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => a.CodeName == "Assignment" && a.CodeId == type.AssignStatus.Value).Select(a => a.SysCodeId).FirstOrDefault();
                assignLeave.AssignDate = request.ActualStartDate.Value;
                assignLeave.CreatedUser = UserName;
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
                    returnAssign.EndDate = new DateTime(2099,1,1);
                    returnAssign.CreatedUser = UserName;
                    returnAssign.CreatedTime = DateTime.Now;
                    _hrUnitOfWork.EmployeeRepository.Add(returnAssign); //ii
                }
                else
                {
                    assignLeave.EndDate = new DateTime(2099, 1, 1);
                    _hrUnitOfWork.EmployeeRepository.Add(assignLeave); //i
                }

                var assignmentCodes = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Language);
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "AssignStatus",
                    CompanyId = CompanyId,
                    ObjectName = "AssignmentsForm",
                    SourceId = copy.Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
                    ValueAfter = assignmentCodes.FirstOrDefault(a => a.CodeId == type.AssignStatus.Value).Name,
                    ValueBefore = assignmentCodes.FirstOrDefault(a => a.CodeId == oldAssignStatus).Name
                });
            }

            return null;
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
                return wf.WorkFlowStatus;
            }
            else 
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }

       
        #endregion

        #region Accepted Leaves
        public ActionResult AcceptedLeaves()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult ReadAcceptedLeave(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.LeaveRepository.GetApprovedLeaveReq(CompanyId, Language).AsQueryable();
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

        public ActionResult AcceptedLeaveDetails(int Id, byte Version)
        {
            LeaveReqViewModel request = _hrUnitOfWork.LeaveRepository.GetRequest(Id, Language);

            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("LeaveRequest", CompanyId, Version);
            //if (columns.FirstOrDefault(fc => fc == "ReplaceEmpId") == null)
            ViewBag.Employees = _hrUnitOfWork.LeaveRepository.GetReplaceEmpList(request.EmpId, Language);

            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("LeaveCancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            return View(request);
        }

        public ActionResult CancelAccepted(int Id, short? Reason, string Desc)
        {
            LeaveRequest request = _hrUnitOfWork.LeaveRepository.Get(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "LeaveRequest",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
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
                Version = Convert.ToByte(Request.Form["Version"]),
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
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel after approved"),
                ValueBefore = MsgUtils.Instance.Trls("Approved")
            });
            request.CancelDesc = Desc;
            request.CancelReason = Reason;
            request.ApprovalStatus = 8;

            _hrUnitOfWork.EmployeeRepository.CancelLeaveAssignState(request, UserName, Convert.ToByte(Request.Form["Version"]), Language);

            string message = _hrUnitOfWork.LeaveRepository.AddCancelLeaveTrans(request, UserName, Language);
            if (message.Length > 0)
                return Json(message);

            _hrUnitOfWork.LeaveRepository.Attach(request);
            _hrUnitOfWork.LeaveRepository.Entry(request).State = EntityState.Modified;
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("Ok");
        }
        #endregion
    }
}