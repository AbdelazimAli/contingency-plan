using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_JobCode")]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(100), Required]
        [Index("IX_JobName")]
        public string Name { get; set; }

        [MaxLength(100)]
        public string NameInInsurance { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<JobClass> JobClasses { get; set; }
        public ICollection<DocType> Documents { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public int? DefaultGradeId { get; set; }

        [ForeignKey("DefaultGradeId")]
        public PayrollGrade PayrollGrade { get; set; }
        public short? PlanCount { get; set; }
        public float? PlanTurnOverRate { get; set; }
        public bool PrimaryRole { get; set; } = false;
        public short? ProbationPeriod { get; set; }
        public string DescInRecruitment { get; set; }
        // public string SecertRequirment { get; set; }
        public short? WorkHours { get; set; }
        public byte? Frequency { get; set; }

        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }
        public bool? ReplacementRequired { get; set; } = false;

        [MaxLength(100)]
        public string ContractTempl { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
