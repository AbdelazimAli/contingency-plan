using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Payroll
{
    [Table("PayrollSetup")]
    public class PayrollSetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("Id")]
        public Company Company { get; set; }

        public bool MultiCurr { get; set; } = true; // Allow multiple currencies
        public int? DiffDebitAcct { get; set; } // Currency differences debit account

        [ForeignKey("DiffDebitAcct")]
        public Account CurrDiffDebitAcct { get; set; }
        public int? DiffCreditAcct { get; set; } // Currency differences credit account

        [ForeignKey("DiffCreditAcct")]
        public Account CurrDiffCreditAcct { get; set; }

     //   public bool MultiBranch { get; set; } = true;
      //  public int? HeadOfficeNo { get; set; }
     //   public byte AccrualAcctIn { get; set; } // Salary accrual account in: 1-Head Office   2-All Branches
     //   public byte GoverAcctIn { get; set; } // Goverment Authorities accounts in:  1-Head Office   2-All Branches
        public int? DebitSettSal { get; set; } // Debit settlements account

        [ForeignKey("DebitSettSal")]
        public SalaryItem DebitSalaryItem { get; set; }

        public int? CreditSettSal { get; set; } // Credit settlements account

        [ForeignKey("CreditSettSal")]
        public SalaryItem CreditSalaryItem { get; set; }

        // Payroll grade setting
        public byte GradeName { get; set; } // Payroll grade naming: 1-One segment  2-Multiple segments

        [MaxLength(1), Column(TypeName = "char")]
        public string Spiltter { get; set; } // . - / \ 

        [MaxLength(100)]
        public string Group { get; set; } // Groups:

        [MaxLength(100)]
        public string Grade { get; set; } // Grades

        [MaxLength(100)]
        public string SubGrade { get; set; } // SubGrades

        public bool AutoApprovSalVal { get; set; } = true;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
