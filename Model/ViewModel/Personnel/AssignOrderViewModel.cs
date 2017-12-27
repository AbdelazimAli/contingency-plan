using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class AssignOrderViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public int ManagerId { get; set; }
        public string Manager { get; set; }
        public byte Duration { get; set; } // 1-Full day  2-Half day  3-Quarter day
        public float NofDays { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime AssignDate { get; set; } // Assignment date
        [Required]
        public byte CalcMethod { get; set; } = 1; // Calculation Method 1-Monetary 2-Time compensation
        public int? LeaveTypeId { get; set; }
        public string LeaveType { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; } // Expiry date
        // Tasks /////////////////////////
        [MaxLength(500)]
        public string TaskDesc { get; set; }

        // Workflow /////////////////////////////
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 
        public int? WFlowId { get; set; }

        // Time Stamp /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public string RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? DeptId { get; set; }
        public DateTime? WorkflowTime { get; set; }
        public string AuthPosName { get; set; }

        public string AuthEmpName { get; set; }
        public int Month { get; set; }
        public float TimeNofDays { get; set; }
        public string Job { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string Year { get; set; }
        public byte Status { get; set; }
    }
}
