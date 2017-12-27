using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model.Domain
{
    public class EmpBenefit
    {
        public int Id { get; set; }

        public int BenefitId { get; set; }

        [Index("IX_EmpBenefit", IsUnique = true, Order = 1)]
        [Index("IX_Benefit", Order = 1)]
        public int BenefitPlanId { get; set; }

        [ForeignKey("BenefitPlanId")]
        public BenefitPlan BenefitPlan { get; set; }

        [Index("IX_EmpBenefit", IsUnique = true, Order = 2)]
        [Index("IX_Benefit", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        [Index("IX_EmpBenefit", IsUnique = true, Order = 3)]
        [Index("IX_Benefit", Order = 3)]
        public int? BeneficiaryId { get; set; }

        public EmpRelative Beneficiary { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_Benefit", Order = 4)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_Benefit", Order = 5)]
        public DateTime? EndDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
