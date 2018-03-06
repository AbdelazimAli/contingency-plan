using Interface.Core;
using Model.ViewModel;
using Model.ViewModel.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using WebApp.Extensions;
using Model.Domain;
using System.Data.Entity;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class PayRollGradesController : BaseController
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
        public PayRollGradesController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        #region Payroll Grades
        public ActionResult Index()
        {
            var Code = _hrUnitOfWork.Repository<PayrollGrade>().DefaultIfEmpty().Max(a => a == null ? 0 : a.Code);
            ViewBag.Code = Code + 1;
            return View();
        }
        public ActionResult ReadPayrollGrades(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Language, CompanyId).AsQueryable();
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
        public ActionResult CreatePayrollGrades(IEnumerable<PayrollGradesViewModel> models)
        {
            var result = new List<PayrollGrade>();

            var datasource = new DataSource<PayrollGradesViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayrollGrades",
                        TableName = "PayrollGrades",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (PayrollGradesViewModel p in models)
                {

                    var payroll = new PayrollGrade();
                    payroll.Points = p.Point == null ? null : string.Join(",", p.Point.ToArray());
                    payroll.CreatedUser = UserName;
                    payroll.CreatedTime = DateTime.Now;
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = payroll,
                        Source = p,
                        ObjectName = "PayrollGrades",
                        Transtype = TransType.Insert
                    });
                    payroll.CompanyId = p.IsLocal ? CompanyId : (int?)null;
                    result.Add(payroll);
                    _hrUnitOfWork.PayrollRepository.Add(payroll);

                }


                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from f in models
                               join r in result on f.Code equals r.Code
                               select new PayrollGradesViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Code = f.Code,
                                   EndDate = f.EndDate,
                                   Name = f.Name,
                                   IsLocal = f.IsLocal,
                                   Group = f.Group,
                                   Grade = f.Grade,
                                   SubGrade = f.SubGrade,
                                   Points = r.Points,
                                   StartDate = f.StartDate,
                                   PointName=f.PointName
                                                                    
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeletePayrollGrades(int Id)
        {
            var datasource = new DataSource<PayrollGradesViewModel>();

            var Obj = _hrUnitOfWork.Repository<PayrollGrade>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "PayrollGrades",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.PayrollRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        public ActionResult UpdatePayrollGrades(IEnumerable<PayrollGradesViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PayrollGradesViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PayrollGrades",
                        TableName = "PayrollGrades",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_PayrollGrade = _hrUnitOfWork.Repository<PayrollGrade>().Where(a => ids.Contains(a.Id)).ToList();
                foreach (PayrollGradesViewModel p in models)
                {
                  var PayrollGrade = db_PayrollGrade.FirstOrDefault(a => a.Id == p.Id);
                    PayrollGrade.Points = p.Point == null ? null : string.Join(",", p.Point.ToArray());
                    PayrollGrade.ModifiedUser = UserName;
                    PayrollGrade.ModifiedTime = DateTime.Now;
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = PayrollGrade,
                        Source = p,
                        ObjectName = "PayrollGrades",
                        Transtype = TransType.Update
                    });
                    PayrollGrade.CompanyId = p.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.PayrollRepository.Attach(PayrollGrade);
                    _hrUnitOfWork.PayrollRepository.Entry(PayrollGrade).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult GetPoints()
        {
            var query = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("GradePoints", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Accounts
        public ActionResult AccountIndex()
        {
            var Code = _hrUnitOfWork.Repository<PayrollGrade>().DefaultIfEmpty().Max(a => a == null ? 0 : a.Code);
            ViewBag.Code = Code + 1;
            return View();
        }
        public ActionResult ReadAccount(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadAccount(CompanyId);
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
        public ActionResult CreateAccount(IEnumerable<AccountViewModel> models)
        {
            var result = new List<Account>();

            var datasource = new DataSource<AccountViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Accounts",
                        TableName = "Accounts",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (AccountViewModel model in models)
                {
                    var account = new Account();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = account,
                        Source = model,
                        ObjectName = "Accounts",
                        Transtype = TransType.Insert
                    });
                    account.CreatedUser = UserName;
                    account.CreatedTime = DateTime.Now;
                    account.CompanyId = CompanyId;
                    result.Add(account);
                    _hrUnitOfWork.PayrollRepository.Add(account);

                }


                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
        }

            datasource.Data = (from a in models
                               join r in result on a.Id equals r.Id
                               select new AccountViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   AccType = a.AccType,
                                   Name = a.Name,
                                   IsLocal = a.IsLocal,
                                   Code = a.Code,
                                   EndDate = a.EndDate,
                                   StartDate = a.StartDate,
                                   Segment1 = a.Segment1,
                                   Segment2 = a.Segment2,
                                   Segment3 = a.Segment3,
                                   Segment4 = a.Segment4,
                                   Segment5 = a.Segment5,
                                   Segment6 = a.Segment6,
                                   Segment7 = a.Segment7,
                                   Segment8 = a.Segment8,
                                   Segment9 = a.Segment9,
                                   Segment10 = a.Segment10,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateAccount(IEnumerable<AccountViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<AccountViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Accounts",
                        TableName = "Accounts",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                   
                }
                var ids = models.Select(a => a.Id);
                var db_Account = _hrUnitOfWork.Repository<Account>().Where(a => ids.Contains(a.Id)).ToList();
                foreach (AccountViewModel model in models)
                {
                   var account = db_Account.FirstOrDefault(s => s.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = account,
                        Source = model,
                        ObjectName = "Accounts",
                        Transtype = TransType.Update
                    });
                    account.CompanyId = CompanyId;
                    account.ModifiedUser = UserName;
                    account.ModifiedTime = DateTime.Now;   
                    _hrUnitOfWork.PayrollRepository.Attach(account);
                    _hrUnitOfWork.PayrollRepository.Entry(account).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeleteAccount(int Id)
        {
            var datasource = new DataSource<AccountViewModel>();

            var Obj = _hrUnitOfWork.Repository<Account>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Accounts",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.PayrollRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion

    }
}