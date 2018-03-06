using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        IQueryable<BranchViewModel> ReadBranches(string culture,int CompanyId);
        AddBranchViewModel ReadBranch(int Id, string culture);
    }
}
