using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class PayrollVarViewModel
    {

        public Guid Id { get; set; }
        public int PayrollId { get; set; }
        public int PayPeriodId { get; set; }
        public byte Status { get; set; } = 0; // 0-New 1-Approved 2-Delete
        public int SalItemId { get; set; }
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
