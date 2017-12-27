using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class SalaryEmpVarGridViewModel
    {
        public IEnumerable<SalaryEmpVarViewModel> inserted { get; set; }
        public IEnumerable<SalaryEmpVarViewModel> updated { get; set; }
        public IEnumerable<SalaryEmpVarViewModel> deleted { get; set; }
    }
}
