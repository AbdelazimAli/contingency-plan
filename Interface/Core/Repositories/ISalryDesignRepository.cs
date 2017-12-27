using Model.Domain.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain;
using Model.ViewModel.Payroll;
using System.Collections;
using System.Data.Entity.Infrastructure;

namespace Interface.Core.Repositories
{
    public interface ISalryDesignRepository : IRepository<InfoTable>
    {
        void AddRangeTable(RangeTable range);
        void AttachRangeTable(RangeTable Range);
        DbEntityEntry<RangeTable> Entry(RangeTable Range);
        void Remove(RangeTable Range);
        void AttachLinkTable(LinkTable Link);
        void AddLinkTable(LinkTable range);
        DbEntityEntry<LinkTable> Entry(LinkTable Range);
        void Remove(LinkTable Range);
        IQueryable<SalaryBasisDesignViewModel> GetPayrollsDesigns(int CompanyId);
        IEnumerable GetPayrollsDesign(int Id);
    }
}
