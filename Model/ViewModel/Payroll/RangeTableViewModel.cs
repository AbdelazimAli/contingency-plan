using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class RangeTableViewModel
    {
        public int Id { get; set; }
        public int GenTableId { get; set; }
        public byte? TableType { get; set; }
        // 1- Ratios on ascending scale 
        // 2- Ratios on fixed scale 
        // 3- Values on fixed scale
        public decimal FormValue { get; set; }
        public decimal ToValue { get; set; }
        public double RangeValue { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
