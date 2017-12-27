using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class RequestDisplinRangeGrid
    {
        public IEnumerable<DisplinRangeViewModel> inserted { get; set; }
        public IEnumerable<DisplinRangeViewModel> updated { get; set; }
        public IEnumerable<DisplinRangeViewModel> deleted { get; set; }
    }
}
