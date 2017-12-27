using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class SalaryEmpVarViewModel
    {
        public int Id { get; set; }
        public byte Status { get; set; } = 0; // 0-New 1-Approved 2-Deleted
        public decimal Amount { get; set; }
        public int EmpId { get; set; }
        public Guid reference { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
