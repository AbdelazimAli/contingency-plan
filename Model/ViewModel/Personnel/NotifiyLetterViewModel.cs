using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class NotifiyLetterViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime NotifyDate { get; set; }
        public string  Employee { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public string NotifySource { get; set; }
        public int EmpId { get; set; }
        public bool Sent { get; set; }
        public bool read { get; set; }
        public DateTime? Readdatetime { get; set; }
        public DateTime EventDate { get; set; }

        public string Description { get; set; }
    }
}
