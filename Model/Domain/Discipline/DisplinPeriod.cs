using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class DisplinPeriod
    {
        public int Id { get; set; }

        // Basic Data
        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public byte SysType { get; set; } // 1- Gradient in penalty 2-Points system
        /// if (SysType = 2)
        /// Minimum points allowed for violations: 
        /// Points added periodically: 
        /// 
        public int? MaxPoints { get; set; } // The maximum penalties allowed
        public int? PointsAdd { get; set; }

        // else if (SysType = 1)
        public short? MaxDaysDeduction { get; set; } // Maximum days deduction in period

        // Period start calculations
        //public DateTime? PeriodSDate { get; set; }
        public byte Frequency { get; set; } // 1-Day 2-Month  3-Year
        public short Times { get; set; }  // Frequency: 3 Month

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    [Table("DisPeriodNo")]
    public class DisPeriodNo
    {
        public int Id { get; set; }

        [Index("IX_DisPeriodNo", IsUnique = true, Order = 1)]
        public int PeriodId { get; set; }

        [ForeignKey("PeriodId")]
        public DisplinPeriod DisPeriod { get; set; }

        [Index("IX_DisPeriodNo", IsUnique = true, Order = 2)]
        public int PeriodNo { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime PeriodSDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime PeriodEDate { get; set; }

        public bool Posted { get; set; } = false;
        public DateTime? PostDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
