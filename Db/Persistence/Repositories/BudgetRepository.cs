using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Collections.Generic;


namespace Db.Persistence.Repositories
{
    class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        public BudgetRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        #region BudgetRegion  By Seham


        public void Add(PeriodName calendar)
        {
            context.PeriodNames.Add(calendar);
        }
        public void Attach(PeriodName calendar)
        {
            context.PeriodNames.Attach(calendar);
        }
        public DbEntityEntry<PeriodName> Entry(PeriodName calendar)
        {
            return Context.Entry(calendar);
        }
        public DbEntityEntry<Budget> Entry(Budget budget)
        {
            return Context.Entry(budget);
        }
        public void Add(CompanyBudget budgetItem)
        {
            context.CompanyBudgets.Add(budgetItem);
        }
        public void Attach(CompanyBudget budgetItem)
        {
            context.CompanyBudgets.Attach(budgetItem);
        }
        public DbEntityEntry<CompanyBudget> Entry(CompanyBudget budgetItem)
        {
            return Context.Entry(budgetItem);
        }

        public IQueryable<BudgetViewModel> GetBudgets(int companyId)
        {
            var Budget = from b in context.Budgets
                         where (b.CompanyId == companyId)
                         select new BudgetViewModel
                         {
                             Id = b.Id,
                             Name = b.Name,
                             Amount = (int)b.Amount,
                             PeriodId = b.PeriodId,
                             CompanyId = b.CompanyId
                         };
            return Budget;

        }

        public void Remove(CompanyBudget BudgetItem)
        {
            if (Context.Entry(BudgetItem).State == EntityState.Detached)
            {
                context.CompanyBudgets.Attach(BudgetItem);
            }
            context.CompanyBudgets.Remove(BudgetItem);
        }
        public void Remove(PeriodName calendar)
        {
            if (Context.Entry(calendar).State == EntityState.Detached)
            {
                context.PeriodNames.Attach(calendar);
            }
            context.PeriodNames.Remove(calendar);
        }
        #endregion
        #region BudgetItems
        public IQueryable<BudgetItemViewModel> GetBudgetsItems()
        {
            return context.BudgetItems.Select(a => new BudgetItemViewModel
            {
                Code = a.Code,
                CreatedTime = a.CreatedTime,
                CreatedUser = a.CreatedUser,
                Id = a.Id,
                ModifiedTime = a.ModifiedTime,
                ModifiedUser = a.ModifiedUser,
                Name = a.Name
            });
        }
        public void Attach(BudgetItem BudgetItem)
        {
            context.BudgetItems.Attach(BudgetItem);
        }
        public DbEntityEntry<BudgetItem> Entry(BudgetItem BudgetItem)
        {
            return Context.Entry(BudgetItem);
        }
        public void Remove(BudgetItem BudgetItem)
        {
            if (Context.Entry(BudgetItem).State == EntityState.Detached)
            {
                context.BudgetItems.Attach(BudgetItem);
            }
            context.BudgetItems.Remove(BudgetItem);
        }
        public void Add(BudgetItem BudgetItem)
        {
            context.BudgetItems.Add(BudgetItem);
        }
        public BudgetItem FindBudgetItem(int Id)
        {
            return context.BudgetItems.FirstOrDefault(a => a.Id == Id);
        }
        #endregion
        #region Budget
        public IEnumerable<BudgetViewModel> CompanyBudget(int CompanyId, int periodId)
        {
            var query = (from b in context.BudgetItems
                         join bc in context.CompanyBudgets on b.Id equals bc.BudgetItemId into g
                         from bc in g.Where(a => a.CompanyId == CompanyId && a.PeriodId == periodId).DefaultIfEmpty()
                         select new BudgetViewModel
                         {
                             BudgetItemName = b.Name,
                             Amount = bc != null ? bc.Amount : 0,
                             Id = bc != null ? bc.Id : 0,
                             BudgetItemId = b.Id,
                             SubPeriodName = bc.SubPeriod.Name,
                             PeriodId = bc.SubPeriodId
                         }).ToList();
            return query;

        }
        #endregion
        #region Period region by Mamdouh
        public IQueryable<PeriodsViewModel> GetPeriods(int Id)
        {

            var period = (from p in context.Periods
                          where p.CalendarId == Id
                          select new PeriodsViewModel
                          {
                              Id = p.Id,
                              Name = p.Name,
                              EndDate = p.EndDate,
                              StartDate = p.StartDate,
                              PeriodNo = p.PeriodNo,
                              CalendarId = p.CalendarId,
                              Status = p.Status
                          }).OrderByDescending(a => a.EndDate);
            return period;
        }
        public IQueryable<HRCalendarViewModel> GetCalender(int companyId)
        {
            var calendar = from c in context.PeriodNames
                           where (c.CompanyId == companyId || c.IsLocal == false)
                           select new HRCalendarViewModel
                           {
                               Id = c.Id,
                               CompanyId = c.CompanyId,
                               Name = c.Name,
                               StartDate = c.StartDate,
                               EndDate = c.EndDate == new System.DateTime(2999, 1, 1) ? null : c.EndDate,
                               IsLocal = c.IsLocal,
                               PeriodLength = c.PeriodLength,
                               SubPeriodCount = c.SubPeriodCount,
                               Default = c.SingleYear,
                           };

            return calendar;
        }
        public IQueryable<HRCalendarViewModel> GetCalender(int companyId,bool flag)
        {

            var calendar = from c in context.PeriodNames
                           where (((c.IsLocal && c.CompanyId == companyId) || c.IsLocal == false) && c.SingleYear == flag)
                           select new HRCalendarViewModel
                           {
                               Id = c.Id,
                               CompanyId = c.CompanyId,
                               Name = c.Name,
                               StartDate = c.StartDate,
                               EndDate = c.EndDate == new System.DateTime(2999, 1, 1) ? null : c.EndDate,
                               IsLocal = c.IsLocal,
                               PeriodLength = c.PeriodLength,
                               SubPeriodCount = c.SubPeriodCount,
                               Default = c.SingleYear,
                           };

            return calendar;
        }

