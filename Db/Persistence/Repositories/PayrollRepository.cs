using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Payroll;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using Model.Domain.Payroll;

namespace Db.Persistence.Repositories
{
    class PayrollRepository : Repository<Payrolls>, IPayrollRepository
    {
        public PayrollRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        #region payroll
        public IQueryable<PayrollViewModel> GetPayrolls(int CompanyId)
        {
            return from p in context.Payrolls
                   where (((p.IsLocal && p.CompanyId == CompanyId) || p.IsLocal == false) && (p.StartDate <= DateTime.Today && (p.EndDate == null || p.EndDate >= DateTime.Today)))
                   join pN in context.PeriodNames on p.PeriodId equals pN.Id into j
                   from pN in j.DefaultIfEmpty()
                   select new PayrollViewModel
                   {
                       Id = p.Id,
                       Code = p.Code,
                       Name = p.Name,
                       EndDate = p.EndDate,
                       StartDate = p.StartDate,
                       PeriodId = pN.Name,
                   };
        }
        public PayrollFormViewModel GetPayroll(int Id)
        {
            var query = (from p in context.Payrolls
                         where p.Id == Id
                         select new PayrollFormViewModel
                         {
                             Id = p.Id,
                             Name = p.Name,
                             FirstCloseDate = p.FirstCloseDate,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             AccrualSalAcct = p.AccrualSalAcct,
                             AllowNegSalary = p.AllowNegSalary,
                             BankId = p.BankId,
                             CalcOfstDays = p.CalcOfstDays,
                             Code = p.Code,
                             CompanyId = p.CompanyId,
                             IsLocal = p.IsLocal,
                             DistPercent = p.DistPercent,
                             PayrollGroup = p.PayrollGroup,
                             PayMethod = p.PayMethod,
                             PayOfstDays = p.PayOfstDays,
                             PeriodId = p.PeriodId,
                             CreatedTime = p.CreatedTime,
                             CreatedUser = p.CreatedUser,
                             ModifiedTime = p.ModifiedTime,
                             ModifiedUser = p.ModifiedUser
                         }).FirstOrDefault();
            return query;
        }
        #endregion

        #region PayrollDue
        public IQueryable<PayrollDueViewModel> readPayDue(int PayrollId)
        {
            return from d in context.PayrollDues
                   where d.PayrollId == PayrollId
                   select new PayrollDueViewModel
                   {
                       Id = d.Id,
                       PayrollId = d.PayrollId,
                       DayNo = d.DayNo,
                       Name = d.Name
                   };
        }
        public void Add(PayrollDue PayrollDue)
        {
            context.PayrollDues.Add(PayrollDue);
        }
        public void Attach(PayrollDue PayrollDue)
        {
            context.PayrollDues.Attach(PayrollDue);
        }
        public DbEntityEntry<PayrollDue> Entry(PayrollDue PayrollDue)
        {
            return Context.Entry(PayrollDue);
        }
        public void Remove(PayrollDue PayrollDue)
        {
            if (Context.Entry(PayrollDue).State == EntityState.Detached)
            {
                context.PayrollDues.Attach(PayrollDue);
            }
            context.PayrollDues.Remove(PayrollDue);
        }
        #endregion

