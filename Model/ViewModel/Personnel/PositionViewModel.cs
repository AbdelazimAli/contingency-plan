using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PositionViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string LName { get; set; }
        public byte PositionType { get; set; }
        public byte HiringStatus { get; set; } 
        public bool Seasonal { get; set; }
        public string JobName { get; set; }
        public string  DeptName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public byte? SeasonDay { get; set; } // 1 - 30
        public byte? SeasonMonth { get; set; } // 1 - 12
        public int JobId { get; set; }
        public int? DeptId { get; set; }
        public byte? Headcount { get; set; }
        public byte? SysResponse { get; set; } // 1- Error 2-Warning 3-None
        // Related Positions
        public int? Supervisor { get; set; }
        public int? Relief { get; set; }
        public int? Successor { get; set; }

        // Contract Information
        public byte? ProbationPeriod { get; set; }
        public short? OverlapPeriod { get; set; }
        public int? PayrollId { get; set; }
        public short? SalaryBasis { get; set; } // 1-Year 2-Month 3-Week 4-Day 5-Hour 6-Payroll Period

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class SubperiodViewModel
    {
        public int Id { get; set; }
        public int SubPeriodId { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
    }
}
