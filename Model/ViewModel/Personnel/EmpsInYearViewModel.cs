using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmpsInYearViewModel
    {
        public int Active { get; set; }
        public int All { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int InActive { get; set; }
        public bool Flag { get; set; } = false;
    }
}
