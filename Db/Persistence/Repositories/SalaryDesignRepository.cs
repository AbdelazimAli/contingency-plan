using System;
using System.Data.Entity;
using System.Linq;
using Model.Domain;
using Interface.Core.Repositories;
using Model.Domain.Payroll;
using Model.ViewModel.Payroll;
using System.Collections;
using System.Data.Entity.Infrastructure;

namespace Db.Persistence.Repositories
{
    class SalaryDesignRepository : Repository<InfoTable>,ISalryDesignRepository
    {
        public SalaryDesignRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        #region RangeTable
        public void AddRangeTable(RangeTable range)
        {
            context.RangeTables.Add(range);
        }
        public void AttachRangeTable(RangeTable Range)
        {
            context.RangeTables.Attach(Range);
        }
        public DbEntityEntry<RangeTable> Entry(RangeTable Range)
        {
            return Context.Entry(Range);
        }
        public void Remove(RangeTable Range)
        {
            if (Context.Entry(Range).State == EntityState.Detached)
            {
                context.RangeTables.Attach(Range);
            }
            context.RangeTables.Remove(Range);
        }
        #endregion
        #region LinkTable
        public void AddLinkTable(LinkTable range)
        {
            context.LinkTables.Add(range);
        }
        public void AttachLinkTable(LinkTable Link)
        {
            context.LinkTables.Attach(Link);
        }
        public DbEntityEntry<LinkTable> Entry(LinkTable Link)
        {
            return Context.Entry(Link);
        }
        public void Remove(LinkTable Link)
        {
            if (Context.Entry(Link).State == EntityState.Detached)
            {
                context.LinkTables.Attach(Link);
            }
            context.LinkTables.Remove(Link);
        }
        #endregion
        public IQueryable<SalaryBasisDesignViewModel> GetPayrollsDesigns(int CompanyId)
        {
            return context.InfoTables.Where(a => (!a.IsLocal || (a.IsLocal && a.CompanyId == CompanyId)) && (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate > DateTime.Today))).Select(c =>
                 new SalaryBasisDesignViewModel
                 {
                     IsLocal = c.IsLocal,
                     Name = c.Name,
                     Id = c.Id,
                     Purpose = c.Purpose,
                     Basis = c.Basis
                 });
        }
        public IEnumerable GetPayrollsDesign(int Id)
        {
            int purpose = context.InfoTables.Where(a => a.Id == Id).Select(v => v.Purpose).FirstOrDefault();

            if (purpose != 2)
            {
                var query = (from c in context.LinkTables
                             where c.GenTableId == Id
                             select new LinkTableViewModel
                             {
                                 Id = c.Id,
                                 GenTableId = c.Id,
                                 Basis = c.Basis,
                                 CellValue = c.CellValue,
                                 CreditGlAccT = c.CreditGlAccT,
                                 DebitGlAccT = c.DebitGlAccT,
                                 DeptId = c.DeptId,
                                 FormulaId = c.FormulaId,
                                 Grade = c.Grade,
                                 GradeId = c.GradeId,
                                 JobId = c.JobId,
                                 GroupId = c.GroupId,
                                 MaxValue = c.MaxValue,
                                 BranchId = c.BranchId,
                                 MinValue = c.MinValue,
                                 PayDueId = c.PayDueId,
                                 PersonType = c.PersonType,
                                 Point = c.Point,
                                 Performance = c.Performance,
                                 SalItemId = c.SalItemId,
                                 SubGrade = c.SubGrade,
                                 YesNoForm = c.YesNoForm
                             }).ToList();
                return query;
            }
            else
            {
                var query = (from c in context.RangeTables
                             where c.GenTableId == Id
                             select new RangeTableViewModel
                             {
                                 Id = c.Id,
                                 GenTableId = c.GenTableId,
                                 FormValue = c.FormValue,
                                 RangeValue = c.RangeValue,
                                 TableType = c.TableType,
                                 ToValue = c.ToValue
                             }).ToList();
                return query;
            }
        }
    }
}
