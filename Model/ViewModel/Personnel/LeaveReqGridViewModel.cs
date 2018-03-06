using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveReqGridViewModel
    {

        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string FlowWork { get; set; }
        public string Employee { get; set; }
        public int? EmpId { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float NofDays { get; set; }
        public string ReqReason { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public float? ActualNofDays { get; set; }
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
        public bool HasImage { get; set; }
        public int? EmpStars { get; set; }//
        public string ApproveName { get; set; }
        public bool isStarted { get; set; }
        public bool isBreaked { get; set; }
        public short Gender { get; set; }
        public string Attachement { get; set; }
    }
}