        #region Pay Request
        public IQueryable<PayReqGridViewModel> ReadPayRequestsGrid(int companyId, string culture)
        {
            return from p in context.PayRequests
                   where p.CompanyId == companyId
                   join wft in context.WF_TRANS on new { p1 = "Pay", p2 = p.CompanyId, p3 = p.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                   from wft in g.DefaultIfEmpty()
                   join apos in context.Positions on wft.AuthPosition equals apos.Id into g1
                   from apos in g1.DefaultIfEmpty()
                   join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g2
                   from dep in g2.DefaultIfEmpty()
                   join role in context.Roles on wft.RoleId equals role.Id into g3
                   from role in g3.DefaultIfEmpty()
                   join ae in context.People on wft.AuthEmp equals ae.Id into g4
                   from ae in g4.DefaultIfEmpty()
                   select new PayReqGridViewModel
                   {
                       Id = p.Id,
                       RequestNo = p.RequestNo,
                       ApprovalStatus = p.ApprovalStatus,
                       CompanyId = p.CompanyId,
                       PayMethod = p.PayMethod,
                       RequestDate = p.RequestDate,
                       Requester = HrContext.TrlsName(p.Employee.Title + " " + p.Employee.FirstName + " " + p.Employee.Familyname, culture),
                       RoleId = wft.RoleId.ToString(),
                       DeptId = wft.DeptId,
                       PositionId = wft.PositionId,
                       AuthBranch = wft.AuthBranch,
                       AuthDept = wft.AuthDept,
                       AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                       AuthEmp = wft.AuthEmp,
                       AuthEmpName = HrContext.TrlsName(ae.Title + " " + ae.FirstName + " " + ae.Familyname, culture),
                       AuthPosition = wft.AuthPosition,
                       AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                       BranchId = wft.BranchId,
                       SectorId = wft.SectorId,
                   };
        }

        public int NextRequestNo(int companyId)
        {
            return context.PayRequests.Where(p => p.CompanyId == companyId).OrderByDescending(p => p.RequestNo).Select(p => p.RequestNo).FirstOrDefault() + 1;
        }
        public PayRequestViewModel GetPayRequestVM(int requestId, string culture)
        {
            if (requestId == 0)
                return new PayRequestViewModel();
            else
                return context.PayRequests.Where(p => p.Id == requestId)
                    .Select(p => new PayRequestViewModel
                    {
                        Id = p.Id,
                        RequestNo = p.RequestNo,
                        RequestDate = p.RequestDate,
                        ApprovalStatus = p.ApprovalStatus,
                        Requester = p.Requester,
                        RequesterEmp = HrContext.TrlsName(p.Employee.Title + " " + p.Employee.FirstName + " " + p.Employee.Familyname, culture),
                        Paid = p.Paid,
                        PayDate = p.PayDate,
                        PayMethod = p.PayMethod,
                        EmpSelect = p.EmpSelect,
                        Departments = p.Departments,
                        Employees = p.Employees,
                        PaySelect = p.PaySelect,
                        PayPercent = p.PayPercent * 100,
                        FormulaId = p.FormulaId,
                        PayrollGroup = p.PayrollGroup,
                        PayrollId = p.PayrollId,
                        SalaryItems = p.SalaryItems,
                        CancelReason = p.CancelReason,
                        CancelDesc = p.CancelDesc,
                        RejectReason = p.RejectReason,
                        RejectDesc = p.RejectDesc,
                        WFlowId = p.WFlowId,
                        CreatedTime = p.CreatedTime,
                        CreatedUser = p.CreatedUser,
                        ModifiedTime = p.ModifiedTime,
                        ModifiedUser = p.ModifiedUser
                    }).FirstOrDefault();
        }

        public PayRequest GetRequest(int Id)
        {
            return context.PayRequests.Find(Id);
        }
        public IQueryable<PayRequestEmpsViewModel> GetDeptsEmp(int requestId, string Emps, string Depts, int companyId, string culture)
        {
            if (requestId != 0 && String.IsNullOrEmpty(Emps) && String.IsNullOrEmpty(Depts))
            {
                return context.PayRequestDets.Where(r => r.RequestId == requestId)
                    .Select(r => new PayRequestEmpsViewModel
                    {
                        Id = r.RequestId,
                        RequestId = r.RequestId,
                        BankId = r.BankId,
                        EmpAccountNo = r.EmpAccountNo,
                        EmpId = r.EmpId,
                        Employee = HrContext.TrlsName(r.Employee.Title + " " + r.Employee.FirstName + " " + r.Employee.Familyname, culture),
                        PayAmount = r.PayAmount,
                        Stopped = r.Stopped
                    });
            }

            var List = String.IsNullOrEmpty(Emps) ? Depts.Split(',').Select(d => Convert.ToInt32(d)).ToList() : Emps.Split(',').Select(e => Convert.ToInt32(e)).ToList();

            var query = (from a in context.Assignments
                         where a.CompanyId == companyId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)
                         && (String.IsNullOrEmpty(Emps) ? List.Contains(a.DepartmentId) : List.Contains(a.EmpId))
                         join p in context.PayRequestDets on a.EmpId equals p.EmpId into g
                         from p in g.Where(r => r.RequestId == requestId).DefaultIfEmpty()
                         select new PayRequestEmpsViewModel
                         {
                             Id = p == null ? 0 : p.RequestId,
                             RequestId = p == null ? 0 : p.RequestId,
                             BankId = p.BankId,
                             EmpAccountNo = p.EmpAccountNo,
                             PayAmount = p == null ? 0 : p.PayAmount,
                             Stopped = p == null ? false : p.Stopped,
                             EmpId = a.EmpId,
                             Employee = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture),
                         });
            return query;
        }

