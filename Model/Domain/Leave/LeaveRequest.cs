using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Index("IX_LeaveReqStatus", Order = 1)]
        public int CompanyId { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public byte ReqStatus { get; set; } = 2; // 1- Planned   2-Confirmed   from hr always 2

        [Index("IX_LeaveRequest", Order = 1)]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public LeaveType LeaveType { get; set; } // Apply Eligiblity criteria & Conditions

        [Index("IX_LeaveRequest", Order = 2)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Index("IX_LeaveRequest", Order = 3)]
        [Index("IX_LeaveEmpRequest", Order = 1)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }

        [Index("IX_LeaveReqStatus", Order = 2)]
        [Index("IX_LeaveRequest", Order = 4)]
        [Index("IX_LeaveEmpRequest", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 

        public short? ReqReason { get; set; } // lookup code LeaveReason

        [MaxLength(250)]
        public string ReasonDesc { get; set; }

        public short? RejectReason { get; set; } // lookup code RejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code CancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ActualStartDate { get; set; }

        //[DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ActualEndDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ReturnDate { get; set; }
        public float NofDays { get; set; } = 0;
        public byte DayFraction { get; set; } = 0; // 1- First quarter of the day  2-First half of the day 3- Last quarter of the day  4-Last half of the day
        public float? BalanceBefore { get; set; }
        public float? ActualNofDays { get; set; }
        public int? ReplaceEmpId { get; set; }
        public int? AuthbyEmpId { get; set; }
        public int? WFlowId { get; set; }
        public byte? Stars { get; set; }
        //public short? StartTolerance { get; set; } = 0; // leave request start tolerance

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
