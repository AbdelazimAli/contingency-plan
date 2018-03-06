using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PeopleTrainViewModel
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }

        public string  PersonId { get; set; }     
        public string  CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime? CourseSDate { get; set; } 
        public DateTime? CourseEDate { get; set; }
        public int EmpStatus { get; set; }
        public bool HasImage { get; set; }
        public string Event { get; set; }
    }
    public class PeopleTrainFormViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public bool submit { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime? CourseSDate { get; set; } 
        public DateTime? CourseEDate { get; set; } 
        public int? ActualHours { get; set; }
        public int? PlannedHours { get; set; }
        public bool Internal { get; set; } 
        public decimal? Cost { get; set; }
        public byte? Adwarding { get; set; }
        public DateTime? CantLeave { get; set; } 
        public byte Status { get; set; } 
        public string Notes { get; set; }
        public int? EventId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public byte ApprovalStatus { get; set; } = 1;
        public string Curr { get; set; }
        public float? CurrRate { get; set; }
        public int? WFlowId { get; set; }
        public short? RejectReason { get; set; } // lookup code CompRejectReason
        public string RejectDesc { get; set; }
        public short? CancelReason { get; set; } // lookup code CompCancelReason
        public string CancelDesc { get; set; }
      
        
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class TrainIndexFollowUpViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public string Event { get; set; }
        public string Course { get; set; }
        public int CompanyId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime? CourseSDate { get; set; }
        public DateTime? CourseEDate { get; set; }
        public int? ActualHours { get; set; }
        public byte Status { get; set; }
        public int? EventId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public byte ApprovalStatus { get; set; } = 1;
        public string RoleId { get; set; }
        public int? DeptId { get; set; }
        public int? PositionId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public bool HasImage { get; set; }
        public string Attachement { get; set; }
        public short Gender { get; set; }
        public int EmpStatus { get; set; }
    }

}
