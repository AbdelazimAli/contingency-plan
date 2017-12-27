using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class AddTrailViewModel
    {
        public string ColumnName { get; set; }
        public string ObjectName { get; set; }
        public string SourceId { get; set; }
        public string ValueAfter { get; set; }
        public string ValueBefore { get; set; }

        public int CompanyId { get; set; }
        public byte Version { get; set; }
        public string UserName { get; set; }
        public byte Transtype { get; set; }

    }
}
