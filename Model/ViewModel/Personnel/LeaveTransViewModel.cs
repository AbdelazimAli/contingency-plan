using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveTransViewModel
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public string Period { get; set; }
        public string Employee { get; set; }
        public short TransType { get; set; } // 1-Open Balance  2-Post Balance 3-Vacation Compensation  4-Time Compensation 5-Cancel Leave  11-Leave 12-Balance Replacement  13-Balance Deduction
        public DateTime TransDate { get; set; }
        public float DebitQty { get; set; }
        public float CreditQty { get; set; }
        public float Balance { get; set; }
        public string TransTypeName { get; set; }
    }

    public class LeaveTransSummary
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int PeriodId { get; set; }
        public string LeaveType { get; set; }
        public string Period { get; set; }
        public string Employee { get; set; }
        public double Balance { get; set; }
        public int EmpId { get; set; }
        public int DeptId { get; set; }
        public int? BranchId { get; set; }
        public int? SectorId { get; set; }
        public string EmpCode { get; set; }
        public int TypeId { get; set; }
        public string Dept { get; set; }
    }
    public class LeaveTransCounts
    {
        public short AbsenceCode { get; set; }
        public float Balance { get; set; }
        public float Days { get; set; }
        public bool HasAccrualPlan { get; set; }
        public string LeaveName { get; set; }
        public int MaxDays { get; set; }
        public string Name { get; set; }
        public int PeriodId { get; set; }
        public float TotalDays { get; set; }
        public int TypeId { get; set; }
    }
    public class LeaveTransOpenBalanceViewModel
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public float transQty { get; set; }
        public int EmpId { get; set; }
        public string Code { get; set; }
        public int Department { get; set; }
        public int TypeId { get; set; }
        public bool flag { get; set; }
        public int PeriodId { get; set; }
    }
    public class LeaveMoneyAdjustViewModel {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int TypeId { get; set; }
        public int PeriodId { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public DateTime AdjustDate { get; set; } // today date can't change
        public short TransType { get; set; } //hidden lookup code ActionType:  1-Open Balance  2-Post Balance 3-Vacation Compensation  4-Time Compensation  12-Balance Replacement  13-Balance Deduction
        public float NofDays { get; set; }
        //public bool Posted { get; set; } = false; // hidden Poseted: 0- Not Posted   1-Posted
        public DateTime? WorkingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Code { get; set; }
    }
    public class LeaveMoneyAdjustFormViewModel
    {
        public int TypeId { get; set; }
        public int PeriodId { get; set; }
        public int EmpId { get; set; }
        public float NofDays { get; set; }
        public float Balance { get; set; }
    }
    public class LeaveCreditAdjustFormViewModel
    {
        public int TypeId { get; set; }
        public int PeriodId { get; set; }
        public int EmpId { get; set; }
        public float NofDays { get; set; }
        public float Balance { get; set; }
        public string Description { get; set; }
        public int Credit { get; set; }
    }
    public class LeaveRestFormViewModel
    {
        public string Departments { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int TypeId { get; set; }
        public IEnumerable<int> IEmpId { get; set; }
        public float NofDays { get; set; } = 1;
        //public bool Posted { get; set; } = false; // hidden Poseted: 0- Not Posted   1-Posted
        public DateTime? WorkingDate { get; set; }
    }
   

}
