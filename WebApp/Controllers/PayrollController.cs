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
using WebApp.Extensions;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class PayrollController : BaseController
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
        public PayrollController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPayroll(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.GetPayrolls(CompanyId);
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
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

        public ActionResult ReadPayrollDue(int PayrollId)
        {
            return Json(_hrUnitOfWork.PayrollRepository.readPayDue(PayrollId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriods(int PeriodId)
        {
            var result = _hrUnitOfWork.BudgetRepository.GetPeriods(PeriodId).Select(a => new { id = a.Id, name = a.Name });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadSubperiod(int? Period)
        {
            var result = _hrUnitOfWork.BudgetRepository.GetSubPeriods(Period.GetValueOrDefault());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEndSubPeriods(int PeriodId)
        {
            var period = _hrUnitOfWork.BudgetRepository.GetPeriods(PeriodId).Select(a => new PayrollDueViewModel { Id = a.Id }).ToList();
            List<DateTime> arr = new List<DateTime>();
            foreach (var item in period)
            {
                arr.AddRange(_hrUnitOfWork.BudgetRepository.GetSubPeriods(item.Id).Select(a => a.EndDate).ToList());
            }

            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            ViewBag.PeriodId = _hrUnitOfWork.BudgetRepository.GetCalender(CompanyId).Where(a => a.PeriodLength > 0).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.BankId = _hrUnitOfWork.Repository<Provider>().Where(a => a.ProviderType == 7).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.AccrualSalAcct = _hrUnitOfWork.Repository<Account>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollDue = _hrUnitOfWork.Repository<PayrollDue>().Where(a => a.PayrollId == id).Select(a => new { value = a.Id, text = a.Name });
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId);

            if (id == 0)
                return View(new PayrollFormViewModel());

            var record = _hrUnitOfWork.PayrollRepository.GetPayroll(id);
            return record == null ? (ActionResult)HttpNotFound() : View(record);
        }
        [HttpPost]
        public ActionResult Details(PayrollFormViewModel model, OptionsViewModel moreInfo, PayrollDueVM grid1, SubPeriodsVM grid2)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Payroll",
                        TableName = "Payrolls",
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

                Payrolls record;
                var PeriodIdLocal = _hrUnitOfWork.Repository<PeriodName>().Where(a => a.Id == model.PeriodId).Select(a => new { Local = a.IsLocal }).FirstOrDefault();

                //insert
                if (model.Id == 0)
                {
                    record = new Payrolls();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Payroll",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.IsLocal = PeriodIdLocal.Local;
                    record.CompanyId = record.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.PayrollRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<Payrolls>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Payroll",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.IsLocal = PeriodIdLocal.Local;
                    record.CompanyId = record.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.PayrollRepository.Attach(record);
                    _hrUnitOfWork.PayrollRepository.Entry(record).State = EntityState.Modified;
                }

                // Save grid1
                var payDueList = new PayDueListViewModel();
                payDueList = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid")), record);
                errors = payDueList.errors;
                errors = SaveGrid2(grid2, payDueList.payDueList, ModelState.Where(a => a.Key.Contains("grid")), record, model.Period.GetValueOrDefault());

                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                var message = "OK";
                if (errors.Count > 0) message = errors.First().errors.First().message;

                return Json(message);
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }

        private PayDueListViewModel SaveGrid1(PayrollDueVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, Payrolls record)
        {
            //  List<Error> errors = new List<Error>();
            // List<PayrollDue> ListPayDue = new List<PayrollDue>();

            var payDueList = new PayDueListViewModel();
            payDueList.payDueList = new List<PayrollDue>();



            // Deleted
            if (grid1.deleted != null)
            {
                foreach (PayrollDueViewModel model in grid1.deleted)
                {
                    var payrolldeu = new PayrollDue
                    {
                        Id = model.Id
                    };


                    _hrUnitOfWork.PayrollRepository.Remove(payrolldeu);
                }
            }

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    payDueList.errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayrollDue",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (payDueList.errors.Count() > 0) return payDueList;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (PayrollDueViewModel model in grid1.updated)
                {
                    var payrolldue = new PayrollDue();
                    AutoMapper(new Models.AutoMapperParm { Destination = payrolldue, Source = model, Transtype = TransType.Update });
                    _hrUnitOfWork.PayrollRepository.Attach(payrolldue);
                    _hrUnitOfWork.PayrollRepository.Entry(payrolldue).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (PayrollDueViewModel model in grid1.inserted)
                {
                    var payrolldue = new PayrollDue();
                    AutoMapper(new Models.AutoMapperParm { Destination = payrolldue, Source = model, Transtype = TransType.Insert });
                    payrolldue.Payroll = record;
                    _hrUnitOfWork.PayrollRepository.Add(payrolldue);
                    payDueList.payDueList.Add(payrolldue);

                }

            }

            return payDueList;
        }
        //SaveGrid2
        private List<Error> SaveGrid2(SubPeriodsVM grid2, IList<PayrollDue> insertedPayDue, IEnumerable<KeyValuePair<string, ModelState>> state, Payrolls record, int Period)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid2.deleted != null)
            {
                foreach (SubPeriodsViewModel model in grid2.deleted)
                {
                    var subPeriod = new SubPeriod
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.JobRepository.Remove(subPeriod);
                }
            }

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SubPeriod",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid2.updated != null)
            {
                foreach (SubPeriodsViewModel model in grid2.updated)
                {
                    var subPeriod = new SubPeriod();
                    AutoMapper(new Models.AutoMapperParm { Destination = subPeriod, Source = model, Transtype = TransType.Update });
                    if (model.PayDueId <= 0)
                    {
                        var pDueRecord = insertedPayDue.Where(a => a.Id == model.PayDueId).FirstOrDefault();
                        subPeriod.PayDue = pDueRecord;
                    }
                    _hrUnitOfWork.JobRepository.Attach(subPeriod);
                    _hrUnitOfWork.JobRepository.Entry(subPeriod).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid2.inserted != null)
            {
                foreach (SubPeriodsViewModel model in grid2.inserted)
                {
                    var subPeriod = new SubPeriod();
                    AutoMapper(new Models.AutoMapperParm { Destination = subPeriod, Source = model, Transtype = TransType.Insert });
                    subPeriod.PeriodId = Period;

                    if (model.PayDueId <= 0)
                    {
                        var pDueRecord = insertedPayDue.Where(a => a.Id == model.PayDueId).FirstOrDefault();
                        subPeriod.PayDue = pDueRecord;
                    }
                    _hrUnitOfWork.JobRepository.Add(subPeriod);
                }
            }

            return errors;
        }

        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<PayrollViewModel> Source = new DataSource<PayrollViewModel>();
            Payrolls payrollObj = _hrUnitOfWork.PayrollRepository.Get(id);
            if (payrollObj != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = payrollObj,
                    ObjectName = "Payrolls",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PayrollRepository.Remove(payrollObj);
            }
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }

    }
}