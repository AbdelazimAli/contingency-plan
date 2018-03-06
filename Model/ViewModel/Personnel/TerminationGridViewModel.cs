using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class TerminationGridViewModel
    {
        public int Id { get; set; } // Request No
        public int EmpId { get; set; } // Employee
        public string Employee { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime RequestDate { get; set; } // Request Date (Auto) can be modified
        public DateTime? PlanedDate { get; set; } // Planned Date
        public float ServYear { get; set; } // Service Duration      Year:       Month: 
        public float BonusInMonths { get; set; }
        public string TermReason { get; set; } // look up user code  1- End of the contract 2-Resignation  3-Dismissal 4-Death
        public string AssignStatus { get; set; } // Assignment Status look up user code for syscode = 3 Termination
        public string PersonType { get; set; } // Employment Status look up user code for syscode = 3 or 6 Ex-employee
        public byte ApprovalStatus { get; set; }  // 1- New 2-Submit 3-Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted
        public string RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? DeptId { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public string Image { get; set; }
        public short Gender { get; set; }
    }
}
