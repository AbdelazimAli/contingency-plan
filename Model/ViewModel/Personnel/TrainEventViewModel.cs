using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class TrainEventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TrainStartDate { get; set; } // Training start date
        public DateTime? TrainEndDate { get; set; }
        public string Center { get; set; } // Training Cente
        public bool AllowCandidate { get; set; } = false; // Allow the candidates by the manager
        public bool AllowEmpBook { get; set; } = false; // Allow employee booking
        public double BookPercent { get; set; }
        public double SucessPercent { get; set; }
        public double AttendPercent { get; set; }
        public int attendCount { get; set; }
        public int BookCount { get; set; }
        public int SucessCount { get; set; }
        public int MaxBookCount { get; set; }


    }
}
