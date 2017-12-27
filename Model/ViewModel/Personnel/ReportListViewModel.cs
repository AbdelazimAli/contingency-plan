using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
        public string Language { get; set; }
        public string Icon { get; set; }
        public byte NodeType { get; set; }
        public string Url { get; set; }
    }
}
