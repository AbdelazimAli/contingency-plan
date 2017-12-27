using System.Collections.Generic;
using Model.Domain;
using System.Linq;
using Model.ViewModel;
using Model.ViewModel.Personnel;

namespace Interface.Core.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        IEnumerable<Person> GetPersonList(int pageIndex, int pageSize);

        IQueryable<PeopleViewModel> GetAllPersons();

      //  DataSourceResult GetDataSourceResult(DataSourceRequest request);
    }
}
