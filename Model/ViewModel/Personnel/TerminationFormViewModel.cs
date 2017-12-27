using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class TerminationFormViewModel
    {
        public int Id { get; set; } // Request No
        public int EmpId { get; set; } // Employee
        public string Employee { get; set; }
        public DateTime JoinedDate { get; set; }
        //public DateTime RequestDate { get; set; } // Request Date (Auto) can be modified
        public short? RejectReason { get; set; } // lookup code RejectReason
        [MaxLength(250)]
        public string RejectDesc { get; set; }
        [Required]
        public DateTime? PlanedDate { get; set; } // Planned Date
        public DateTime? ActualDate { get; set; } // Actual Date (not in request)
        public DateTime? LastAccDate { get; set; } // Last salary accural date
        public DateTime? LastAdjustDate { get; set; } // Last payroll adjustment date
        public float ServYear { get; set; } // Service Duration      Year:       Month: 
        public float BonusInMonths { get; set; }
        public short TermReason { get; set; } // look up user code  1- End of the contract 2-Resignation  3-Dismissal 4-Death
        public short AssignStatus { get; set; } // Assignment Status look up user code for syscode = 3 Termination
        public short PersonType { get; set; } // Employment Status look up user code for syscode = 3 or 6 Ex-employee
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted
        public bool Terminated { get; set; } = false;
        [MaxLength(250)]
        public string ReasonDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }
        public DateTime ServStartDate { get; set; }
        public DateTime RequestDate { get; set; } // Request Date (Auto)
        public int Attachments { get; set; }
        public bool Excute { get; set; }
    }

    public class MobileTerminationFormViewModel
    {
        public int Id { get; set; } // Request No
        public int EmpId { get; set; } // Employee
        public string Employee { get; set; }
        public short? RejectReason { get; set; } // lookup code RejectReason
        [MaxLength(250)]
        public string RejectDesc { get; set; }
        [Required]
        public DateTime? PlanedDate { get; set; } // Planned Date
        public DateTime? ActualDate { get; set; } // Actual Date (not in request)
        public DateTime? LastAccDate { get; set; } // Last salary accural date
        public DateTime? LastAdjustDate { get; set; } // Last payroll adjustment date
        public float ServYear { get; set; } // Service Duration      Year:       Month: 
        public float BonusInMonths { get; set; }
        public short TermReason { get; set; } // look up user code  1- End of the contract 2-Resignation  3-Dismissal 4-Death
        public short AssignStatus { get; set; } // Assignment Status look up user code for syscode = 3 Termination
        public short PersonType { get; set; } // Employment Status look up user code for syscode = 3 or 6 Ex-employee
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted
        public bool Terminated { get; set; } = false;
        [MaxLength(250)]
        public string ReasonDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }
        public DateTime ServStartDate { get; set; }
        public DateTime RequestDate { get; set; } // Request Date (Auto)

        public string Job { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
    }
}
