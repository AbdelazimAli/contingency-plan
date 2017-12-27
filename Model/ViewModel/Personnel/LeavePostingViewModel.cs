using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeavePostingViewModel
    {
        public int LeaveType { get; set; }
        public int Period { get; set; }
        public bool ClosePeriod { get; set; } = false;
    }

}
