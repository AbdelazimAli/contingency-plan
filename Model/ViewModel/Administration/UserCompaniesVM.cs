using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class UserCompaniesVM
    {
        public IEnumerable<UserCompanyViewModel> inserted { get; set; }
        public IEnumerable<UserCompanyViewModel> updated { get; set; }
        public IEnumerable<UserCompanyViewModel> deleted { get; set; }
    }
}
