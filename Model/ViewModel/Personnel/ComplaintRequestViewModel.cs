using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class ComplaintRequestViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public byte ComplainType { get; set; } // 1-Complaint  2-Grievance   3-Enquiry
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public string PicUrl { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted 8-Manager Review
        [MaxLength(500)]
        public string Description { get; set; }
        public byte Against { get; set; } // 1-Employee 2-Manager  3-Procedure  4-Decision  5-Other
        public bool IsDeleted { get; set; } = false;
        public short? RejectReason { get; set; } // lookup code CompRejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CompCancelReason
        [MaxLength(250)]
        public string CancelDesc { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool submit { get; set; }
        public string RoleId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public string FlowWork { get; set; }
        public int Attachments { get; set; }
    }

     public class ComplaintIndexViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string AginstName { get; set; }
        public string ComplaintTypeName { get; set; }
        public string ApproveName { get; set; }
        public byte ComplainType { get; set; } // 1-Complaint  2-Grievance   3-Enquiry
        public DateTime RequestDate { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted 8-Manager Review
        public string Description { get; set; }
        public byte Against { get; set; } // 1-Employee 2-Manager  3-Procedure  4-Decision  5-Other
        public string RoleId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public bool HasImage { get; set; }
    }
}
