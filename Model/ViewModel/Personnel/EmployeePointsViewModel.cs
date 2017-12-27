using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmployeePointsViewModel
    {
        public string Emp { get; set; }
        public int EmpId { get; set; }

        public int? TotalDeduction { get; set; }
        public int? PointsAdd { get; set; }
        public string Period { get; set; }
        public int PeriodId { get; set; }
        public int? Balance { get; set; }
       

    }
}
