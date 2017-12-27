using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class SalaryBasisDesignViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public byte Basis { get; set; } // 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        public byte Purpose { get; set; } = 1;

        // 1- Determining the values ​​of the various salary items, such as the table of the values ​​of the Grades, 
        //  Allowances and how these values ​​are linked to payrolls, jobs or employees
        // 2- Design a table of Graduated taxes, Overtime rates ..., and use them in formulas
        // 3- Decision table (for some complex business rules)

       
    }
}