        //followup
        public IQueryable<PayReqGridViewModel> ReadPayFollowUpsGrid(int companyId, string culture)
        {
            return from p in context.PayRequests
                   where p.CompanyId == companyId && p.ApprovalStatus > 1 && p.ApprovalStatus < 6
                   join wft in context.WF_TRANS on new { p1 = "Pay", p2 = p.CompanyId, p3 = p.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                   from wft in g.DefaultIfEmpty()
                   join apos in context.Positions on wft.AuthPosition equals apos.Id into g1
                   from apos in g1.DefaultIfEmpty()
                   join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g2
                   from dep in g2.DefaultIfEmpty()
                   join role in context.Roles on wft.RoleId equals role.Id into g3
                   from role in g3.DefaultIfEmpty()
                   join ae in context.People on wft.AuthEmp equals ae.Id into g4
                   from ae in g4.DefaultIfEmpty()
                   select new PayReqGridViewModel
                   {
                       Id = p.Id,
                       RequestNo = p.RequestNo,
                       ApprovalStatus = p.ApprovalStatus,
                       CompanyId = p.CompanyId,
                       PayMethod = p.PayMethod,
                       RequestDate = p.RequestDate,
                       Requester = HrContext.TrlsName(p.Employee.Title + " " + p.Employee.FirstName + " " + p.Employee.Familyname, culture),
                       RoleId = wft.RoleId.ToString(),
                       DeptId = wft.DeptId,
                       PositionId = wft.PositionId,
                       AuthBranch = wft.AuthBranch,
                       AuthDept = wft.AuthDept,
                       AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                       AuthEmp = wft.AuthEmp,
                       AuthEmpName = HrContext.TrlsName(ae.Title + " " + ae.FirstName + " " + ae.Familyname, culture),
                       AuthPosition = wft.AuthPosition,
                       AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                       BranchId = wft.BranchId,
                       SectorId = wft.SectorId,
                   };
        }
        public IQueryable<PayReqGridViewModel> ReadApprovedPaysGrid(int companyId, string culture)
        {
            return from p in context.PayRequests
                   where p.CompanyId == companyId && p.ApprovalStatus == 6 && !p.Paid
                   join wft in context.WF_TRANS on new { p1 = "Pay", p2 = p.CompanyId, p3 = p.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                   from wft in g.DefaultIfEmpty()
                   select new PayReqGridViewModel
                   {
                       Id = p.Id,
                       RequestNo = p.RequestNo,
                       ApprovalStatus = p.ApprovalStatus,
                       CompanyId = p.CompanyId,
                       PayMethod = p.PayMethod,
                       RequestDate = p.RequestDate,
                       Requester = HrContext.TrlsName(p.Employee.Title + " " + p.Employee.FirstName + " " + p.Employee.Familyname, culture),
                       Paid = p.Paid,
                       PayDate = p.PayDate,
                       PayPercent = p.PayPercent * 100,
                       DeptId = wft.DeptId,
                       PositionId = wft.PositionId,
                       BranchId = wft.BranchId,
                       SectorId = wft.SectorId,
                   };
        }
        //end followup

        public void Add(PayRequest PayRequest)
        {
            context.PayRequests.Add(PayRequest);
        }
        public void Attach(PayRequest PayRequest)
        {
            context.PayRequests.Attach(PayRequest);
        }
        public DbEntityEntry<PayRequest> Entry(PayRequest PayRequest)
        {
            return Context.Entry(PayRequest);
        }
        public void Remove(PayRequest PayRequest)
        {
            if (Context.Entry(PayRequest).State == EntityState.Detached)
            {
                context.PayRequests.Attach(PayRequest);
            }
            context.PayRequests.Remove(PayRequest);
        }

        public void Add(PayRequestDet PayRequestDet)
        {
            context.PayRequestDets.Add(PayRequestDet);
        }
        public void Attach(PayRequestDet PayRequestDet)
        {
            context.PayRequestDets.Attach(PayRequestDet);
        }
        public DbEntityEntry<PayRequestDet> Entry(PayRequestDet PayRequestDet)
        {
            return Context.Entry(PayRequestDet);
        }
        public void Remove(PayRequestDet PayRequestDet)
        {
            if (Context.Entry(PayRequestDet).State == EntityState.Detached)
            {
                context.PayRequestDets.Attach(PayRequestDet);
            }
            context.PayRequestDets.Remove(PayRequestDet);
        }

        #endregion

        #region Avtive DDLs
        public IEnumerable<FormList> GetSalaryItemList(int companyId, string culture)
        {
            return context.SalaryItems.Where(s => (!s.IsLocal || s.IsLocal && s.CompanyId == companyId))
                .Select(s => new FormList
                {
                    id = s.Id,
                    name = HrContext.TrlsName(s.Name, culture),
                    isActive = s.StartDate <= DateTime.Today && s.EndDate >= DateTime.Today
                }).ToList();
        }

        public IEnumerable<FormList> GetFormulaList(int companyId, string culture)
        {
            return context.Formulas.Where(s => (!s.IsLocal || s.IsLocal && s.CompanyId == companyId))
                .Select(s => new FormList
                {
                    id = s.Id,
                    name = HrContext.TrlsName(s.Name, culture),
                    isActive = s.StartDate <= DateTime.Today && s.EndDate >= DateTime.Today
                }).ToList();
        }

        public IEnumerable<FormList> GetPayrollList(int companyId, string culture)
        {
            return context.Payrolls.Where(s => (!s.IsLocal || s.IsLocal && s.CompanyId == companyId))
                .Select(s => new FormList
                {
                    id = s.Id,
                    name = HrContext.TrlsName(s.Name, culture),
                    isActive = s.StartDate <= DateTime.Today && (s.EndDate >= DateTime.Today || s.EndDate == null)
                }).ToList();
        }

        public IEnumerable<FormList> GetBankList()
        {
            return context.Providers.Where(a => a.ProviderType == 7).Select(a => new FormList { id = a.Id, name = a.Name });
        }

        public IEnumerable<FormList> GetRangeTableList(int companyId, string culture)
        {
            return context.InfoTables.Where(t => !t.IsLocal || (t.IsLocal && t.CompanyId == companyId)).Select(t => new FormList { id = t.Id, name = t.Name }).ToList();
        }
        #endregion

        //ReadPayrollGrades
        #region Payroll Grades
        public IList<PayrollGradesViewModel> ReadPayrollGrades(string culture,int CompanyId)
        {
            var Grades = context.PayrollGrades.
                Where(c => ((c.IsLocal && c.CompanyId == CompanyId) || c.IsLocal == false) && (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today)))
                .ToList();
            var listOfGrads = new List<PayrollGradesViewModel>();
            foreach (var c in Grades)
            {
                var grade = new PayrollGradesViewModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Points = c.Points,
                    Point = c.Points == null ? null : c.Points.Split(',').Select(int.Parse).ToList(),
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Grade = c.Grade,
                    Group = c.Group,
                    IsLocal = c.IsLocal,
                    SubGrade = c.SubGrade,
                    CreatedUser = c.CreatedUser,
                    CreatedTime = c.CreatedTime,
                    ModifiedTime = c.ModifiedTime,
                    ModifiedUser = c.ModifiedUser
                };
                if (grade.Point != null)
                    grade.PointName = (from x in context.LookUpCodes
                                       where grade.Point.Contains(x.CodeId) && x.CodeName == "GradePoints"
                                       select x.Name).ToList();

                listOfGrads.Add(grade);
            }
            return listOfGrads; 
                                   
        }
        #endregion

