using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class TermDurationViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public byte TermSysCode { get; set; }
        public byte WorkDuration { get; set; } // Working duration less than
        public byte FirstPeriod { get; set; } // First Period
        public float Percent1 { get; set; } // Percentage
        public float? Percent2 { get; set; } // Duration serv. remindar percentage
    }
}
