using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class ComplainRequest
    {
        public int Id { get; set; }

        [Index("IX_ComplainReqStatus", Order = 1)]
        public int CompanyId { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Today;
      
        [Index("IX_ComplainRequest", Order = 1)]
        public byte ComplainType { get; set; } // 1-Complaint  2-Grievance   3-Enquiry

        [Index("IX_ComplainRequest", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }

        [Index("IX_ComplainRequest", Order = 3)]
        [Index("IX_ComplainReqStatus", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected

        [MaxLength(500)]
        public string Description { get; set; }
        public byte Against { get; set; } // 1-Employee 2-Manager  3-Procedure  4-Decision  5-Other
        public short? RejectReason { get; set; } // lookup code CompRejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code CompCancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }

        public int? WFlowId { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