        #region Account SetUp
        public IQueryable<AccountSetupViewModel> readAccountSetup(int accTypId, int companyId)
        {
            var account = from acc in context.AccountSetup
                          where acc.AccType == accTypId && acc.CompanyId == companyId
                          select new AccountSetupViewModel
                          {
                              Id =acc.Id, 
                              SegLength = acc.SegLength,
                              Seq =acc.Seq,
                              Segment = acc.Segment,
                              Spiltter = acc.Spiltter,
                              CompanyId = acc.CompanyId,
                              AccType = acc.AccType                             
                          };
            return account;        
        }
        
     
        public void Add(AccountSetup account)
        {
            context.AccountSetup.Add(account);
        }
        public void Attach(AccountSetup account)
        {
            context.AccountSetup.Attach(account);
        }
        public DbEntityEntry<AccountSetup> Entry(AccountSetup account)
        {
            return Context.Entry(account);
        }
        public void Remove(AccountSetup account)
        {
            if (Context.Entry(account).State == EntityState.Detached)
            {
                context.AccountSetup.Attach(account);
            }
            context.AccountSetup.Remove(account);
        }

        #endregion

        #region PayrollSetup
        public void Add(PayrollSetup payroll)
        {
            context.PayrollSetup.Add(payroll);
        }
        public void Add(PayrollGrade payrollGrade)
        {
            context.PayrollGrades.Add(payrollGrade);
        }
        public void Attach(PayrollSetup payroll)
        {
            context.PayrollSetup.Attach(payroll);
        }
        public DbEntityEntry<PayrollSetup> Entry(PayrollSetup payroll)
        {
            return Context.Entry(payroll);
        }
        public void Remove(PayrollSetup payroll)
        {
            if (Context.Entry(payroll).State == EntityState.Detached)
            {
                context.PayrollSetup.Attach(payroll);
            }
            context.PayrollSetup.Remove(payroll);
        }
        public void Attach(PayrollGrade payroll)
        {
            context.PayrollGrades.Attach(payroll);
        }
        public DbEntityEntry<PayrollGrade> Entry(PayrollGrade payroll)
        {
            return Context.Entry(payroll);
        }
        public void Remove(PayrollGrade payroll)
        {
            if (Context.Entry(payroll).State == EntityState.Detached)
            {
                context.PayrollGrades.Attach(payroll);
            }
            context.PayrollGrades.Remove(payroll);
        }
        #endregion

