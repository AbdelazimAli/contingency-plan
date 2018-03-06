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
    [Table("PersonSetup")]
    public class PersonSetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("Id")]
        public Company Company { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string LocalCurrCode { get; set; }
        public Currency LocalCurr { get; set; }

        public byte MaxPassTrials { get; set; } = 5;
        public int WaitingInMinutes { get; set; } = 5;

        public bool CodeReuse { get; set; } = false;
        public byte GenEmpCode { get; set; } = 2; // CodeGeneration 1- Auto, 2- Manually Entered, 3- National Id/ Resident Id
        public byte GenWorkCode { get; set; } = 2;
        public byte GenAppCode { get; set; } = 1;

        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }

        public short? WorkHours { get; set; }
        public byte? Frequency { get; set; } // Day, Week, Month, Year
        public byte? Weekend1 { get; set; } = 5;
        public byte? Weekend2 { get; set; } = 6;

        public bool AutoEmployment { get; set; } = false; // Auto create employment checklist
        public bool AutoTermiation { get; set; } = false; // Auto create termination checklist
        public byte EmploymentDoc { get; set; } = 0; // System Actions: Not upload the necessary documents for employment 0-No Action  1-Warning Message  2-Error Message
        public byte JobDoc { get; set; } = 0; //Not upload the necessary documents for specific job 0-No Action  1-Warning Message  2-Error Message
        public byte AssignFlex { get; set; } = 0; // The presence of non-identical data when assigning employee 0-No Action  1-Warning Message  2-Error Message
        public byte ExceedPlanLimit { get; set; } = 0; // When employee service request(under HR permission) exceed planned limit 0-No Action  1-Warning Message  2-Error Message
        public byte ExceedBudgetLimit { get; set; } = 0; // Exceed budget limit 0-No Action  1-Warning Message  2-Error Message

        // Termination Bonus Calculation
        public byte? TermSysCode { get; set; } // Termination Reason // Trls 1-End (or Terminate) of the contract 2-Resignation 3-Involuntary Separation 
        public byte? WorkServMethod { get; set; } // 1-From join date  2-From employment date

        [MaxLength(100)]
        public string ContractTempl { get; set; }

        public int? TaskPeriodId { get; set; }
        public PeriodName TaskPeriod { get; set; }
        public int? BudgetPeriodId { get; set; }
        public PeriodName BudgetPeriod { get; set; }

        public int? QualGroupId { get; set; }

        [ForeignKey("QualGroupId")]
        public QualGroup DefaultQualGroup { get; set; }

        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class TermDuration
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_TermDuration", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Index("IX_TermDuration", IsUnique = true, Order = 2)]
        public byte TermSysCode { get; set; }

        [Index("IX_TermDuration", IsUnique = true, Order = 3)]
        public byte WorkDuration { get; set; } // Working duration less than
        public byte FirstPeriod { get; set; } // First Period
        public float Percent1 { get; set; } // Percentage
        public float? Percent2 { get; set; } // Duration serv. remindar percentage

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public enum CodeGeneration { Auto, Manually, NationalId };
}
