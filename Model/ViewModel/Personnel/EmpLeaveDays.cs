using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpLeaveDays
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime BirthDate { get; set; }
        public double Balance { get; set; }
    }
}
