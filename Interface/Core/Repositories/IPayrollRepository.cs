using Model.ViewModel.Payroll;
using Model.ViewModel.Personnel;
using Model.Domain.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
namespace Interface.Core.Repositories
{
    public interface IPayrollRepository : IRepository<Payrolls>
    {
        void Add(PayrollSetup payroll);
        void Add(PayrollGrade payrollGrade);

        void Attach(PayrollSetup payroll);

        DbEntityEntry<PayrollSetup> Entry(PayrollSetup payroll);

        void Remove(PayrollSetup payroll);
        IList<PayrollGradesViewModel> ReadPayrollGrades(string culture, int CompanyId);
        IQueryable<PayrollViewModel> GetPayrolls(int CompanyId);
        PayrollFormViewModel GetPayroll(int Id);
        IQueryable<PayrollDueViewModel> readPayDue(int PayrollId);
        void Add(PayrollDue PayrollDue);
        void Attach(PayrollDue PayrollDue);
        DbEntityEntry<PayrollDue> Entry(PayrollDue PayrollDue);
        void Remove(PayrollDue PayrollDue);
        void Attach(PayrollGrade payroll);

        DbEntityEntry<PayrollGrade> Entry(PayrollGrade payroll);

        void Remove(PayrollGrade payroll);
        IQueryable<AccountSetupViewModel> readAccountSetup(int accTypId, int companyId);
        void Add(AccountSetup account);
        
        IQueryable<SalaryItemViewModel> GetSalaryItems(int CompanyId);
        SalaryItemFormViewModel GetSalaryItem(int Id);
        void Add(SalaryItem salaryItem);
        void Attach(SalaryItem salaryItem);
        DbEntityEntry<SalaryItem> Entry(SalaryItem salaryItem);
        void Remove(SalaryItem salaryItem);
        SalaryItem GetSalary(int? id);
        void Attach(AccountSetup account);
        DbEntityEntry<AccountSetup> Entry(AccountSetup account);
        void Remove(AccountSetup account);
        void Add(Account account);
       
        void Attach(Account account);
       
        DbEntityEntry<Account> Entry(Account account);
       
        void Remove(Account account);
        IQueryable<AccountViewModel> ReadAccount(int companyId);

        #region Pay Request
        IQueryable<PayReqGridViewModel> ReadPayRequestsGrid(int companyId, string culture);
        int NextRequestNo(int companyId);
        PayRequestViewModel GetPayRequestVM(int requestId, string culture);
        PayRequest GetRequest(int Id);
        IQueryable<PayRequestEmpsViewModel> GetDeptsEmp(int requestId, string Emps, string Depts, int companyId, string culture);

        void Add(PayRequest PayRequest);
        void Attach(PayRequest PayRequest);
        DbEntityEntry<PayRequest> Entry(PayRequest PayRequest);
        void Remove(PayRequest PayRequest);
        void Add(PayRequestDet PayRequestDet);
        void Attach(PayRequestDet PayRequestDet);
        DbEntityEntry<PayRequestDet> Entry(PayRequestDet PayRequestDet);
        void Remove(PayRequestDet PayRequestDet);
        //Follow Up
        IQueryable<PayReqGridViewModel> ReadPayFollowUpsGrid(int companyId, string culture);
        IQueryable<PayReqGridViewModel> ReadApprovedPaysGrid(int companyId, string culture);
        #endregion

        #region ActiveDDL
        IEnumerable<FormList> GetSalaryItemList(int companyId, string culture);
        IEnumerable<FormList> GetFormulaList(int companyId, string culture);
        IEnumerable<FormList> GetPayrollList(int companyId, string culture);
        IEnumerable<FormList> GetBankList();
        IEnumerable<FormList> GetRangeTableList(int companyId, string culture);
        #endregion

        #region Formula
        IQueryable<FormulaGridViewModel> ReadFormulaGrid(int companyId, string culture);
        FormulaViewModel GetFormulaVM(int id);
        Formula GetFormula(int id);

        void Add(Formula Formula);
        void Attach(Formula Formula);
        DbEntityEntry<Formula> Entry(Formula Formula);
        void Remove(Formula Formula);
        #endregion

        #region Salary Variable
        IQueryable<PayrollVarViewModel> GetSalaryVar();
        SalaryVarFormViewModel GetSalaryVar(Guid Id);
        List<FormList> GetSubPeriods(int payrollId);
        IQueryable<SalaryEmpVarViewModel> ReadEmpSalaryVar(Guid reference);
        SalaryVar GetSalVar(Guid refernce);
        void Add(SalaryVar salaryvar);
        void Attach(SalaryVar salaryvar);
        DbEntityEntry<SalaryVar> Entry(SalaryVar salaryVar);
        void Remove(SalaryVar salaryVar);
        void AddRange(List<SalaryVar> listofsalaryvar);

        #endregion
    }
}
