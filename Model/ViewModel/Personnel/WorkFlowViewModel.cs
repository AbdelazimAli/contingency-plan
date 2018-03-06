using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class WorkFlowViewModel
    {
        public string localName;

        public int Id { get; set; }
        public string Source { get; set; }
        public string Employee { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted 8-Manager Review
        public string Department { get; set; }
        public string Branch { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public bool HasImage { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int CompanyId { get; set; }
        public int? EmpId { get; set; }
        public string RoleId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public int EmpStatus { get; set; }
        public string PositionName { get; set; }
        public string LocalRoleName { get; set; }
    }
}
