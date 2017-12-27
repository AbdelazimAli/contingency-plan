using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public  class BudgetViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? PeriodId { get; set; }
        public string SubPeriodName { get; set; }
        public string BudgetItemName { get; set; }
        public decimal? Amount { get; set; }
        public int? CompanyId { get; set; }
        public int BudgetItemId { get; set; }
    }
    public class BudgetItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Code { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }


}
