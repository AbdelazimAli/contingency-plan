using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Index("IX_PositionCode")]
        public int Code { get; set; }

        [MaxLength(100), Required]
        [Index("IX_PositionName")]
        public string Name { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        // public byte PositionType { get; set; } = 1; // 1- Single 2-Shared 3-None
        public byte? Headcount { get; set; }
        public byte? SysResponse { get; set; } // 1- Error 2-Warning 3-None
        public bool Seasonal { get; set; } = false;
        public byte? SeasonDay { get; set; } // 1 - 30
        public byte? SeasonMonth { get; set; } // 1 - 12
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int? DeptId { get; set; }

        [ForeignKey("DeptId")]
        public CompanyStructure Department { get; set; }
        public byte HiringStatus { get; set; } // 1- Proposed  2-Active  3-Frozen  (4-Cancelled 5-Deleted) invisible in list
        
        // Related Positions
        public int? Supervisor  { get; set; }
        public int? Relief { get; set; }
        public int? Successor { get; set; }

        // Contract Information
        public byte? ProbationPeriod { get; set; }
        public short? OverlapPeriod { get; set; }
        public int? PayrollId { get; set; }
        public Payrolls Payroll { get; set; }
        public short? SalaryBasis { get; set; } // 1-Year 2-Month 3-Week 4-Day 5-Hour 6-Payroll Period

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
