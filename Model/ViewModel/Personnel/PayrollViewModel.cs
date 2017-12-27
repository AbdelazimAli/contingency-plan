using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.ViewModel.Personnel
{
    public class PayrollViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string PeriodId { get; set; } // Period Name
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
    }
    public class PayDueListViewModel
    {
       
        public List<Error> errors { get; set; }
        public  List<PayrollDue> payDueList { get; set; }
    }
    public class PayrollDueViewModel
    {
        public int Id { get; set; }

        public int PayrollId { get; set; }
        public string Name { get; set; }
        public byte? DayNo { get; set; }
    }
    public class PayrollDueVM
    {
        public IEnumerable<PayrollDueViewModel> inserted { get; set; }
        public IEnumerable<PayrollDueViewModel> updated { get; set; }
        public IEnumerable<PayrollDueViewModel> deleted { get; set; }
    }
    public class SubPeriodsVM
    {
        public IEnumerable<SubPeriodsViewModel> inserted { get; set; }
        public IEnumerable<SubPeriodsViewModel> updated { get; set; }
        public IEnumerable<SubPeriodsViewModel> deleted { get; set; }
    }
       

    public class PayrollFormViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int? Period { get; set; }
        public int? PeriodId { get; set; } // Period Name
        public DateTime? FirstCloseDate { get; set; } // First period end date
        public short? PayrollGroup { get; set; } // look up code Payroll group
        public bool AllowNegSalary { get; set; } = false; // Allow negative result
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public short CalcOfstDays { get; set; } = 0; // Calculation day
        public short PayOfstDays { get; set; } = 0; // Pay day
        public byte PayMethod { get; set; } // Payment Method: 1-Cash  2-Cheque  3-Bank Transfer
        public int? BankId { get; set; } // Bank:
        public int? AccrualSalAcct { get; set; } // Accrual salary accout
        public byte DistPercent { get; set; } // distribution: 1-Fixed percentage    2-Variable percentage
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

}
