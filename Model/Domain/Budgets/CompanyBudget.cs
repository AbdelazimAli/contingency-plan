using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class CompanyBudget
    {
        public int Id { get; set; }

        [Index("IX_CompanyBudget", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }

        [Index("IX_CompanyBudget", IsUnique = true, Order = 2)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Index("IX_CompanyBudget", IsUnique = true, Order = 3)]
        public int SubPeriodId { get; set; }
        public SubPeriod SubPeriod { get; set; }

        [Index("IX_CompanyBudget", IsUnique = true, Order = 4)]
        public int BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
