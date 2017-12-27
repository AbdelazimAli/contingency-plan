using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class DisplinRangeViewModel
    {
        public int Id { get; set; }
        public int DisPeriodId { get; set; }      
        public int FromPoint { get; set; }
        public int ToPoint { get; set; }
        public float Percentage { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
