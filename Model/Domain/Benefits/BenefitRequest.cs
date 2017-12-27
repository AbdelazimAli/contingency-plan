using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class BenefitRequest
    {
        public int Id { get; set; }

        [Index("IX_BenefitReqStatus", Order = 1)]
        public int CompanyId { get; set; }

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Index("IX_BenefitRequest", Order = 1)]
        public int? SubPeriodId { get; set; } // hidden
        public SubPeriod SubPeriod { get; set; }

        [Index("IX_BenefitRequest", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }

        [Index("IX_BenefitRequest", Order = 3)]
        public int? BeneficiaryId { get; set; }

        [ForeignKey("BeneficiaryId")]
        public EmpRelative Beneficiary { get; set; }

        [Index("IX_BenefitRequest", Order = 4)]
        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

        public int BenefitPlanId { get; set; }
        public BenefitPlan BenefitPlan { get; set; }

        public int ServiceId { get; set; }
        public BenefitServ Service { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }


        [Index("IX_BenefitReqStatus", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected

        public short? RejectReason { get; set; } // lookup code MedRejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code MedCancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }

        public int? WFlowId { get; set; }

        public decimal? ServCost { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Curreny { get; set; }
        public float? CurrRate { get; set; } // mid rate
        public decimal? EmpCost { get; set; }
        public decimal? CompanyCost { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? IssueDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ServStartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ServEndDate { get; set; }

        public byte PaidBy { get; set; } = 1; // 1- company  2- employee

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
