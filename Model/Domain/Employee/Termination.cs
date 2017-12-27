using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Termination
    {
        [Key]
        public int Id { get; set; } // Request No

        [Index("IX_TermReqStatus", Order = 1)]
        public int CompanyId { get; set; }
        public int EmpId { get; set; } // Employee

        [ForeignKey("EmpId")]
        public Person Employee { get; set; } // has active employement

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime ServStartDate { get; set; } // Service Start Date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime RequestDate { get; set; } // Request Date (Auto)

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? PlanedDate { get; set; } // Planned Date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ActualDate { get; set; } // Actual Date (not in request)

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? LastAccDate { get; set; } // Last salary accural date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? LastAdjustDate { get; set; } // Last payroll adjustment date

        public float ServYear { get; set; } // Service duration in years
        public float BonusInMonths { get; set; } // Bonus in months
        public short TermReason { get; set; } // look up user code 

        [MaxLength(250)]
        public string ReasonDesc { get; set; }

        public short? RejectReason { get; set; } // lookup code RejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code CancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }

        public short AssignStatus { get; set; } // Assignment Status look up user code for syscode = 3 Termination
        public short PersonType { get; set; } // Employment Status look up user code for syscode = 3 or 6 Ex-employee

        [Index("IX_TermReqStatus", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 
        public bool Terminated { get; set; } = false;
        public int? WFlowId { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
