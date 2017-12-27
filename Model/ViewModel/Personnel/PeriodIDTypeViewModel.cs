using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PeriodIDTypeViewModel
    {
        public int? PeriodId { get; set; }
        public byte? SysType { get; set; } = 2;
        public int Id { get; set; }
        public int DisPeriodNOId { get; set; }

    }
    public class DisplinDLLViewModel
    {
    
        public int id { get; set; }
        public string name { get; set; }
        public int value { get; set; }

    }

}
