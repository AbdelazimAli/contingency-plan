using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Interface.Core.Repositories
{
   public interface IBudgetRepository:IRepository<Budget>
    {
        // void Add(Budget budget);
        // void Attach(Budget budget);
        // DbEntityEntry<Budget> Entry(Budget budget);
        IQueryable<HRCalendarViewModel> GetCalender(int companyId);
        void Add(CompanyBudget budgetItem);
        DbEntityEntry<PeriodName> Entry(PeriodName calendar);
        void Attach(CompanyBudget CompanybudgetItem);
        DbEntityEntry<CompanyBudget> Entry(CompanyBudget budgetItem);
        IQueryable<BudgetViewModel> GetBudgets(int companyId);
        #region BudgetItems
        IQueryable<BudgetItemViewModel> GetBudgetsItems();
        void Add(BudgetItem BudgetItem);
        void Attach(BudgetItem BudgetItem);
        DbEntityEntry<BudgetItem> Entry(BudgetItem budgetItem);
        void Remove(BudgetItem BudgetItem);
        BudgetItem FindBudgetItem(int Id);

        #endregion
        #region Budget
        IEnumerable<BudgetViewModel> CompanyBudget(int CompanyId,int periodId);
        #endregion
        void RemoveRange(IEnumerable<SubPeriod> entities);
        void Remove(CompanyBudget CompanyBudgetItem);
        void Remove(PeriodName calendar);
        void RemoveRange(IEnumerable<Period> entities);
        IQueryable<PeriodsViewModel> GetPeriods(int Id);
        IQueryable<SubPeriodsViewModel> GetSubPeriods(int Id);
        IQueryable<HRCalendarViewModel> GetCalender(int companyId,bool flag);
        string GenerateAccuralPeriods(PeriodName calendar, string UserName, string culture);
        void Add(PeriodName calendar);
        void Attach(PeriodName calendar);
        void Add(FiscalYear fiscalYear);

         FiscalYear Find(int Id);

        FiscalYear GetFiscalYear(int? id);
        int? chkLeaveTrans(int id);
        void Attach(FiscalYear fiscalYear);

        DbEntityEntry<FiscalYear> Entry(FiscalYear fiscalYear);

        void Remove(FiscalYear fiscalYear);

        IQueryable<FiscalYearViewModel> ReadFiscal();
    }
}
