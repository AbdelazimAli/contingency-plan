using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.MissionRequest
{
    public class ErrandIndexRequestViewModel
    {
        public int Id { get; set; }       
        public int EmpId { get; set; }
        public int SiteId { get; set; }
        public string Subject { get; set; }
        public bool MultiDays { get; set; } = false; // Multiple Days      
        public string StartDate { get; set; }      
        public string EndDate { get; set; }     
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected    
        public string CreatedUser { get; set; }      
        public string ModifiedUser { get; set; }
        public string Employee { get; set; }
        public string Site { get; set; }
        public int CompanyId { get; set; }
        public string Manager { get; set; }
        public string RoleId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public byte Status { get; set; }
        public DateTime? WorkflowTime { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthBranch { get; set; }
        public int? AuthDept { get; set; }
        public string AuthPosName { get; set; }
        public string AuthEmpName { get; set; }
        public int? AuthEmp { get; set; }
        public int? BranchId { get; set; }
    }
    public class ErrandFormRequestViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public int SiteId { get; set; }

        [MaxLength(500)]
        public string Subject { get; set; }
        public bool MultiDays { get; set; } // Multiple Days
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? ManagerId { get; set; }
        public byte ErrandType { get; set; }
        public bool submit { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
