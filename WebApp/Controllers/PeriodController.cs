using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using WebApp.Extensions;
using Model.ViewModel;
using System;
using System.Linq.Dynamic;
using WebApp.Models;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class PeriodController : BaseController
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
        public PeriodController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OpenPeriodIndex()
        {
            return View();
        }

        #region CRUD PeriodName
        public ActionResult GetCalender()
        {
            var flag = true;
            var query = _hrUnitOfWork.BudgetRepository.GetCalender(CompanyId,flag);
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetOpenCalender()
        {
            var flag = false;
            var query = _hrUnitOfWork.BudgetRepository.GetCalender(CompanyId, flag);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateCalendar(IEnumerable<HRCalendarViewModel> models)
        {
            var result = new List<PeriodName>();
            var datasource = new DataSource<HRCalendarViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeriodName",
                        TableName = "PeriodNames",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (HRCalendarViewModel c in models)
                {
                    var calendar = new PeriodName
                    {
                        Name = c.Name,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate == null ? new DateTime(2999, 1, 1) : c.EndDate,
                        IsLocal = c.IsLocal,
                        CompanyId = c.IsLocal ? CompanyId : (int?)null,
                        PeriodLength =c.PeriodLength,
                        SubPeriodCount = c.SubPeriodCount,
                        CreatedUser = UserName,
                        CreatedTime = DateTime.Now,
                        SingleYear = c.Default
                    
                    };
                    if (calendar.StartDate > calendar.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndMustGrtStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(calendar);
                    _hrUnitOfWork.BudgetRepository.Add(calendar);
                    var PeriodNo = _hrUnitOfWork.Repository<Period>().Where(a => a.Name == c.Name).DefaultIfEmpty().Max(a => a == null ? 0 : a.PeriodNo);
                    if (calendar.SingleYear==true && calendar.PeriodLength == 1)
                    {
                        var ListOfFiscalYear = _hrUnitOfWork.Repository<FiscalYear>().ToList();
                        if (ListOfFiscalYear.Count > 0)
                        {
                            for (int f = 0; f < ListOfFiscalYear.Count; f++)
                            {
                                var Fiscalperiod = new Period
                                {
                                    Name = ListOfFiscalYear[f].Name,
                                    StartDate = ListOfFiscalYear[f].StartDate,
                                    EndDate = ListOfFiscalYear[f].EndDate == null ? ListOfFiscalYear[f].StartDate.AddYears(1).AddDays(-1) : (DateTime)ListOfFiscalYear[f].EndDate,
                                    PeriodNo = f + 1,
                                    Calendar = calendar,
                                    Status = 1,
                                    CreatedUser = UserName,
                                    CreatedTime = DateTime.Now,
                                    YearId = ListOfFiscalYear[f].Id
                                };

                                var subPeriodNo1 = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.StartDate == Fiscalperiod.StartDate).DefaultIfEmpty().Max(a => a == null ? 0 : a.SubPeriodNo);
                                var enddate1 = Fiscalperiod.EndDate.AddDays(1);
                                _hrUnitOfWork.JobRepository.Add(Fiscalperiod);
                                if (c.SubPeriodCount > 0)
                                {
                                    DateTime startTime = ListOfFiscalYear[f].StartDate;
                                    DateTime endTime = Fiscalperiod.StartDate;
                                    int num = enddate1.Year - Fiscalperiod.StartDate.Year;
                                    int Addmonth = num * 12 / c.SubPeriodCount;

                                    for (int i = 1; i <= c.SubPeriodCount; i++)
                                    {
                                        if (c.SubPeriodCount == 1 || c.SubPeriodCount == 2 || c.SubPeriodCount == 3 || c.SubPeriodCount == 4 || c.SubPeriodCount == 6 || c.SubPeriodCount == 12)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddMonths(Addmonth * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                                        }
                                        else if (c.SubPeriodCount == 26)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(14 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(14 * i).AddDays(-1);
                                        }
                                        else if (c.SubPeriodCount == 52)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(7 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(7 * i).AddDays(-1);
                                        }
                                        else if (c.SubPeriodCount == 24)
                                        {
                                            startTime = i == 1 ? Fiscalperiod.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                                            if (i == 1)
                                                endTime = startTime == endTime ? Fiscalperiod.StartDate.AddDays(15).AddDays(-1) : Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                            if (i > 1)
                                            {
                                                if (i % 2 == 0)
                                                {
                                                    switch (i)
                                                    {
                                                        case 2:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                                            break;
                                                        case 4:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 2).AddDays(-1);
                                                            break;
                                                        case 6:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 3).AddDays(-1);
                                                            break;
                                                        case 8:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 4).AddDays(-1);
                                                            break;
                                                        case 10:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 5).AddDays(-1);
                                                            break;
                                                        case 12:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 6).AddDays(-1);
                                                            break;
                                                        case 14:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 7).AddDays(-1);
                                                            break;
                                                        case 16:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 8).AddDays(-1);
                                                            break;
                                                        case 18:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 9).AddDays(-1);
                                                            break;
                                                        case 20:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 10).AddDays(-1);
                                                            break;
                                                        case 22:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 11).AddDays(-1);
                                                            break;
                                                        case 24:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 12).AddDays(-1);
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                                else if (i % 2 != 0)
                                                {
                                                    endTime = startTime.AddDays(15).AddDays(-1);

                                                }
                                            }
                                        }

                                        SubPeriod subPeriod = new SubPeriod()
                                        {
                                            Name = i + "/" + startTime.ToString("yyyy"),
                                            Period = Fiscalperiod,
                                            StartDate = startTime,
                                            EndDate = endTime,
                                            Status = 1,
                                            CreatedTime = DateTime.Now,
                                            CreatedUser = UserName,
                                            SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo

                                        };

                                        _hrUnitOfWork.JobRepository.Add(subPeriod);

                                    }

                                }
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("FiscalYearnotfound"));
                            datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                            return Json(datasource);

                        }
                    }
                    else
                    {
                        var period = new Period
                        {
                            Name = c.StartDate.ToString("yyyy"),
                            StartDate = c.StartDate,
                            EndDate = c.PeriodLength != 0 ? c.StartDate.AddYears((c.PeriodLength)).AddDays(-1) : new DateTime(2999, 1, 1),
                            PeriodNo = PeriodNo == 0 ? 1 : ++PeriodNo,
                            Calendar = calendar,
                            Status = 1,
                            CreatedUser = UserName,
                            CreatedTime = DateTime.Now
                        };

                        var subPeriodNo = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.StartDate == period.StartDate).DefaultIfEmpty().Max(a => a == null ? 0 : a.SubPeriodNo);
                        var enddate = period.EndDate.AddDays(1);
                        _hrUnitOfWork.JobRepository.Add(period);
                        var date = new DateTime(2999, 1, 1);
                        if (c.SubPeriodCount > 0 && period.EndDate != date)
                        {
                            DateTime startTime = c.StartDate;
                            DateTime endTime = period.StartDate;
                            int num = enddate.Year - period.StartDate.Year;
                            int Addmonth = num * 12 / c.SubPeriodCount;

                            for (int i = 1; i <= c.SubPeriodCount; i++)
                            {
                                if (c.SubPeriodCount == 1 || c.SubPeriodCount == 2 || c.SubPeriodCount == 3 || c.SubPeriodCount == 4 || c.SubPeriodCount == 6 || c.SubPeriodCount == 12)
                                {
                                    startTime = period.StartDate.AddMonths(Addmonth * (i - 1));
                                    endTime = period.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                                }
                                else if (c.SubPeriodCount == 26)
                                {
                                    startTime = period.StartDate.AddDays(14 * (i - 1));
                                    endTime = period.StartDate.AddDays(14 * i).AddDays(-1);
                                }
                                else if (c.SubPeriodCount == 52)
                                {
                                    startTime = period.StartDate.AddDays(7 * (i - 1));
                                    endTime = period.StartDate.AddDays(7 * i).AddDays(-1);
                                }
                                else if (c.SubPeriodCount == 24)
                                {
                                    startTime = i == 1 ? period.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                                    if (i == 1)
                                        endTime = startTime == endTime ? period.StartDate.AddDays(15).AddDays(-1) : period.StartDate.AddMonths(i - 1).AddDays(-1);
                                    if (i > 1)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            switch (i)
                                            {
                                                case 2:
                                                    endTime = period.StartDate.AddMonths(i - 1).AddDays(-1);
                                                    break;
                                                case 4:
                                                    endTime = period.StartDate.AddMonths(i - 2).AddDays(-1);
                                                    break;
                                                case 6:
                                                    endTime = period.StartDate.AddMonths(i - 3).AddDays(-1);
                                                    break;
                                                case 8:
                                                    endTime = period.StartDate.AddMonths(i - 4).AddDays(-1);
                                                    break;
                                                case 10:
                                                    endTime = period.StartDate.AddMonths(i - 5).AddDays(-1);
                                                    break;
                                                case 12:
                                                    endTime = period.StartDate.AddMonths(i - 6).AddDays(-1);
                                                    break;
                                                case 14:
                                                    endTime = period.StartDate.AddMonths(i - 7).AddDays(-1);
                                                    break;
                                                case 16:
                                                    endTime = period.StartDate.AddMonths(i - 8).AddDays(-1);
                                                    break;
                                                case 18:
                                                    endTime = period.StartDate.AddMonths(i - 9).AddDays(-1);
                                                    break;
                                                case 20:
                                                    endTime = period.StartDate.AddMonths(i - 10).AddDays(-1);
                                                    break;
                                                case 22:
                                                    endTime = period.StartDate.AddMonths(i - 11).AddDays(-1);
                                                    break;
                                                case 24:
                                                    endTime = period.StartDate.AddMonths(i - 12).AddDays(-1);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        else if (i % 2 != 0)
                                        {
                                            endTime = startTime.AddDays(15).AddDays(-1);

                                        }
                                    }
                                }

                                SubPeriod subPeriod = new SubPeriod()
                                {
                                    Name = i + "/" + startTime.ToString("yyyy"),
                                    Period = period,
                                    StartDate = startTime,
                                    EndDate = endTime,
                                    Status = 1,
                                    CreatedTime = DateTime.Now,
                                    CreatedUser = UserName,
                                    SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo

                                };

                                _hrUnitOfWork.JobRepository.Add(subPeriod);

                            }

                        }
                    }

                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models
                               join r in result on c.Name equals r.Name
                               select new HRCalendarViewModel
                               {
                                   Id = r.Id,
                                   Name = c.Name,
                                   StartDate = c.StartDate,
                                   EndDate = c.EndDate,
                                   IsLocal = c.IsLocal,
                                   PeriodLength = c.PeriodLength,
                                   SubPeriodCount = c.SubPeriodCount
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateCalendar(IEnumerable<HRCalendarViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<HRCalendarViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeriodName",
                        TableName = "PeriodNames",
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
                var db_PeriodName = _hrUnitOfWork.Repository<PeriodName>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var calendar = db_PeriodName.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new WebApp.Models.AutoMapperParm() {
                        ObjectName = "PeriodName",
                        Destination = calendar,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Update
                    });
                    calendar.CompanyId = models.ElementAtOrDefault(i).IsLocal ? CompanyId : (int?)null;
                    calendar.SingleYear = models.ElementAtOrDefault(i).Default;
                    _hrUnitOfWork.BudgetRepository.Attach(calendar);
                    _hrUnitOfWork.BudgetRepository.Entry(calendar).State = EntityState.Modified;
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
        public ActionResult DeleteCalendar(int Id)
        {
            var datasource = new DataSource<HRCalendarViewModel>();

            var Obj = _hrUnitOfWork.Repository<PeriodName>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "HRCalendar",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.BudgetRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region CRUD Period
        public ActionResult GetPeriods(int Id)
        {
            var query = _hrUnitOfWork.BudgetRepository.GetPeriods(Id);
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CreatePeriods(IEnumerable<PeriodsViewModel> models ,OptionsViewModel options)
        {
            var result = new List<Period>();

            var datasource = new DataSource<PeriodsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Periods",
                        TableName = "Periods",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (PeriodsViewModel p in models)
                {
                    var period = new Period();
                    //{
                    //    Name = p.Name,
                    //    EndDate = p.EndDate,
                    //    PeriodNo = p.PeriodNo,
                    //    StartDate = p.StartDate,
                    //    Status = p.Status,
                    //    CreatedTime = DateTime.Now,
                    //    CalendarId = p.CalendarId,
                    //    CreatedUser = UserName
                    //};
                    AutoMapper(new AutoMapperParm() {
                        ObjectName = "Periods",
                        Destination = period,
                        Source = p,
                        Version = 0,
                        Options = options,
                        Transtype = TransType.Insert
                    });
                    if (period.StartDate > period.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(period);
                    _hrUnitOfWork.JobRepository.Add(period);
                    var c = _hrUnitOfWork.Repository<PeriodName>().Where(a => a.Id == p.CalendarId).FirstOrDefault();
                    var date = new DateTime(2999, 1, 1);
                    var enddate = period.EndDate.AddDays(1);
                    if (c.SubPeriodCount > 0 && period.EndDate != date)
                    {
                        DateTime startTime = c.StartDate;
                        DateTime endTime = period.StartDate;
                        int num = enddate.Year - period.StartDate.Year;
                        int Addmonth = num * 12 / c.SubPeriodCount;
                        for (int i = 1; i <= c.SubPeriodCount; i++)
                        {
                            if (c.SubPeriodCount == 1 || c.SubPeriodCount == 2 || c.SubPeriodCount == 3 || c.SubPeriodCount == 4 || c.SubPeriodCount == 6 || c.SubPeriodCount == 12)
                            {
                                startTime = period.StartDate.AddMonths(Addmonth * (i - 1));
                                endTime = period.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                            }
                            else if (c.SubPeriodCount == 26)
                            {
                                startTime = period.StartDate.AddDays(14 * (i - 1));
                                endTime = period.StartDate.AddDays(14 * i).AddDays(-1);
                            }
                            else if (c.SubPeriodCount == 52)
                            {
                                startTime = period.StartDate.AddDays(7 * (i - 1));
                                endTime = period.StartDate.AddDays(7 * i).AddDays(-1);
                            }
                            else if (c.SubPeriodCount == 24)
                            {
                                startTime = i == 1 ? period.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                                if (i == 1)
                                    endTime = startTime == endTime ? period.StartDate.AddDays(15).AddDays(-1) : period.StartDate.AddMonths(i - 1).AddDays(-1);
                                if (i > 1)
                                {
                                    if (i % 2 == 0)
                                    {
                                        switch (i)
                                        {
                                            case 2:
                                                endTime = period.StartDate.AddMonths(i - 1).AddDays(-1);
                                                break;
                                            case 4:
                                                endTime = period.StartDate.AddMonths(i - 2).AddDays(-1);
                                                break;
                                            case 6:
                                                endTime = period.StartDate.AddMonths(i - 3).AddDays(-1);
                                                break;
                                            case 8:
                                                endTime = period.StartDate.AddMonths(i - 4).AddDays(-1);
                                                break;
                                            case 10:
                                                endTime = period.StartDate.AddMonths(i - 5).AddDays(-1);
                                                break;
                                            case 12:
                                                endTime = period.StartDate.AddMonths(i - 6).AddDays(-1);
                                                break;
                                            case 14:
                                                endTime = period.StartDate.AddMonths(i - 7).AddDays(-1);
                                                break;
                                            case 16:
                                                endTime = period.StartDate.AddMonths(i - 8).AddDays(-1);
                                                break;
                                            case 18:
                                                endTime = period.StartDate.AddMonths(i - 9).AddDays(-1);
                                                break;
                                            case 20:
                                                endTime = period.StartDate.AddMonths(i - 10).AddDays(-1);
                                                break;
                                            case 22:
                                                endTime = period.StartDate.AddMonths(i - 11).AddDays(-1);
                                                break;
                                            case 24:
                                                endTime = period.StartDate.AddMonths(i - 12).AddDays(-1);
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                    else if (i % 2 != 0)
                                    {
                                        endTime = startTime.AddDays(15).AddDays(-1);

                                    }
                                }
                            }


                            SubPeriod subPeriod = new SubPeriod()
                            {
                                Name = i + "/" + startTime.ToString("yyyy"),
                                Period = period,
                                StartDate = startTime,
                                EndDate = endTime,
                                Status = p.Status,
                                CreatedTime = DateTime.Now,
                                CreatedUser = UserName,
                                SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo
                            };
                            _hrUnitOfWork.JobRepository.Add(subPeriod);

                        }

                    }
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.PeriodNo equals r.PeriodNo
                               select new PeriodsViewModel
                               {
                                   Id = r.Id,
                                   PeriodNo = p.PeriodNo,
                                   EndDate = p.EndDate,
                                   Name = p.Name,
                                   StartDate = p.StartDate,
                                   Status = p.Status
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        // create sub period
        public ActionResult Updateperiod(IEnumerable<PeriodsViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PeriodsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Periods",
                        TableName = "Periods",
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
                var db_Period = _hrUnitOfWork.Repository<Period>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var period = db_Period.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new WebApp.Models.AutoMapperParm() {
                        ObjectName = "Periods",
                        Destination = period,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Update });
                    _hrUnitOfWork.JobRepository.Attach(period);
                    _hrUnitOfWork.JobRepository.Entry(period).State = EntityState.Modified;
                }
                var PeriodElement = models.Select(a => new { id = a.Id, status = a.Status }).FirstOrDefault();
                var ListOfSubPeriod = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.PeriodId == PeriodElement.id).ToList();
                foreach (var item in ListOfSubPeriod)
                {
                    if (PeriodElement.status != item.Status)
                    {
                        item.Status = PeriodElement.status;
                        _hrUnitOfWork.JobRepository.Attach(item);
                        _hrUnitOfWork.JobRepository.Entry(item).State = EntityState.Modified;
                    }
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
      
        public ActionResult DeletePeriod(int Id)
        {
            var datasource = new DataSource<PeriodsViewModel>();

            var Obj = _hrUnitOfWork.Repository<Period>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Periods",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.JobRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region CRUD SubPeriod
        public ActionResult GetSubPeriods(int Id)
        {
            var query = _hrUnitOfWork.BudgetRepository.GetSubPeriods(Id);

            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CreateSubPeriods(IEnumerable<SubPeriodsViewModel> models ,OptionsViewModel options)
        {
            var result = new List<SubPeriod>();

            var datasource = new DataSource<SubPeriodsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SubPeriods",
                        TableName = "SubPeriods",
                        ParentColumn = "PeriodId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (SubPeriodsViewModel sub in models)
                {

                    var Subperiod = new SubPeriod();
                    //{
                    //    Name = sub.Name,
                    //    SubPeriodNo = sub.SubPeriodNo,
                    //    PeriodId = sub.PeriodId,
                    //    StartDate = sub.StartDate,
                    //    EndDate = sub.EndDate,
                    //    CreatedUser = UserName,
                    //    CreatedTime = DateTime.Now

                    //};
                    AutoMapper(new AutoMapperParm()
                    {
                        ObjectName = "SubPeriods",
                        Destination = Subperiod,
                        Source = sub,
                        Version = 0,
                        Options = options,
                        Transtype = TransType.Insert
                    });

                    if (Subperiod.StartDate > Subperiod.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(Subperiod);
                    _hrUnitOfWork.JobRepository.Add(Subperiod);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from s in models
                               join r in result on s.PeriodId equals r.PeriodId
                               select new SubPeriodsViewModel
                               {
                                   Id = r.Id,
                                   EndDate = s.EndDate,
                                   StartDate = s.StartDate,
                                   Name = s.Name,
                                   SubPeriodNo = s.SubPeriodNo,
                                   PeriodId = s.PeriodId

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateSubperiod(IEnumerable<SubPeriodsViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<SubPeriodsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SubPeriods",
                        TableName = "SubPeriods",
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
                var db_subPeriod = _hrUnitOfWork.Repository<SubPeriod>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var period = db_subPeriod.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new WebApp.Models.AutoMapperParm() {
                        ObjectName = "SubPeriods",
                        Destination = period,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Update
                    });
                    _hrUnitOfWork.JobRepository.Attach(period);
                    _hrUnitOfWork.JobRepository.Entry(period).State = EntityState.Modified;
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
        public ActionResult DeleteSubPeriod(int Id)
        {
            var datasource = new DataSource<SubPeriodsViewModel>();
            var Obj = _hrUnitOfWork.Repository<SubPeriod>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "SubPeriods",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.JobRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");

        }

        #endregion

        #region CRUD FiscalYeasr
        public ActionResult FiscalIndex()
        {
            ViewBag.count = _hrUnitOfWork.Repository<FiscalYear>().Count();
            return View();
        }
        public ActionResult FiscalYearIndex()
        {
            ViewBag.count = _hrUnitOfWork.Repository<FiscalYear>().Count();
            return View();
        }


        public ActionResult ReadFiscal()
        {           
            var query = _hrUnitOfWork.BudgetRepository.ReadFiscal();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //CreateFiscal
        public ActionResult CreateFiscal(IEnumerable<FiscalYearViewModel> models , OptionsViewModel moreInfo)
        {
            var result = new List<FiscalYear>();
            var datasource = new DataSource<FiscalYearViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FiscalYear",
                        TableName = "FiscalYears",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (FiscalYearViewModel f in models)
                {
                    var fiscal = new FiscalYear();
                    
                    AutoMapper(new AutoMapperParm() {
                        ObjectName = "FiscalYear",
                        Destination = fiscal,
                        Source = f,
                        Version = 0,
                        Options = moreInfo,
                        Transtype = TransType.Insert });

                    if (fiscal.StartDate > fiscal.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(fiscal);
                    _hrUnitOfWork.BudgetRepository.Add(fiscal);

                    var PeriodName = _hrUnitOfWork.Repository<PeriodName>().Where(a => a.SingleYear && a.PeriodLength == 1 && ((a.IsLocal && a.CompanyId == CompanyId) || a.IsLocal == false)).ToList();
                    if (PeriodName.Count > 0)
                    {
                        foreach (var calendar in PeriodName)
                        {

                            var period = _hrUnitOfWork.Repository<Period>().Where(a => a.EndDate > f.StartDate && a.CalendarId == calendar.Id).FirstOrDefault();
                            if (period == null)
                            {

                                var PeriodNo = _hrUnitOfWork.Repository<Period>().Where(a => a.CalendarId == calendar.Id).DefaultIfEmpty().Max(a => a == null ? 0 : a.PeriodNo);

                                var Fiscalperiod = new Period
                                {
                                    Name = f.Name,
                                    StartDate = f.StartDate,
                                    EndDate = f.EndDate == null ? f.StartDate.AddYears(1).AddDays(-1) : (DateTime)f.EndDate,
                                    PeriodNo = PeriodNo + 1,
                                    CalendarId = calendar.Id,
                                    Status = 1,
                                    CreatedUser = UserName,
                                    CreatedTime = DateTime.Now,
                                    FiscalYear = fiscal
                                };

                                var subPeriodNo1 = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.StartDate == Fiscalperiod.StartDate).DefaultIfEmpty().Max(a => a == null ? 0 : a.SubPeriodNo);
                                var enddate1 = Fiscalperiod.EndDate.AddDays(1);
                                _hrUnitOfWork.JobRepository.Add(Fiscalperiod);
                                if (calendar.SubPeriodCount > 0)
                                {
                                    DateTime startTime = f.StartDate;
                                    DateTime endTime = Fiscalperiod.StartDate;
                                    int num = enddate1.Year - Fiscalperiod.StartDate.Year;
                                    int Addmonth = num * 12 / calendar.SubPeriodCount;

                                    for (int i = 1; i <= calendar.SubPeriodCount; i++)
                                    {
                                        if (calendar.SubPeriodCount == 1 || calendar.SubPeriodCount == 2 || calendar.SubPeriodCount == 3 || calendar.SubPeriodCount == 4 || calendar.SubPeriodCount == 6 || calendar.SubPeriodCount == 12)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddMonths(Addmonth * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 26)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(14 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(14 * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 52)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(7 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(7 * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 24)
                                        {
                                            startTime = i == 1 ? Fiscalperiod.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                                            if (i == 1)
                                                endTime = startTime == endTime ? Fiscalperiod.StartDate.AddDays(15).AddDays(-1) : Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                            if (i > 1)
                                            {
                                                if (i % 2 == 0)
                                                {
                                                    switch (i)
                                                    {
                                                        case 2:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                                            break;
                                                        case 4:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 2).AddDays(-1);
                                                            break;
                                                        case 6:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 3).AddDays(-1);
                                                            break;
                                                        case 8:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 4).AddDays(-1);
                                                            break;
                                                        case 10:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 5).AddDays(-1);
                                                            break;
                                                        case 12:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 6).AddDays(-1);
                                                            break;
                                                        case 14:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 7).AddDays(-1);
                                                            break;
                                                        case 16:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 8).AddDays(-1);
                                                            break;
                                                        case 18:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 9).AddDays(-1);
                                                            break;
                                                        case 20:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 10).AddDays(-1);
                                                            break;
                                                        case 22:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 11).AddDays(-1);
                                                            break;
                                                        case 24:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 12).AddDays(-1);
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                                else if (i % 2 != 0)
                                                {
                                                    endTime = startTime.AddDays(15).AddDays(-1);

                                                }
                                            }
                                        }

                                        SubPeriod subPeriod = new SubPeriod()
                                        {
                                            Name = i + "/" + startTime.ToString("yyyy"),
                                            Period = Fiscalperiod,
                                            StartDate = startTime,
                                            EndDate = endTime,
                                            Status = 1,
                                            CreatedTime = DateTime.Now,
                                            CreatedUser = UserName,
                                            SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo

                                        };

                                        _hrUnitOfWork.JobRepository.Add(subPeriod);

                                    }

                                }

                            }
                        }
                    }
                
            }
        

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from f in models
                               join r in result on f.StartDate equals r.StartDate
                               select new FiscalYearViewModel
                               {
                                   Id = r.Id,
                                   EndDate = f.EndDate,
                                   Name = f.Name,
                                   StartDate = f.StartDate,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateFiscal(IEnumerable<FiscalYearViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<FiscalYearViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FiscalYear",
                        TableName = "FiscalYears",
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
                var db_FiscalYear = _hrUnitOfWork.Repository<FiscalYear>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var fiscal = db_FiscalYear.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new WebApp.Models.AutoMapperParm() {
                        ObjectName = "FiscalYear",
                        Destination = fiscal,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i)
                    });
                    _hrUnitOfWork.BudgetRepository.Attach(fiscal);
                    _hrUnitOfWork.BudgetRepository.Entry(fiscal).State = EntityState.Modified;
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
        public ActionResult DeleteFiscal(int Id)
        {
            List<Error> errors = new List<Error>();
            var datasource = new DataSource<FiscalYearViewModel>();
            var PeriodId = _hrUnitOfWork.BudgetRepository.chkLeaveTrans(Id);
            if (PeriodId == 0)
            {
                var Obj = _hrUnitOfWork.Repository<FiscalYear>().FirstOrDefault(k => k.Id == Id);
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = Obj,
                    ObjectName = "FiscalYear",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.BudgetRepository.Remove(Obj);
                var listOfPeriods = _hrUnitOfWork.Repository<Period>().Where(a => a.YearId == Id).ToList();
                var ListOfPeroidIds = _hrUnitOfWork.Repository<Period>().Where(a => a.YearId == Id).Select(a => a.Id).ToList();
                if (listOfPeriods.Count > 0)
                {
                    _hrUnitOfWork.BudgetRepository.RemoveRange(listOfPeriods);
                    var ListOfSubPeriod=  _hrUnitOfWork.Repository<SubPeriod>().Where(a => ListOfPeroidIds.Contains(a.PeriodId)).ToList();
                    if (ListOfSubPeriod.Count > 0)
                        _hrUnitOfWork.BudgetRepository.RemoveRange(ListOfSubPeriod);
                }

            }else
            {
                errors.Add(new Error() { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("cantdeleteFiscal") } } });
                datasource.Errors = errors;
                return Json(datasource);
            }
            string message = "OK";
            datasource.Errors = SaveChanges(Language);
            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json(message);
        }
        public ActionResult AddFiscal(string name , string date)
        {
            string message = "Ok";
            var StartDate = DateTime.Now;
            if(date == "" || date == null)
            {
                DateTime? End = _hrUnitOfWork.Repository<FiscalYear>().Select(a => a.EndDate).OrderByDescending(a => a).FirstOrDefault();
                StartDate = End.Value.AddDays(1);
            }
            else
            StartDate = DateTime.Parse(date);
            var EndDate = StartDate.AddYears(1).AddDays(-1);
                var fiscal = new FiscalYear()
                {
                    Name = name,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    CreatedUser = UserName,
                    CreatedTime = DateTime.Now
                };          
                    var Newfiscal = new FiscalYear();

                    AutoMapper(new AutoMapperParm()
                    {
                        ObjectName = "FiscalYear",
                        Destination = Newfiscal,
                        Source = fiscal,
                        Version = 0,
                        Transtype = TransType.Insert
                    });
                    _hrUnitOfWork.BudgetRepository.Add(fiscal);

                    var PeriodName = _hrUnitOfWork.Repository<PeriodName>().Where(a => a.SingleYear && a.PeriodLength == 1 && ((a.IsLocal && a.CompanyId == CompanyId) || a.IsLocal == false)).ToList();
                    if (PeriodName.Count > 0)
                    {
                        foreach (var calendar in PeriodName)
                        {

                            var period = _hrUnitOfWork.Repository<Period>().Where(a => a.EndDate > StartDate && a.CalendarId == calendar.Id).FirstOrDefault();
                            if (period == null)
                            {

                                var PeriodNo = _hrUnitOfWork.Repository<Period>().Where(a => a.CalendarId == calendar.Id).DefaultIfEmpty().Max(a => a == null ? 0 : a.PeriodNo);

                                var Fiscalperiod = new Period
                                {
                                    Name = name,
                                    StartDate = StartDate,
                                    EndDate = EndDate,
                                    PeriodNo = PeriodNo + 1,
                                    CalendarId = calendar.Id,
                                    Status = 1,
                                    CreatedUser = UserName,
                                    CreatedTime = DateTime.Now,
                                    FiscalYear = fiscal
                                };

                                var subPeriodNo1 = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.StartDate == Fiscalperiod.StartDate).DefaultIfEmpty().Max(a => a == null ? 0 : a.SubPeriodNo);
                                var enddate1 = Fiscalperiod.EndDate.AddDays(1);
                                _hrUnitOfWork.JobRepository.Add(Fiscalperiod);
                                if (calendar.SubPeriodCount > 0)
                                {
                                    DateTime startTime = StartDate;
                                    DateTime endTime = Fiscalperiod.StartDate;
                                    int num = enddate1.Year - Fiscalperiod.StartDate.Year;
                                    int Addmonth = num * 12 / calendar.SubPeriodCount;

                                    for (int i = 1; i <= calendar.SubPeriodCount; i++)
                                    {
                                        if (calendar.SubPeriodCount == 1 || calendar.SubPeriodCount == 2 || calendar.SubPeriodCount == 3 || calendar.SubPeriodCount == 4 || calendar.SubPeriodCount == 6 || calendar.SubPeriodCount == 12)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddMonths(Addmonth * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 26)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(14 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(14 * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 52)
                                        {
                                            startTime = Fiscalperiod.StartDate.AddDays(7 * (i - 1));
                                            endTime = Fiscalperiod.StartDate.AddDays(7 * i).AddDays(-1);
                                        }
                                        else if (calendar.SubPeriodCount == 24)
                                        {
                                            startTime = i == 1 ? Fiscalperiod.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                                            if (i == 1)
                                                endTime = startTime == endTime ? Fiscalperiod.StartDate.AddDays(15).AddDays(-1) : Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                            if (i > 1)
                                            {
                                                if (i % 2 == 0)
                                                {
                                                    switch (i)
                                                    {
                                                        case 2:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 1).AddDays(-1);
                                                            break;
                                                        case 4:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 2).AddDays(-1);
                                                            break;
                                                        case 6:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 3).AddDays(-1);
                                                            break;
                                                        case 8:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 4).AddDays(-1);
                                                            break;
                                                        case 10:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 5).AddDays(-1);
                                                            break;
                                                        case 12:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 6).AddDays(-1);
                                                            break;
                                                        case 14:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 7).AddDays(-1);
                                                            break;
                                                        case 16:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 8).AddDays(-1);
                                                            break;
                                                        case 18:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 9).AddDays(-1);
                                                            break;
                                                        case 20:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 10).AddDays(-1);
                                                            break;
                                                        case 22:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 11).AddDays(-1);
                                                            break;
                                                        case 24:
                                                            endTime = Fiscalperiod.StartDate.AddMonths(i - 12).AddDays(-1);
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                                else if (i % 2 != 0)
                                                {
                                                    endTime = startTime.AddDays(15).AddDays(-1);

                                                }
                                            }
                                        }

                                        SubPeriod subPeriod = new SubPeriod()
                                        {
                                            Name = i + "/" + startTime.ToString("yyyy"),
                                            Period = Fiscalperiod,
                                            StartDate = startTime,
                                            EndDate = endTime,
                                            Status = 1,
                                            CreatedTime = DateTime.Now,
                                            CreatedUser = UserName,
                                            SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo

                                        };

                                        _hrUnitOfWork.JobRepository.Add(subPeriod);

                                    }

                                }

                            }
                        }
                    }
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Distribute Button
        public ActionResult DistributeSubPeriods(int id,int CalendarId)
        {
            string message = "Ok";
            //check if there is a List of sub Period related to this Period
            var ListOfSubPeriod = _hrUnitOfWork.Repository<SubPeriod>().Where(a => a.PeriodId == id).ToList();
            if(ListOfSubPeriod.Count > 0)
            {
                //check if status of any record in sub period list is closed 
                bool has = ListOfSubPeriod.Any(l => l.Status == 2);
                if( has == true)
                {
                    return Json("NotOk",JsonRequestBehavior.AllowGet);
                }else
                {
                    _hrUnitOfWork.BudgetRepository.RemoveRange(ListOfSubPeriod);
                }
            }
            // Add New Distributed Sub Period 
            var period = _hrUnitOfWork.Repository<Period>().Where(a => a.Id == id).FirstOrDefault();
            var c = _hrUnitOfWork.Repository<PeriodName>().Where(a => a.Id == CalendarId).FirstOrDefault();
            var date = new DateTime(2999, 1, 1);
            var enddate = period.EndDate.AddDays(1);
            if(period.EndDate == date)
            {
                return Json("EndDateIsOpen", JsonRequestBehavior.AllowGet);
            }
            if(c.SubPeriodCount == 0)
            {
                return Json("ZeroSubPeriod", JsonRequestBehavior.AllowGet);
            }
            if (c.SubPeriodCount > 0 && period.EndDate != date)
            {
                DateTime startTime = c.StartDate;
                DateTime endTime = period.StartDate;
                int num = enddate.Year - period.StartDate.Year;
                int Addmonth = num * 12 / c.SubPeriodCount;
                for (int i = 1; i <= c.SubPeriodCount; i++)
                {
                    if (c.SubPeriodCount == 1 || c.SubPeriodCount == 2 || c.SubPeriodCount == 3 || c.SubPeriodCount == 4 || c.SubPeriodCount == 6 || c.SubPeriodCount == 12)
                    {
                        startTime = period.StartDate.AddMonths(Addmonth * (i - 1));
                        endTime = period.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                    }
                    else if (c.SubPeriodCount == 26)
                    {
                        startTime = period.StartDate.AddDays(14 * (i - 1));
                        endTime = period.StartDate.AddDays(14 * i).AddDays(-1);
                    }
                    else if (c.SubPeriodCount == 52)
                    {
                        startTime = period.StartDate.AddDays(7 * (i - 1));
                        endTime = period.StartDate.AddDays(7 * i).AddDays(-1);
                    }
                    else if (c.SubPeriodCount == 24)
                    {
                        startTime = i == 1 ? period.StartDate.AddDays(7 * (i - 1)) : endTime.AddDays(1);
                        if (i == 1)
                            endTime = startTime == endTime ? period.StartDate.AddDays(15).AddDays(-1) : period.StartDate.AddMonths(i - 1).AddDays(-1);
                        if (i > 1)
                        {
                            if (i % 2 == 0)
                            {
                                switch (i)
                                {
                                    case 2:
                                        endTime = period.StartDate.AddMonths(i - 1).AddDays(-1);
                                        break;
                                    case 4:
                                        endTime = period.StartDate.AddMonths(i - 2).AddDays(-1);
                                        break;
                                    case 6:
                                        endTime = period.StartDate.AddMonths(i - 3).AddDays(-1);
                                        break;
                                    case 8:
                                        endTime = period.StartDate.AddMonths(i - 4).AddDays(-1);
                                        break;
                                    case 10:
                                        endTime = period.StartDate.AddMonths(i - 5).AddDays(-1);
                                        break;
                                    case 12:
                                        endTime = period.StartDate.AddMonths(i - 6).AddDays(-1);
                                        break;
                                    case 14:
                                        endTime = period.StartDate.AddMonths(i - 7).AddDays(-1);
                                        break;
                                    case 16:
                                        endTime = period.StartDate.AddMonths(i - 8).AddDays(-1);
                                        break;
                                    case 18:
                                        endTime = period.StartDate.AddMonths(i - 9).AddDays(-1);
                                        break;
                                    case 20:
                                        endTime = period.StartDate.AddMonths(i - 10).AddDays(-1);
                                        break;
                                    case 22:
                                        endTime = period.StartDate.AddMonths(i - 11).AddDays(-1);
                                        break;
                                    case 24:
                                        endTime = period.StartDate.AddMonths(i - 12).AddDays(-1);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else if (i % 2 != 0)
                            {
                                endTime = startTime.AddDays(15).AddDays(-1);

                            }
                        }
                    }


                    SubPeriod subPeriod = new SubPeriod()
                    {
                        Name = i + "/" + startTime.ToString("yyyy"),
                        Period = period,
                        StartDate = startTime,
                        EndDate = endTime,
                        Status = period.Status,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName,
                        SubPeriodNo = i//subPeriodNo == 0 ? i : ++subPeriodNo
                    };
                    _hrUnitOfWork.JobRepository.Add(subPeriod);

                }

            }
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message, JsonRequestBehavior.AllowGet);
        }      
    }

        #endregion
    }