        #region  SalaryItem
        public IQueryable<SalaryItemViewModel> GetSalaryItems(int CompanyId)
        {
            return from p in context.SalaryItems
                   where (((p.IsLocal && p.CompanyId == CompanyId) || p.IsLocal == false) && (p.StartDate <= DateTime.Today && (p.EndDate == null || p.EndDate >= DateTime.Today)))
                   select new SalaryItemViewModel
                   {
                       Id = p.Id,
                       Name = p.Name,
                       EndDate = p.EndDate,
                       StartDate = p.StartDate,
                       IsSalaryItem = p.IsSalaryItem,
                       Order = p.Order,
                       ShortName = p.ShortName,
                   };
        }
        public SalaryItemFormViewModel GetSalaryItem(int Id)
        {
            var query = (from s in context.SalaryItems
                         where s.Id == Id
                         select new SalaryItemFormViewModel
                         {
                             Id = s.Id,
                             Name = s.Name,
                             ShortName = s.ShortName,
                             CompanyId = s.CompanyId,
                             IsLocal = s.IsLocal,
                             IsSalaryItem = s.IsSalaryItem,
                             StartDate = s.StartDate,
                             EndDate = s.EndDate,
                             AnnualSettl = s.AnnualSettl,
                             Basis = s.Basis,
                             BatchCurr = s.BatchCurr,
                             CreatedTime = s.CreatedTime,
                             CreatedUser = s.CreatedUser,
                             CreditGlAccT = s.CreditGlAccT,
                             DebitGlAccT = s.DebitGlAccT,
                             FormulaId = s.FormulaId,
                             Freezed = s.Freezed,
                             InputCurr = s.InputCurr,
                             InValidValue = s.InValidValue,
                             ItemType = s.ItemType,
                             MaxValue = s.MaxValue,
                             MinAgeInYears = s.MinAgeInYears,
                             MinServInYears = s.MinServInYears,
                             MinValue = s.MinValue,
                             ModifiedTime = s.ModifiedTime,
                             ModifiedUser = s.ModifiedUser,
                             Multiple = s.Multiple,
                             Order = s.Order,
                             PrimaryClass = s.PrimaryClass,
                             SecondaryClass = s.SecondaryClass,
                             Termination = s.Termination,
                             TstFormula = s.TstFormula,
                             UoMeasure = s.UoMeasure,
                             ValueType = s.ValueType
                         }).FirstOrDefault();
            return query;
        }

