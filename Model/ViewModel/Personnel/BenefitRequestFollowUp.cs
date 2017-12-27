using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class BenefitRequestFollowUp
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public int EmpId { get; set; }
        public int BenefitPlanId { get; set; }
        public int ServiceId { get; set; }
        public int ProviderId { get; set; }
        public int? BeneficiaryId { get; set; }
        public int BenefitId { get; set; }
        public int? SubPeriodId { get; set; }
        public decimal? ServCost { get; set; }
        public string Curr { get; set; }
        public float? CurrRate { get; set; } // mid rate
        public decimal? EmpCost { get; set; }
        public decimal? CompanyCost { get; set; }
        public string Employee { get; set; }
        public string PicUrl { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted 8-Manager Review
        [MaxLength(500)]
        public string Description { get; set; }
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
        public short? RejectReason { get; set; } // lookup code CompRejectReason
        [MaxLength(250)]
        public string RejectDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CompCancelReason
        [MaxLength(250)]
        public string CancelDesc { get; set; } 
        public DateTime? IssueDate { get; set; } = DateTime.Today;
        public DateTime? ExpiryDate { get; set; }    
        public DateTime? ServStartDate { get; set; }     
        public DateTime? ServEndDate { get; set; }
        public byte PaidBy { get; set; } = 1; // 1- company  2- employee
        public int ParentId { get; set; }

    }
    public class MedicalRequestViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public int EmpId { get; set; }
        public int ServiceId { get; set; }
        public int ProviderId { get; set; }
        public int? BeneficiaryId { get; set; }
        public int BenefitId { get; set; }
        public int BenefitPlanId { get; set; }
        public decimal? ServCost { get; set; }
        public string Curr { get; set; }
        public float? CurrRate { get; set; } // mid rate
        public decimal? EmpCost { get; set; }
        public decimal? CompanyCost { get; set; }
        public string Employee { get; set; }
        public string PicUrl { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Cancel before accepted 5-Rejected  6-Accepted  7-Cancel after accepted 8-Manager Review
        [MaxLength(500)]
        public string Description { get; set; }  
           
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool submit { get; set; }
        public string RoleId { get; set; }
        public int ParentId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public string FlowWork { get; set; }
        public int Attachments { get; set; }
    }


    public class MedicalIndexViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string Service { get; set; }
        public string Provider { get; set; }
        public int EmpId { get; set; }
        public byte ApprovalStatus { get; set; } = 1;
        public string ApprovalStatusName { get; set; }
        public int? WFlowId { get; set; }
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
        public int CompanyId { get; set; }
        public string Employee { get; set; }
    }
}

