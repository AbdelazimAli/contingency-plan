using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }

        // Basic Data
        [Index("IX_DisciplineCode")]
        public int Code { get; set; }

        [MaxLength(250), Required]
        [Index("IX_DisciplineName")]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public short DisciplineClass { get; set; } // look up code 

        // Discipline Period
        public int? PeriodId { get; set; }

        [ForeignKey("PeriodId")]
        public DisplinPeriod DisPeriod { get; set; } // Default null: Open period 

        public short? DeductPoint { get; set; } // in case of point period

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class DisplinRepeat
    {
        [Key]
        public int Id { get; set; }

        public int DisplinId { get; set; }

        [ForeignKey("DisplinId")]
        public Discipline Discipline { get; set; }

        public byte RepNo { get; set; }

        public short DisplinType { get; set; } // look up user code 1-Verbal alert 2-Written Alert 3-Warning 4-Deduction from salary 5-Suspension 6-Deny promotion 7-Dismissal 8-Dismissal without reward

        // In Case of DisplinType = 4: Deduction from salary
        public float? NofDays { get; set; }
        // In Case of DisplinType = 5-Suspension 6-Deny promotion
        public byte? DenyPeriod { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}

