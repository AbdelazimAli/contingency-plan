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

namespace WebApp.Controllers
{
    public class LeaveActionController : BaseController
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
        public LeaveActionController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        #region Leave Actions by Seham
        public ActionResult Index()
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("ActionType", Language).Select(a => new { value = a.CodeId, text = a.Title });
            ViewBag.Posted = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Posted", Language).Select(a => new { value = a.CodeId, text = a.Title });
            return View();
        }
        public ActionResult GetLeaveAction(int MenuId)
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveAction(Language,CompanyId);
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
        public ActionResult GetLeaveType(int LeaveId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetLeavePeriods(LeaveId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpLeaveType(int EmpId)
        {
            var result = _hrUnitOfWork.LeaveRepository.GetEmpLeaveTypes(EmpId, CompanyId, Language).Select(a=> new { id=a.Id , name=a.Name});
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id = 0, byte Version = 0)
        {
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("LeaveAction", CompanyId, Version, "EmpId"))
                ViewBag.PersonId = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);

            ViewBag.TypeId = _hrUnitOfWork.LeaveRepository.GetLeaveTypesList(CompanyId, Language);

            if (id == 0)
                return View(new LeaveActionFormViewModel());

            var LeaveAction = _hrUnitOfWork.LeaveRepository.ReadleaveAction(id);
            ViewBag.PeriodId = _hrUnitOfWork.LeaveRepository.GetLeavePeriods(LeaveAction.TypeId);

            return LeaveAction == null ? (ActionResult)HttpNotFound() : View(LeaveAction);
        }
        [HttpPost]
        public ActionResult Details(LeaveActionFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveAction",
                        TableName = "LeaveAdjusts",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

                var record = _hrUnitOfWork.LeaveRepository.GetLeaveAction(model.Id);
                //insert
                if (record == null)
                {
                    record = new LeaveAdjust();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "LeaveAction",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.AdjustDate = DateTime.Now;
                    record.CompanyId = CompanyId;
                    _hrUnitOfWork.LeaveRepository.Add(record);

                }

                //update
                else
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "LeaveAction",
                        Options = moreInfo,
                        Transtype=TransType.Update
                    });

                    record.CompanyId = CompanyId;
                    _hrUnitOfWork.LeaveRepository.Attach(record);
                    _hrUnitOfWork.LeaveRepository.Entry(record).State = EntityState.Modified;

                }
                var Errors = SaveChanges(Language);
                var message = "OK";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);

            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }
        public ActionResult DeleteLeaveaction(int id)
        {
            DataSource<LeaveActionViewModel> Source = new DataSource<LeaveActionViewModel>();
            List<Error> error = new List<Error>();
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            List<Error> errors = new List<Error>();
            LeaveAdjust obj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(id);
            //if (obj.Posted == false)
            //{
            //    ErrorMessage errormessa = new ErrorMessage() { message = "Change Posted Value to This Record Then Delete it" };
            //    errorMessages.Add(errormessa);
            //    Error err = new Error() { errors = errorMessages };
            //    error.Add(err);
            //    Source.Errors = error;
            //    return Json(Source);
            //}
            if (obj != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = obj,
                    ObjectName = "LeaveAction",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.LeaveRepository.Remove(obj);
            }
            string message = "OK";
            if (errors.Count() > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult PostLeaveAction(int objId)
        {
            //LeaveAdjust oldobj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(objId);
            //_hrUnitOfWork.LeaveRepository.PostLeaveAction(oldobj, UserName);
            //var Errors = SaveChanges(Language);
            //var message = "OK";
            //if (Errors.Count > 0)
            //    message = Errors.First().errors.First().message;
            return Json("Please remove this button", JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnPostLeaveAction(int objId)
        {
            //LeaveAdjust oldobj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(objId);
            //_hrUnitOfWork.LeaveRepository.UnPostLeaveAction(oldobj, UserName, Language);
            //var Errors = SaveChanges(Language);
            //var message = "OK";
            //if (Errors.Count > 0)
            //    message = Errors.First().errors.First().message;
            return Json("Please remove this button", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GroupLeaveActionIndex()
        {
            ViewBag.TypeId = _hrUnitOfWork.LeaveRepository.GetLeaveTypesList(CompanyId, Language);

            return View(new LeaveActionFormViewModel());
        }

        [HttpPost]
        public ActionResult GroupLeaveActionDetails(LeaveActionFormViewModel model, OptionsViewModel moreInfo, int[] grid1)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveAction",
                        TableName = "LeaveAdjusts",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
                var message = "OK";
                var Employee = _hrUnitOfWork.Repository<Assignment>().Where(a => grid1.Contains(a.DepartmentId)).Select(a => a.EmpId).ToList().Distinct();
                if (Employee.Count() > 0)
                    foreach (var item in Employee)
                    {
                        var record = new LeaveAdjust();
                        AutoMapper(new Models.AutoMapperParm { Destination = record, Source = model, ObjectName = "LeaveAction",  Options = moreInfo });
                        record.EmpId = item;
                        record.CreatedUser = UserName;
                        record.CreatedTime = DateTime.Now;
                        record.AdjustDate = DateTime.Now;
                        record.CompanyId = CompanyId;
                        _hrUnitOfWork.LeaveRepository.Add(record);
                        //_hrUnitOfWork.LeaveRepository.PostLeaveAction(record, UserName);
                    }
                else
                    message = MsgUtils.Instance.Trls("thereisnoEmployee");
                var Errors = SaveChanges(Language);
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                return Json(message);
            }
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }
        #endregion


        #region LeaveActions by Mamdouh
        public ActionResult LeaveActionIndex()
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("ActionType", Language).Select(a => new { value = a.CodeId, text = a.Title });
            string Role = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            return View();
        }
        //Index to First Tab
        public ActionResult FirstOpenBalance()
        {
            //Get List of Leaves has acural Period
            ViewBag.LeaveTypes  = _hrUnitOfWork.LeaveRepository.GetAccrualLeaveTypes(CompanyId, Language);
            ViewBag.GridLeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralGridLeaveTypes(CompanyId, Language);
            ViewBag.FiscalYears = _hrUnitOfWork.Repository<FiscalYear>().Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.Dept = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language);
            return View();
        }

        // read grid 
        public ActionResult ReadLeaveAction( int LeaveId, string FiscalYearDate, string Departments)
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveFirstTrans(CompanyId, Language, LeaveId, Convert.ToDateTime(FiscalYearDate), Departments);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //Add Trans Leave record
        [HttpPost] 
        public ActionResult PostLeaveTrans(IEnumerable<LeaveTransOpenBalanceViewModel> models,string transDate)
        {
            string message = "Ok";
            var AbsenceType = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == models.Select(s => s.TypeId).FirstOrDefault()).Select(a => a.AbsenceType).FirstOrDefault();
            foreach (var item in models)
            {
                if(item.flag == true && item.transQty != 0)
                {
                    var LeaveTrans = new LeaveTrans
                    {
                        AbsenceType = AbsenceType,
                        EmpId = item.EmpId,
                        TransDate = Convert.ToDateTime(transDate),
                        TransQty = item.transQty,
                        TransType = 0 ,
                        CompanyId = CompanyId,
                        TransFlag = 1,
                        CreatedTime = DateTime.Now,
                        CreatedUser =UserName,
                        PeriodId = item.PeriodId,
                        TypeId = item.TypeId                    
                    };
                    _hrUnitOfWork.LeaveRepository.Add(LeaveTrans);
                }
            }
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPeriod(string Period , int LeaveId)
        {
            var checkPeriod = _hrUnitOfWork.LeaveRepository.CheckPeriods(Convert.ToDateTime(Period), LeaveId,Language);       
            return Json(checkPeriod, JsonRequestBehavior.AllowGet);
        }
        // Index to second tab Money repaid
        public ActionResult MoneyOpenBalance()
        {
            //Get List of Leaves has acural Period
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            ViewBag.GridLeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralGridLeaveTypes(CompanyId, Language);
            ViewBag.Periods = _hrUnitOfWork.Repository<Period>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            return View();
        }
        // read Grid
        public ActionResult ReadLeaveAdjust()
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveMoneyTrans(CompanyId, Language).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //AddNewAdjustment
        public ActionResult AddNewAdjustment(byte Version = 0)
        {
            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetAccrualLeaveTypes(CompanyId, Language);
            
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("AdjustLeave", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(new LeaveMoneyAdjustViewModel());
        }
        //FillPeriod
        public ActionResult FillPeriod(int typeId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetLeavePeriods(typeId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCurrentBalance(int TypeId , int PeriodId , int EmpId)
        {
            var Balance =  _hrUnitOfWork.Repository<LeaveTrans>().Where(a => a.TypeId == TypeId && a.PeriodId == PeriodId && a.EmpId == EmpId).Sum(a => a.TransQty * a.TransFlag);
            return Json(Balance, JsonRequestBehavior.AllowGet);
        }
        //SaveAdjustment
        public ActionResult SaveAdjustment(LeaveMoneyAdjustFormViewModel model, OptionsViewModel moreInfo)
        {
            var message = "OK";
            if (model.NofDays > 0)
            {
                if (model.NofDays <= model.Balance)
                {
                    var Absence = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == model.TypeId).Select(s => s.AbsenceType).FirstOrDefault();
                    var TransRecord = new LeaveTrans()
                    {
                        AbsenceType = Absence,
                        CompanyId = CompanyId,
                        CreatedUser = UserName,
                        CreatedTime = DateTime.Now,
                        TransFlag = -1,
                        EmpId = model.EmpId,
                        PeriodId = model.PeriodId,
                        TypeId = model.TypeId,
                        TransDate = DateTime.Now,
                        TransType = 12,
                        TransQty = model.NofDays

                    };
                    _hrUnitOfWork.LeaveRepository.Add(TransRecord);
                    var AdjustRecord = new LeaveAdjust()
                    {
                        NofDays = model.NofDays,
                        EmpId = model.EmpId,
                        PeriodId = model.PeriodId,
                        CreatedUser = UserName,
                        CreatedTime = DateTime.Now,
                        TypeId = model.TypeId,
                        AdjustDate = DateTime.Now,
                        CompanyId = CompanyId,
                        TransType = 12,
                        PayDone = false
                    };
                    _hrUnitOfWork.LeaveRepository.Add(AdjustRecord);

                }
                else
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("NotEnoughBalance"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("EnterNoOfDays"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            return Json(message);

        }
       // Delete LeaveAdjust where Period Status is Open and Paydone in leave adjust equal false 
        public ActionResult DeleteLeaveAdjust(int id)
        {
            DataSource<LeaveActionViewModel> Source = new DataSource<LeaveActionViewModel>();
            List<Error> error = new List<Error>();
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            List<Error> errors = new List<Error>();
            LeaveAdjust obj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(id);
           
            if (obj != null)
            {
               var status = _hrUnitOfWork.Repository<Period>().Where(a => a.Id == obj.PeriodId).Select(a => a.Status).FirstOrDefault();
                if (obj.PayDone == true)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("PayDoneIsTrue") };
                    errorMessages.Add(errormessa);
                    Error err = new Error() { errors = errorMessages };
                    error.Add(err);
                    Source.Errors = error;
                    return Json(Source);
                }
                if (status != 1)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("StatusIsClosed") };
                    errorMessages.Add(errormessa);
                    Error err = new Error() { errors = errorMessages };
                    error.Add(err);
                    Source.Errors = error;
                    return Json(Source);
                }
                var Absence = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == obj.TypeId).Select(s => s.AbsenceType).FirstOrDefault();
                var TransRecord = new LeaveTrans()
                {
                    AbsenceType = Absence,
                    CompanyId = CompanyId,
                    CreatedUser = UserName,
                    CreatedTime = DateTime.Now,
                    TransFlag = 1,
                    EmpId = obj.EmpId,
                    PeriodId = obj.PeriodId,
                    TypeId = obj.TypeId,
                    TransDate = DateTime.Now,
                    TransType = 24,
                    TransQty = obj.NofDays

                };
                _hrUnitOfWork.LeaveRepository.Add(TransRecord);

                AutoMapper(new Models.AutoMapperParm
                {
                    Source = obj,
                    ObjectName = "LeaveAction",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.LeaveRepository.Remove(obj);
            }
            string message = "OK";
            if (errors.Count() > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult LeaveCreditBalance()
        {
            string MenuId = Request.QueryString["MenuId"];
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            ViewBag.GridLeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralGridLeaveTypes(CompanyId, Language);
            ViewBag.Periods = _hrUnitOfWork.Repository<Period>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            return View();
        }
        public ActionResult ReadCreditBalance()
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveCreaditBalance(CompanyId, Language).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddCreditDebitAdjustment(byte Version = 0)
        {
            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetAccrualLeaveTypes(CompanyId, Language);
            
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("AdjustCreditLeave", CompanyId, Version, "EmpId"))
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee });

            return View(new LeaveMoneyAdjustViewModel());
        }
        //SaveCreditAdjustment
        public ActionResult SaveCreditAdjustment(LeaveCreditAdjustFormViewModel model)
        {
            var message = "OK";
            if (model.NofDays > 0)
            {
                if (model.Credit == 1)
                {
                    AddActions(model, 4, 1);
                }
                else if (model.Credit == 2)
                {
                    if (model.NofDays <= model.Balance)
                    {
                        {
                            AddActions(model, 13, -1);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("NotEnoughBalance"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("EnterNoOfDays"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            return Json(message);
        }
        public void AddActions (LeaveCreditAdjustFormViewModel model,short transType, short transFlag)
        {
            var Absence = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == model.TypeId).Select(s => s.AbsenceType).FirstOrDefault();

            var TransRecord = new LeaveTrans()
            {
                AbsenceType = Absence,
                CompanyId = CompanyId,
                CreatedUser = UserName,
                CreatedTime = DateTime.Now,
                TransFlag = transFlag,
                EmpId = model.EmpId,
                PeriodId = model.PeriodId,
                TypeId = model.TypeId,
                TransDate = DateTime.Now,
                TransType = transType,
                TransQty = model.NofDays

            };
            _hrUnitOfWork.LeaveRepository.Add(TransRecord);
            var AdjustRecord = new LeaveAdjust()
            {
                NofDays = model.NofDays,
                EmpId = model.EmpId,
                PeriodId = model.PeriodId,
                CreatedUser = UserName,
                CreatedTime = DateTime.Now,
                TypeId = model.TypeId,
                AdjustDate = DateTime.Now,
                CompanyId = CompanyId,
                TransType = transType,
                PayDone = false
            };
            _hrUnitOfWork.LeaveRepository.Add(AdjustRecord);

        }
        public ActionResult DeleteCreditBalance(int id)
        {
            DataSource<LeaveMoneyAdjustViewModel> Source = new DataSource<LeaveMoneyAdjustViewModel>();
            List<Error> error = new List<Error>();
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            List<Error> errors = new List<Error>();
            LeaveAdjust obj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(id);
            var Balance = _hrUnitOfWork.Repository<LeaveTrans>().Where(a => a.TypeId == obj.TypeId && a.PeriodId == obj.PeriodId && a.EmpId == obj.EmpId).Sum(a => a.TransQty * a.TransFlag);
            if (obj.TransType == 4)
            {
                if (obj.NofDays > Balance)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("BalanceNegative") };
                    errorMessages.Add(errormessa);
                    Error err = new Error() { errors = errorMessages };
                    error.Add(err);
                    Source.Errors = error;
                    return Json(Source);
                }
            }
            if (obj != null)
            {
                var status = _hrUnitOfWork.Repository<Period>().Where(a => a.Id == obj.PeriodId).Select(a => a.Status).FirstOrDefault();          
                if (status != 1)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("StatusIsClosed") };
                    errorMessages.Add(errormessa);
                    Error err = new Error() { errors = errorMessages };
                    error.Add(err);
                    Source.Errors = error;
                    return Json(Source);
                }
               
                var Absence = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == obj.TypeId).Select(s => s.AbsenceType).FirstOrDefault();
               if(obj.TransType == 4)
                {
                    AddLeaveTrans(obj, Absence, 14, -1);
                }
                else if(obj.TransType == 13)
                {
                    AddLeaveTrans(obj, Absence, 23, 1);
                }

                AutoMapper(new Models.AutoMapperParm
                {
                    Source = obj,
                    ObjectName = "LeaveAction",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.LeaveRepository.Remove(obj);
            }
            string message = "OK";
            if (errors.Count() > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public void AddLeaveTrans(LeaveAdjust obj , short Absence ,short TransType, short TransFlag)
        {
            var TransRecord = new LeaveTrans()
            {
                AbsenceType = Absence,
                CompanyId = CompanyId,
                CreatedUser = UserName,
                CreatedTime = DateTime.Now,
                TransFlag = TransFlag,
                EmpId = obj.EmpId,
                PeriodId = obj.PeriodId,
                TypeId = obj.TypeId,
                TransDate = DateTime.Now,
                TransType = TransType,
                TransQty = obj.NofDays

            };
            _hrUnitOfWork.LeaveRepository.Add(TransRecord);
        }
        public ActionResult LeaveRestCredit()
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            ViewBag.GridLeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralGridLeaveTypes(CompanyId, Language);
            ViewBag.Periods = _hrUnitOfWork.Repository<Period>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            return View();
        }
        public ActionResult ReadRestBalance()
        {
            var query = _hrUnitOfWork.LeaveRepository.GetLeaveRest(CompanyId, Language).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddRestAdjustment()
        {
            ViewBag.LeaveTypes = _hrUnitOfWork.LeaveRepository.GetAcuralRestLeaveTypes(CompanyId, Language);
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee }).ToList();
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId);
            return View(new LeaveRestFormViewModel());
        }
        public ActionResult FillNoDays(int TypeId)
        {
            bool ChkFraction = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == TypeId).Select(a => a.AllowFraction).FirstOrDefault();
            return Json(ChkFraction, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillEmployeesMultiSelect(string Departments)
        {
            var Emp =_hrUnitOfWork.LeaveRepository.GetDeptEmployees(CompanyId, Language, Departments).ToList();
            return Json(Emp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRestLeave(LeaveRestFormViewModel model)
        {
            var message = "OK";
            int periodId = 0;
            var LeaveTypeRecord = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == model.TypeId).Select(s => new { Absence = s.AbsenceType, CalenderId = s.CalendarId }).FirstOrDefault();
            var PeriodRecord = _hrUnitOfWork.Repository<Period>().Where(a => a.StartDate <= model.WorkingDate && a.EndDate >= model.WorkingDate && a.CalendarId == LeaveTypeRecord.CalenderId).FirstOrDefault();
            if(PeriodRecord == null)
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("Periodnotdefined"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            else
            {
                periodId = PeriodRecord.Id;
            }
            foreach (var item in model.IEmpId)
            {

                var TransRecord = new LeaveTrans()
                {
                    AbsenceType = LeaveTypeRecord.Absence,
                    CompanyId = CompanyId,
                    CreatedUser = UserName,
                    CreatedTime = DateTime.Now,
                    TransFlag = 1,
                    EmpId = item,
                    PeriodId = periodId,
                    TypeId = model.TypeId,
                    TransDate = DateTime.Now,
                    TransType = 3,
                    TransQty = model.NofDays 
                };
                _hrUnitOfWork.LeaveRepository.Add(TransRecord);
                var AdjustRecord = new LeaveAdjust()
                {
                    NofDays = model.NofDays,
                    EmpId = item,
                    PeriodId = periodId,
                    CreatedUser = UserName,
                    CreatedTime = DateTime.Now,
                    TypeId = model.TypeId,
                    AdjustDate = DateTime.Now,
                    CompanyId = CompanyId,
                    TransType = 3,
                    PayDone = false
                };
                _hrUnitOfWork.LeaveRepository.Add(AdjustRecord);

            }
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            return Json(message);

        }
        public ActionResult DeleteRestBalance(int id)
        {
            DataSource<LeaveMoneyAdjustViewModel> Source = new DataSource<LeaveMoneyAdjustViewModel>();
            List<Error> error = new List<Error>();
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            List<Error> errors = new List<Error>();
            LeaveAdjust obj = _hrUnitOfWork.LeaveRepository.GetLeaveAction(id);
            if (obj != null)
            {
                var status = _hrUnitOfWork.Repository<Period>().Where(a => a.Id == obj.PeriodId).Select(a => a.Status).FirstOrDefault();
                if (status != 1)
                {
                    ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("StatusIsClosed") };
                    errorMessages.Add(errormessa);
                    Error err = new Error() { errors = errorMessages };
                    error.Add(err);
                    Source.Errors = error;
                    return Json(Source);
                }
                var Absence = _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id == obj.TypeId).Select(s => s.AbsenceType).FirstOrDefault();
                AddLeaveTrans(obj, Absence, 14, -1);
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = obj,
                    ObjectName = "LeaveAction",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.LeaveRepository.Remove(obj);
            }
            string message = "OK";
            if (errors.Count() > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        #endregion
    }
}