using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class GroupLeave
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Index("IX_LeaveRequest", Order = 2)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Index("IX_LeaveRequest", Order = 1)]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public LeaveType LeaveType { get; set; }

        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [MaxLength(250)]
        public string Departments { get; set; }

        [Index("IX_LeaveRequest", Order = 3)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New  6-Approved
        public short? ReqReason { get; set; } // lookup code LeaveReason

        [MaxLength(250)]
        public string ReasonDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public float NofDays { get; set; } = 0;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class GroupLeaveLog
    {
        public int Id { get; set; }
        public int GroupLeaveId { get; set; }
        public GroupLeave GroupLeave { get; set; }
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public bool Success { get; set; }
        public bool Approved { get; set; }
        public byte? ReasonCode { get; set; }

        [MaxLength(100)]
        public string Reason { get; set; }
    }
}
