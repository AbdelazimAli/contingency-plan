using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class FunctionVM
    {
        public IEnumerable<FunctionViewModel> inserted { get; set; }
        public IEnumerable<FunctionViewModel> updated { get; set; }
        public IEnumerable<FunctionViewModel> deleted { get; set; }
    }
}
