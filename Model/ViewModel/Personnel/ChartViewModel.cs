using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class ChartViewModel
    {
        public int Id { get; set; }
        public int value { get; set; }
        public float floatValue { get; set; }
        public string category { get; set; }
        public string color { get; set; }
        public string myGroup { get; set; }

        //Additional
        public int Month { get; set; }
        public int Year { get; set; }
        public int EmpId { get; set; }
        public short Gender { get; set; }
        public DateTime? dateCategory { get; set; }

    }
    public class ColumNameVM
    {
        public string Name { get; set; }
    }

    public class JobPercentChartVM
    {
        public string message { get; set; } 
        public List<ChartViewModel> chartData { get; set; }
    }
}
