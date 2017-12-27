using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class SalaryItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsSalaryItem { get; set; } // hidden field
        public short? Order { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }

    }
    public class SalaryItemFormViewModel
    {
        public int Id { get; set; } // ALL
        public string Name { get; set; } // ALL
        public string ShortName { get; set; } // ALL
        public bool IsSalaryItem { get; set; } // hidden field
        public short? Order { get; set; } // ALL If !IsSalaryItem && Order == null hide in reports
        public bool IsLocal { get; set; } = false; // ALL
        public int? CompanyId { get; set; } // ALL
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1); // ALL
        public DateTime? EndDate { get; set; } // ALL
        public byte? PrimaryClass { get; set; } // S 1-Earning  2-Deduction  3-Company shares
        public short? SecondaryClass { get; set; } // S lookup user code
        public byte? ItemType { get; set; } // S 1-Permanent  2-Non-Permanent
        public bool Multiple { get; set; } // ALL Multiple entries allowed
        public byte? Basis { get; set; } // S 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        public string InputCurr { get; set; } // ALL
        public byte ValueType { get; set; } = 0; 
        public byte? UoMeasure { get; set; } // Non 1-Money 2-Units 3-Hours  4-Days 
        public int? TstFormula { get; set; } // ALL Testing Value Formula
        public Decimal? MinValue { get; set; } // ALL
        public Decimal? MaxValue { get; set; } // ALL
        public byte? InValidValue { get; set; } // ALL 1-Waring Message   2-Error Message
        public int? FormulaId { get; set; } // S Calculation formula
        public string BatchCurr { get; set; } // S
        public int? DebitGlAccT { get; set; } // S
        public int? CreditGlAccT { get; set; } // S
        public bool Freezed { get; set; } = false; // S
        public byte Termination { get; set; } = 0; // S Salary item processing in termination: 0-To termination date  1-Next close period  2-Next settlement date
        public byte? MinAgeInYears { get; set; } // S Age:
        public byte? MinServInYears { get; set; } // S Working services in years:
        public bool AnnualSettl { get; set; } = false; // S Required annual settlement
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
