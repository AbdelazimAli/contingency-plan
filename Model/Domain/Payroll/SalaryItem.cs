using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain.Payroll
{
    public class SalaryItem
    {
        [Key]
        public int Id { get; set; } // ALL

        [MaxLength(100), Required]
        public string Name { get; set; } // ALL

        [MaxLength(30), Required]
        public string ShortName { get; set; } // ALL
        public bool IsSalaryItem { get; set; } // hidden field
        public short? Order { get; set; } // ALL If !IsSalaryItem && Order == null hide in reports
        public bool IsLocal { get; set; } = false; // ALL
        public int? CompanyId { get; set; } // ALL
        public Company Company { get; set; } // ALL

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1); // ALL

        [DataType(DataType.Date), Column(TypeName = "Date")] 
        public DateTime? EndDate { get; set; } // ALL

        // Salary items only /////////////////
        public byte? PrimaryClass { get; set; } // S 1-Earning  2-Deduction  3-Company shares
        public short? SecondaryClass { get; set; } // S lookup user code

        // Salary Items Processing
        public byte? ItemType { get; set; } // S 1-Permanent  2-Non-Permanent
        public bool Multiple { get; set; } // ALL Multiple entries allowed
        public byte? Basis { get; set; } // S 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period

        [MaxLength(3)]
        public string InputCurr { get; set; } // ALL
        public byte ValueType { get; set; } = 0; // S (for salary items only)
                                                 // Salary Item amount: 0-Enter a value directly on the employee
                                                 // 1-Select one formula for all employees  
                                                 // 2-Specific value or formula when linking with employees  

        // Non salary item
        public byte? UoMeasure { get; set; } // Non 1-Money 2-Units 3-Hours  4-Days 

        // Value directly on the employee
        public int? TstFormula { get; set; } // ALL Testing Value Formula

        [ForeignKey("TstFormula")]
        public Formula TestFormula { get; set; }
        public Decimal? MinValue { get; set; } // ALL
        public Decimal? MaxValue { get; set; } // ALL
        public byte? InValidValue { get; set; } // ALL 1-Waring Message   2-Error Message

        /////////////////////  Salary items only ///////////////////////////////////
        // case of formula
        public int? FormulaId { get; set; } // S Calculation formula
        public Formula Formula { get; set; }

        // Costing
       // public byte CostType { get; set; } = 0; // 0-Fixed Cost 1-Not fixed cost
        [MaxLength(3)]
        public string BatchCurr { get; set; } // S

        public int? DebitGlAccT { get; set; } // S

        [ForeignKey("DebitGlAccT")]
        public Account DebitAccount { get; set; }
        public int? CreditGlAccT { get; set; } // S

        [ForeignKey("CreditGlAccT")]
        public Account CreditAccount { get; set; }
      
        // Advanced options 
        public bool Freezed { get; set; } = false; // S
        public byte Termination { get; set; } = 0; // S Salary item processing in termination: 0-To termination date  1-Next close period  2-Next settlement date
        public byte? MinAgeInYears { get; set; } // S Age:
        public byte? MinServInYears { get; set; } // S Working services in years:
        public bool AnnualSettl { get; set; } = false; // S Required annual settlement
        /////////////////////  //// ///////////////////////////////////

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