        public IQueryable<SubPeriodsViewModel> GetSubPeriods(int Id)
        {

            var subperiod = from s in context.SubPeriods
                            where s.PeriodId == Id
                            select new SubPeriodsViewModel
                            {
                                Id = s.Id,
                                Name = s.Name,
                                EndDate = s.EndDate,
                                StartDate = s.StartDate,
                                SubPeriodNo = s.SubPeriodNo,
                                PeriodId = s.PeriodId,
                                Status = s.Status,
                                CalcSalaryDate = s.CalcSalaryDate,
                                PayDueId = s.PayDueId,
                                PaySalaryDate = s.PaySalaryDate

                            };

            return subperiod;
        }

        public string GenerateAccuralPeriods(PeriodName calendar, string UserName, string culture)
        {
            int subPeriodCount = calendar.SubPeriodCount;
            var PeriodNo = context.Periods.Where(a => a.Name == calendar.Name).DefaultIfEmpty().Max(a => a == null ? 0 : a.PeriodNo);
            if (calendar.SingleYear == true && calendar.PeriodLength == 1)
            {
                var ListOfFiscalYear = context.FiscalYears.ToList();

                if (ListOfFiscalYear.Count == 0)
                    return MsgUtils.Instance.Trls(culture, "FiscalYearnotfound");
                else
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

                        var subPeriodNo1 = context.SubPeriods.Where(a => a.StartDate == Fiscalperiod.StartDate).DefaultIfEmpty().Max(a => a == null ? 0 : a.SubPeriodNo);
                        var enddate1 = Fiscalperiod.EndDate.AddDays(1);
                        context.Periods.Add(Fiscalperiod);
                        if (subPeriodCount > 0)
                        {
                            DateTime startTime = ListOfFiscalYear[f].StartDate;
                            DateTime endTime = Fiscalperiod.StartDate;
                            int num = enddate1.Year - Fiscalperiod.StartDate.Year;
                            int Addmonth = num * 12 / subPeriodCount;

                            for (int i = 1; i <= subPeriodCount; i++)
                            {
                                if (subPeriodCount == 1 || subPeriodCount == 2 || subPeriodCount == 3 || subPeriodCount == 4 || subPeriodCount == 6 || subPeriodCount == 12)
                                {
                                    startTime = Fiscalperiod.StartDate.AddMonths(Addmonth * (i - 1));
                                    endTime = Fiscalperiod.StartDate.AddMonths(Addmonth * i).AddDays(-1);
                                }
                                else if (subPeriodCount == 26)
                                {
                                    startTime = Fiscalperiod.StartDate.AddDays(14 * (i - 1));
                                    endTime = Fiscalperiod.StartDate.AddDays(14 * i).AddDays(-1);
                                }
                                else if (subPeriodCount == 52)
                                {
                                    startTime = Fiscalperiod.StartDate.AddDays(7 * (i - 1));
                                    endTime = Fiscalperiod.StartDate.AddDays(7 * i).AddDays(-1);
                                }
                                else if (subPeriodCount == 24)
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

                                context.SubPeriods.Add(subPeriod);

                            }

                        }
                    }

                }
            }
            return null;
        }
        #endregion
        #region Fiscal Year
        public IQueryable<FiscalYearViewModel> ReadFiscal()
        {
            var fiscal = (from p in context.FiscalYears
                          select new FiscalYearViewModel
                          {
                              Id = p.Id,
                              Name = p.Name,
                              EndDate = (DateTime)p.EndDate,
                              StartDate = p.StartDate,
                              CreatedTime = p.CreatedTime,
                              CreatedUser = p.CreatedUser,
                              ModifiedTime = p.ModifiedTime,
                              ModifiedUser = p.ModifiedUser
                          })
                          .OrderByDescending(a => a.StartDate);

            return fiscal;
        }
        public int? chkLeaveTrans(int id)
        {
            var PeriodId = (from f in context.FiscalYears
                            where f.Id == id
                            join p in context.Periods on f.Id equals p.YearId
                            join t in context.LeaveTrans on p.Id equals t.PeriodId
                            select t.PeriodId).FirstOrDefault();
            return PeriodId;
        }
        public void Add(FiscalYear fiscalYear)
        {
            context.FiscalYears.Add(fiscalYear);
        }
        public FiscalYear Find(int Id)
        {
            return context.FiscalYears.FirstOrDefault(a => a.Id == Id);
        }
        public FiscalYear GetFiscalYear(int? id)
        {
            return context.FiscalYears.Find(id);
        }
        public void Attach(FiscalYear fiscalYear)
        {
            context.FiscalYears.Attach(fiscalYear);
        }
        public DbEntityEntry<FiscalYear> Entry(FiscalYear fiscalYear)
        {
            return Context.Entry(fiscalYear);
        }
        public void Remove(FiscalYear fiscalYear)
        {
            if (Context.Entry(fiscalYear).State == EntityState.Detached)
            {
                context.FiscalYears.Attach(fiscalYear);
            }
            context.FiscalYears.Remove(fiscalYear);
        }
        public void RemoveRange(IEnumerable<Period> entities)
        {
            Context.Set<Period>().RemoveRange(entities);
        }
        public void RemoveRange(IEnumerable<SubPeriod> entities)
        {
            Context.Set<SubPeriod>().RemoveRange(entities);
        }
        #endregion
    }
}
