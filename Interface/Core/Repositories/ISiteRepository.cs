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
    public interface ISiteRepository : IRepository<Site>
    {
        IEnumerable<ExcelFormSiteViewModel> ReadExcelSites(int CompanyId,string Language);
        IQueryable<SiteViewModel> ReadSites(string culture, int CompanyId);
        AddSiteViewModel ReadSite(int Id,int read, string culture);
        void Add(SiteToEmp request);
        void Remove(SiteToEmp request);
    }
}
