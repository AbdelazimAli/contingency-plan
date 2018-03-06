using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class SalaryDesignSecondViewModel
    {
        public int Id { get; set; }
        // Page 2
        public int? SalItemId { get; set; }


        /// The value of the salary items may vary depending on the Grade or Job. 
        /// Please specify the levels on which the salary item is entered. 
        /// It will appear as columns on the next page.
        public bool IDepartment { get; set; } = false;
        public bool IJob { get; set; } = false;
        public bool IPeopleGroup { get; set; } = false;
        public bool IPayrollGrade { get; set; } = false;
        public bool IGrade { get; set; } = false;
        public bool ISubGrade { get; set; } = false;
        public bool IPoints { get; set; } = false;
        public bool IPayroll { get; set; } = false;
        public bool IBranch { get; set; } = false;
        public bool IPersonType { get; set; } = false; // lookup user code
        public bool IPerformance { get; set; } = false; // lookup code

        // Purpose 1 cell value = 0 
        public byte CellValue { get; set; } = 0; // Table cell value: 0- Value  1-Formula

        // Case of value
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }

        // case of formula
        public int? FormulaId { get; set; }
        public byte? TableType { get; set; } // purpose = 2 only
        // 1- Ratios on ascending scale 
        // 2- Ratios on fixed scale 
        // 3- Values on fixed scale
        public int? Y_N_Formula { get; set; } // purpose = 3 only

        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
