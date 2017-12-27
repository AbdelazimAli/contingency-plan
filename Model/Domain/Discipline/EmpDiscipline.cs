using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class EmpDiscipline
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_EmpDisCompany")]
        public int CompanyId { get; set; }
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public int? InvestigatId { get; set; }

        [ForeignKey("InvestigatId")]
        public Investigation Investigation { get; set; }

        public int DiscplinId { get; set; } // Violiation

        [ForeignKey("DiscplinId")]
        public Discipline Discipline { get; set; }


        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime ViolDate { get; set; } // Violiation Date

        public int? SuggDispline { get; set; }  // Suggested Discipline

        // In Case of DisplinType = 5-Suspension 6-Deny promotion
        public byte? SuggPeriod { get; set; }  // Suggested period in months
        
        // In Case of DisplinType = 4: Deduction from salary
        public float? SuggNofDays { get; set; }

        public int? ActDispline { get; set; } // Actual Discipline

        [ForeignKey("ActDispline")]
        public DisplinRepeat ActualDiscipline { get; set; }
        public byte? ActualPeriod { get; set; }  // Actual period in months

        // In Case of DisplinType = 4: Deduction from salary
        public float? ActualNofDays { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } // xx

        [MaxLength(30)]
        public string Witness { get; set; } // xx comma seperated Person Id, Multi-select list

        [MaxLength(500)]
        public string Summary { get; set; } // xx

        [MaxLength(500)]
        public string Defense { get; set; } // xx
  
        public int? PeriodId { get; set; }
        public short? DeductPoint { get; set; } // in case of point period

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EffectEDate { get; set; } // in case of open period

        [MaxLength(20)]
        public string DescionNo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? DescionDate { get; set; }
        public int? Manager { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
