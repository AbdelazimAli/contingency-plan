using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmpTaskVM
    {
        public IEnumerable<EmpTaskViewModel> inserted { get; set; }
        public IEnumerable<EmpTaskViewModel> updated { get; set; }
        public IEnumerable<EmpTaskViewModel> deleted { get; set; }
    }
}
