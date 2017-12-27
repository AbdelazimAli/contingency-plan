using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class TrainEventFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; } // Training Course
        public DateTime StartBookDate { get; set; } // Start Booking Date
        public DateTime EndBookDate { get; set; }
        public bool Internal { get; set; } // Internal Trainging
        public short? MaxBookCount { get; set; } // Max Booking Count
        public short? MinBookCount { get; set; }
        public bool AllowCandidate { get; set; } = false; // Allow the candidates by the manager
        public bool AllowEmpBook { get; set; } = false; // Allow employee booking
        public DateTime? TrainStartDate { get; set; } // Training start date
        public DateTime? TrainEndDate { get; set; }
        public int? PeriodId { get; set; } // hidden
        public int? CenterId { get; set; } // Training Center
        public decimal? Cost { get; set; } // Traing Cost: 1000 Pound Per Employee
        public byte? Adwarding { get; set; } // 1- Company  2-Employee

        public string Curr { get; set; }

        public float? CurrRate { get; set; } // mid rate
        public byte? CostFlag { get; set; } // 1-Per Employee  2-Total
        public int? ResponsbleId { get; set; } // Responsible Employee

        public string Description { get; set; }
        public int? PersonId { get; set; }
        public bool book { get; set; }
    
        public string Notes { get; set; }

        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
