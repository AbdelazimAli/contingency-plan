using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class DisplinPeriodViewModel
    {
        public int Id { get; set; }
        // Basic Data
        [MaxLength(100)]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public byte SysType { get; set; } // 1- Gradient in penalty 2-Points system
        public int? MaxPoints { get; set; } // The maximum penalties allowed
        public int? PointsAdd { get; set; }
        public byte Frequency { get; set; } // 1-Day 2-Month  3-Year
        public short Times { get; set; }  // Frequency: 3 Month
        public int? TotalYear { get; set; }
        public DateTime PeriodSDate { get; set; }
        public int PeriodNo { get; set; }
        public bool Posted { get; set; }
        public bool submit { get; set; }
        public short? MaxDaysDeduction { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }


    }
}
