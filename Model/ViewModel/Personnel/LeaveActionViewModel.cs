using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveActionViewModel
    {
        public int Id { get; set; }
        public string TypeId { get; set; }//leave type
        public string PeriodId { get; set; }//leaveperiod
        public string EmpId { get; set; } // employee
        public int TransType { get; set; } // 1-Open Balance  2-Post Balance 3-Vacation Compensation  4-Time Compensation  12-Balance Replacement  13-Balance Deduction
        public float NofDays { get; set; }
        public int EmpStatus { get; set; }
        public bool HasImage { get; set; }
        public int PhotoId { get; set; }
        public int empId { get; set; }

    }
    public class LeaveActionFormViewModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int PeriodId { get; set; }
        public int EmpId { get; set; }
        public DateTime ActionDate { get; set; } = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
        public short TransType { get; set; } 
        public float NofDays { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int Attachments { get; set; }

    }
}
