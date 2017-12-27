using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain.Payroll
{
    public class InfoTable
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        //[MaxLength(30), Required]
        //public string ShortName { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public byte Basis { get; set; } // 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        public byte Purpose { get; set; } = 1;

        // 1- Determining the values ​​of the various salary items, such as the table of the values ​​of the Grades, 
        //  Allowances and how these values ​​are linked to payrolls, jobs or employees
        // 2- Design a table of Graduated taxes, Overtime rates ..., and use them in formulas
        // 3- Decision table (for some complex business rules)

        // Page 2
        public int? SalItemId { get; set; }

        [ForeignKey("SalItemId")]
        public SalaryItem SalaryItem { get; set; }
        /// The value of the salary items may vary depending on the Grade or Job. 
        /// Please specify the levels on which the salary item is entered. 
        /// It will appear as columns on the next page.
        public bool IDepartment { get; set; }
        public bool IJob { get; set; } = false;
        public bool IPeopleGroup { get; set; }
        public bool IPayrollGrade { get; set; } = false;
        public bool IGrade { get; set; } = false;
        public bool ISubGrade { get; set; } = false;
        public bool IPoints { get; set; } = false;
        public bool IPayroll { get; set; } = false;
        public bool ILocation { get; set; } = false;
        public bool IPersonType { get; set; } = false; // lookup user code
        public bool IPerformance { get; set; } = false; // lookup code
        
        // Purpose 1 cell value = 0 
        public byte CellValue { get; set; } = 0; // Table cell value: 0- Value  1-Formula

        // Case of value
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }

        // case of formula
        public int? FormulaId { get; set; }
        public Formula Formula { get; set; }
        public byte? TableType { get; set; } // purpose = 2 only
        // 1- Ratios on ascending scale 
        // 2- Ratios on fixed scale 
        // 3- Values on fixed scale
        public int? Y_N_Formula { get; set; } // purpose = 3 only
        [ForeignKey("Y_N_Formula")]
        public Formula YNFormula { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class LinkTable
    {
        [Key]
        public int Id { get; set; }
        public int GenTableId { get; set; }
        public InfoTable GenTable { get; set; }
        public byte Basis { get; set; } // 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        //public byte Purpose { get; set; } = 1;
        public int? SalItemId { get; set; }

        [ForeignKey("SalItemId")]
        public SalaryItem SalaryItem { get; set; }
        public int? DeptId { get; set; }

        [ForeignKey("DeptId")]
        public CompanyStructure Department { get; set; }

        public int? GroupId { get; set; }
        public PeopleGroup Group { get; set; }

        public int? JobId { get; set; }
        public Job Job { get; set; }

        public int? GradeId { get; set; }
        public PayrollGrade PayrollGrade { get; set; }

        [MaxLength(20)]
        public string Grade { get; set; }

        [MaxLength(20)]
        public string SubGrade { get; set; }

        [MaxLength(20)]
        public string Point { get; set; }

        public int? PayDueId { get; set; }
        public PayrollDue Payroll { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public short? PersonType { get; set; } // lookup user code
        public short? Performance { get; set; } // lookup code

        public int? YesNoForm { get; set; } // Formula              Formula.Result is 3 or 4
        public decimal? CellValue { get; set; } // Value
        public int? FormulaId { get; set; } // Result     Formula.Result is 1 or 2 or 5
        public Formula Formula { get; set; }
        

        // Case of value
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }

        //costing
        public int? DebitGlAccT { get; set; }

        [ForeignKey("DebitGlAccT")]
        public Account DebitAccount { get; set; }
        public int? CreditGlAccT { get; set; }

        [ForeignKey("CreditGlAccT")]
        public Account CreditAccount { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class RangeTable
    {
        [Key]
        public int Id { get; set; }
        public int GenTableId { get; set; }
        public InfoTable GenTable { get; set; }
        public byte? TableType { get; set; }
        // 1- Ratios on ascending scale 
        // 2- Ratios on fixed scale 
        // 3- Values on fixed scale

        public decimal FormValue { get; set; }
        public decimal ToValue { get; set; }
        public double RangeValue { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
