using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class SetCurrencyInfoViewModel
    {
        public string Curr { get; set; }
        public decimal Cost { get; set; }
        public float? rate { get; set; }
        public float? EmpPercent { get; set; }
        public float? CompPercent { get; set; }
        public int ParentId { get; set; }

    }
}
