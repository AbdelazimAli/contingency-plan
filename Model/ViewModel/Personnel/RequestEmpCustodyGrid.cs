using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class RequestEmpCustodyGrid
    {
        public IEnumerable<EmpCustodyViewModel> inserted { get; set; }
        public IEnumerable<EmpCustodyViewModel> updated { get; set; }
        public IEnumerable<EmpCustodyViewModel> deleted { get; set; }
    }
}
