using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Payroll
{
    public class SalaryVar
    {
        [Key]
        public int Id { get; set; }
        public int PayrollId { get; set; }
        public Payrolls Payroll { get; set; }

        [Index("SalaryVarPayEmp", Order = 1)]
        public int PayPeriodId { get; set; }
        public SubPeriod PayPeriod { get; set; }
        public byte Status { get; set; } = 0; // 0-New 1-Approved 2-Deleted

        [Index("SalaryVarPayEmp", Order = 2)]
        public int SalItemId { get; set; }
        public SalaryItem SalItem { get; set; }

        [Index("SalaryVarPayEmp", Order = 3)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Currency { get; set; } // in case of money only

        [MaxLength(20)]
        public string Approvedby { get; set; }

        [Index("SalaryVarRef")]
        public Guid Reference { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