        public void Add(SalaryItem salaryItem)
        {
            context.SalaryItems.Add(salaryItem);
        }
        public void Attach(SalaryItem salaryItem)
        {
            context.SalaryItems.Attach(salaryItem);
        }
        public DbEntityEntry<SalaryItem> Entry(SalaryItem salaryItem)
        {
            return Context.Entry(salaryItem);
        }
        public void Remove(SalaryItem salaryItem)
        {
            if (Context.Entry(salaryItem).State == EntityState.Detached)
            {
                context.SalaryItems.Attach(salaryItem);
            }
            context.SalaryItems.Remove(salaryItem);
        }
        public SalaryItem GetSalary(int? id)
        {
            return Context.Set<SalaryItem>().Find(id);
        }
        #endregion

        #region Accounts
        public IQueryable<AccountViewModel> ReadAccount(int companyId)
        {
            var account = from c in context.Accounts
                          where (c.IsLocal && c.CompanyId == companyId) || c.IsLocal == false && (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today))
                          select new AccountViewModel
                          {
                              Id = c.Id,
                              AccType = c.AccType,
                              Code = c.Code,
                              CompanyId = c.CompanyId,
                              IsLocal = c.IsLocal,
                              EndDate = c.EndDate,                             
                              Name = c.Name,
                              StartDate = c.StartDate,
                              Segment1 = c.Segment1,
                              Segment2 = c.Segment2,
                              Segment3 = c.Segment3,
                              Segment4 = c.Segment4,
                              Segment5 = c.Segment5,
                              Segment6 = c.Segment6,
                              Segment7 = c.Segment7,
                              Segment8 = c.Segment8,
                              Segment9 = c.Segment9,
                              Segment10 = c.Segment10                                
                          };
            return account;
        }
        public void Add(Account account)
        {
            context.Accounts.Add(account);
        }
        public void Attach(Account account)
        {
            context.Accounts.Attach(account);
        }
        public DbEntityEntry<Account> Entry(Account account)
        {
            return Context.Entry(account);
        }
        public void Remove(Account account)
        {
            if (Context.Entry(account).State == EntityState.Detached)
            {
                context.Accounts.Attach(account);
            }
            context.Accounts.Remove(account);
        }
        #endregion

        #region Formula
        public IQueryable<FormulaGridViewModel> ReadFormulaGrid(int companyId, string culture)
        {
            return context.Formulas.Where(f => (!f.IsLocal || f.IsLocal && f.CompanyId == companyId) &&
                    f.StartDate <= DateTime.Today && (f.EndDate >= DateTime.Today) || f.EndDate == null)
                    .Select(f => new FormulaGridViewModel
                    {
                        Id = f.Id,
                        Name = f.Name,
                        IsLocal = f.IsLocal,
                        ShortName = f.ShortName,
                        FormType = f.FormType,
                        Basis = f.Basis,
                        StartDate = f.StartDate,
                        EndDate = f.EndDate,
                        Result = f.Result
                    });
        }

        public FormulaViewModel GetFormulaVM(int id)
        {
            if (id == 0)
                return new FormulaViewModel();
            else
                return context.Formulas.Where(f => f.Id == id)
                .Select(f => new FormulaViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsLocal = f.IsLocal,
                    ShortName = f.ShortName,
                    FormType = f.FormType,
                    Basis = f.Basis,
                    Curr = f.Curr,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    FormText = f.FormText,
                    Result = f.Result,
                    InfoId = f.InfoId,

                }).FirstOrDefault();
        }

        public Formula GetFormula(int id)
        {
            return context.Formulas.Find(id);
        }

        public void Add(Formula Formula)
        {
            context.Formulas.Add(Formula);
        }
        public void Attach(Formula Formula)
        {
            context.Formulas.Attach(Formula);
        }
        public DbEntityEntry<Formula> Entry(Formula Formula)
        {
            return Context.Entry(Formula);
        }
        public void Remove(Formula Formula)
        {
            if (Context.Entry(Formula).State == EntityState.Detached)
            {
                context.Formulas.Attach(Formula);
            }
            context.Formulas.Remove(Formula);
        }
        #endregion

        #region Payroll Variables
        public IQueryable<PayrollVarViewModel> GetSalaryVar()
        {
            var query =context.SalaryVar
            .GroupBy(t => new {t.Reference, t.PayrollId ,t.SalItemId ,t.PayPeriodId})
            .Select(g => new PayrollVarViewModel
            {            
                PayrollId= g.Key.PayrollId,
                SalItemId = g.Key.SalItemId,
                PayPeriodId =g.Key.PayPeriodId ,
                Id = g.Key.Reference                             
            });

            return query;
        }
        public SalaryVarFormViewModel GetSalaryVar(Guid Id)
        {
            var query = (from s in context.SalaryVar
                         where s.Reference == Id
                         select new SalaryVarFormViewModel
                         {
                             Id = s.Id.ToString(), 
                             PayrollId = s.PayrollId,
                             PayPeriodId = s.PayPeriodId,
                             Amount = s.Amount,
                             Curr = s.Curr,
                             SalItemId = s.SalItemId,
                             Status = s.Status,
                             Reference = s.Reference                             
                         }).FirstOrDefault();
            return query;
        }
        //GetSubPeriods
        public List<FormList> GetSubPeriods(int payrollId)
        {
            var ListOfSubPeriods = ( from r in context.Payrolls where r.Id == payrollId
                                    join c in context.PeriodNames on r.PeriodId equals c.Id
                                    join p in context.Periods on c.Id equals p.CalendarId
                                    join s in context.SubPeriods on p.Id equals s.PeriodId
                                    where s.Status == 1
                                    select new FormList
                                    {
                                        id = s.Id,
                                        name = s.Name
                                    }).ToList();
                                    
            return ListOfSubPeriods;
        }
        public IQueryable<SalaryEmpVarViewModel> ReadEmpSalaryVar(Guid reference)
        {
            var SalaryEmpVar = from s in context.SalaryVar
                          where s.Reference == reference &&( s.Status == 0 || s.Status == 1)
                          select new SalaryEmpVarViewModel
                          {
                             Id = s.Id,
                             Amount = s.Amount,
                             Status = s.Status,
                             EmpId = s.EmpId
                          };
            return SalaryEmpVar;
        }
        //find var sallary record
        public SalaryVar GetSalVar(Guid refernce)
        {
            return context.SalaryVar.Where(a => a.Reference == refernce).FirstOrDefault();
        }
        public void Add(SalaryVar salaryvar)
        {
            context.SalaryVar.Add(salaryvar);
        }
        public void Attach(SalaryVar salaryvar)
        {
            context.SalaryVar.Attach(salaryvar);
        }
        public DbEntityEntry<SalaryVar> Entry(SalaryVar salaryVar)
        {
            return Context.Entry(salaryVar);
        }
        public void Remove(SalaryVar salaryVar)
        {
            if (Context.Entry(salaryVar).State == EntityState.Detached)
            {
                context.SalaryVar.Attach(salaryVar);
            }
            context.SalaryVar.Remove(salaryVar);
        }
        public void AddRange(List<SalaryVar> listofsalaryvar)
        {
            context.SalaryVar.AddRange(listofsalaryvar);
        }

        #endregion

    }
}

