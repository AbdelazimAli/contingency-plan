using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpSchedual
    {
        public string Source { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Organizer { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string SourceName { get; set; }
        public string SourceType { get; set; }
        public string LocatName { get; set; }
        public int MultiDays { get; set; }
    }

    public class Schedual
    {
        public string EmpName { get; set; }
        public IList<EmpSchedual> Tasks { get; set; }
    }
}
