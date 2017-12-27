using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Domain.Payroll;

namespace Model.Domain
{
    [Table("PeopleTrain")]
    public class PeopleTraining
    {
        // Basic Data
        public int Id { get; set; }

        [Index("IX_TrainEvent", IsUnique = true, Order = 1)]
        public int? EventId { get; set; }
        public TrainEvent Event { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Index("IX_TrainingReqStatus", Order = 1)]
        public int CompanyId { get; set; }

        [Index("IX_PeopleTrain", Order = 1)]
        [Index("IX_TrainEvent", IsUnique = true, Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }

        [Index("IX_PeopleTrain", Order = 2)]
        public int CourseId { get; set; }
        public TrainCourse Course { get; set; }

        [Index("IX_TrainingReqStatus", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected

        [MaxLength(150)]
        public string CourseTitle { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? CourseSDate { get; set; } // Course start date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? CourseEDate { get; set; } // Course end date
        public int? ActualHours { get; set; }
        public short? CourseGrade { get; set; } // lookup code CourseGrade: Good, ..
        // Course Cost
        public bool Internal { get; set; } // Internal Trainging
        public decimal? Cost { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Curreny { get; set; }
        public float? CurrRate { get; set; }
        public byte? Adwarding { get; set; } // 1- Company  2-Employee   3-Other

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? CantLeave { get; set; } // The employee should pay training cost in case of resignation prior to the date

        public byte Status { get; set; } // 0-Request 1-Ingoing  2-Completed  3-Fail

        [MaxLength(250)]
        public string Notes { get; set; }

        public int? WFlowId { get; set; }

        public short? RejectReason { get; set; } // lookup code TrainRejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code TrainCancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
