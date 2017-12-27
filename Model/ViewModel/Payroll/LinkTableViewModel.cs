using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class LinkTableViewModel
    {
        public int Id { get; set; }
        public int GenTableId { get; set; }
        public byte Basis { get; set; } // 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        public int? SalItemId { get; set; }
        public int? DeptId { get; set; }
        public int? GroupId { get; set; }
        public int? JobId { get; set; }
        public int? GradeId { get; set; }
        public string Grade { get; set; }
        public string SubGrade { get; set; }
        public string Point { get; set; }
        public int? PayDueId { get; set; }
        public int? LocationId { get; set; }
        public short? PersonType { get; set; } // lookup user code
        public short? Performance { get; set; } // lookup code
        public int? YesNoForm { get; set; } // Formula              Formula.Result is 3 or 4
        public decimal? CellValue { get; set; } // Value
        public int? FormulaId { get; set; } // Result     Formula.Result is 1 or 2 or 5
        // Case of value
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        //costing
        public int? DebitGlAccT { get; set; }
        public int? CreditGlAccT { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
