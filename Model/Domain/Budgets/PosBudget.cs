using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class PosBudget
    {
        public int Id { get; set; }

        [Index("IX_PosBudget", IsUnique = true, Order = 1)]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Index("IX_PosBudget", IsUnique = true, Order = 2)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Index("IX_PosBudget", IsUnique = true, Order = 3)]
        public int SubPeriodId { get; set; }
        public Period SubPeriod { get; set; }

        [Index("IX_PosBudget", IsUnique = true, Order = 4)]
        public int BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; }
        public decimal BudgetAmount { get; set; }
    }
}
