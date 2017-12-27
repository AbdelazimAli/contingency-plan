using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class BenefitServPlans
    {
        [Key, Column(Order = 1)]
        public int BenefitPlanId { get; set; }
        public BenefitPlan BenefitPlan { get; set; }

        [Key, Column(Order = 2)]
        public int BenefitServId { get; set; }
        public BenefitServ BenefitServ { get; set; }
    }
}
