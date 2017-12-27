using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class BenefitPlan
    {
        public int Id { get; set; }

        [Index("IX_BenefitPlan", IsUnique = true, Order = 1)]
        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

        [MaxLength(100)]
        [Index("IX_BenefitPlan", IsUnique = true, Order = 2)]
        public string PlanName { get; set; }

        public float? EmpPercent { get; set; }
        public decimal? EmpAmount { get; set; }
        public float? CompPercent { get; set; }
        public decimal? CompAmount { get; set; }
        public decimal? CoverAmount { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
