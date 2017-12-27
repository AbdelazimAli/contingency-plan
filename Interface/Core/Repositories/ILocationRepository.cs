using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        IQueryable<LocationViewModel> ReadLocations(string culture,int CompanyId);
        AddLocationViewModel ReadLocation(int Id, string culture);
        IEnumerable<ExcelFormLocationViewModel> ReadExcelLocations(int CompanyId,string Language);

    }
}
