using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class WorkFlowRangVM
    {
       
            public IEnumerable<WorkFlowObjectsViewModel> inserted { get; set; }
            public IEnumerable<WorkFlowObjectsViewModel> updated { get; set; }
        
    }
}
