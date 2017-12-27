using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveBalanceGridViewModel
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int TypeId { get; set; }
        public string Period { get; set; }
        public string NewPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public int NewPeriodId { get; set; }
        public string NewPeriodDate { get; set; }
        public int? NewSubPeriodId { get; set; }
        public int PeriodId { get; set; }
        public int MonthWorkDuration { get; set; }
        public int Age { get; set; }
        public string AgeText { get; set; }
        public float CurrBalance { get; set; }
        public float OpenBalance { get; set; }
        public float PostBal { get; set; }
        public byte PostAction { get; set; }
        public string Reason { get; set; }
        public float Total { get; set; }
        public float Compensations { get; set; }
        public string Employee { get; set; }
        public bool selected { get; set; } = false;
        public short AbsenceType { get; set; }
        public string EmpCode { get; set; }
        public DateTime? SubStartDate { get; set; }
        public DateTime? SubEndDate { get; set; }
        public string WorkDuration { get; set; }
    }

    public class MinDaysViewModel
    {
        public int EmpId { get; set; }
        public float MinLeaveDays { get; set; }
        public float IncludContinu { get; set; }
    }
}
