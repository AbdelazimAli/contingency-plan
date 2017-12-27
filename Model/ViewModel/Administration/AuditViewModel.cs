using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class AuditViewModel
    {
        public int Id { get; set; }
        public string ObjectName { get; set; }
        public int ? CompanyId { get; set; }
        public byte Version { get; set; } = 0;
        public string ColumnName { get; set; }
        public string Source { get; set; }
        public string SourceId { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public String Company { get; set; }
        public string URL { get; set; }
        public string DivType { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
}
