using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Payroll
{
    public class Payrolls
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_PayrollCode")]
        public int Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        public int? PeriodId { get; set; } // Period Name
        public PeriodName Period { get; set; }
        public DateTime? FirstCloseDate { get; set; } // First period end date
        public short? PayrollGroup { get; set; } // look up code Payroll group
        public bool AllowNegSalary { get; set; } = false; // Allow negative result

        // hidden
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        // effective date
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        // Offset days
        public short CalcOfstDays { get; set; } = 0; // Calculation day
        public short PayOfstDays { get; set; } = 0; // Pay day

       
        // Default
        public byte PayMethod { get; set; } // Payment Method: 1-Cash  2-Cheque  3-Bank Transfer
        public int? BankId { get; set; } // Bank:
        public Provider Bank { get; set; }

        // Costing
        public int? AccrualSalAcct { get; set; } // Accrual salary accout

        [ForeignKey("AccrualSalAcct")]
        public Account AccrualAccount { get; set; }
        public byte DistPercent { get; set; } // distribution: 1-Fixed percentage    2-Variable percentage

        // Accrual Period
        // Period
        // SubPeriod grid

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    // Payroll Dues
    public class PayrollDue
    {
        [Key]
        public int Id { get; set; }

        public int PayrollId { get; set; }
        public Payrolls Payroll { get; set; }

        [MaxLength(30)]
        [Index("IX_PayrollDue", IsUnique = true)]
        public string Name { get; set; }
        public byte? DayNo { get; set; }
    }

    public class PayrollCostDist
    {
        [Key, Column(Order = 1)]
        public int PayrollId { get; set; }
        public Payrolls Payroll { get; set; }

        [Key, Column(Order = 2)]
        public int CostCenterId { get; set; }
        public Account CostCenter { get; set; }

        public float Percentage { get; set; }
    }
}
