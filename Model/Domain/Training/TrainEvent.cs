using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class TrainEvent
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public int CourseId { get; set; } // Training Course
        public TrainCourse Course { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartBookDate { get; set; } // Start Booking Date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime EndBookDate { get; set; }
        public bool Internal { get; set; } // Internal Trainging
        public short? MaxBookCount { get; set; } // Max Booking Count
        public short? MinBookCount { get; set; }
        public bool AllowCandidate { get; set; } = false; // Allow the candidates by the manager
        public bool AllowEmpBook { get; set; } = false; // Allow employee booking

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? TrainStartDate { get; set; } // Training start date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? TrainEndDate { get; set; }
        public int? PeriodId { get; set; } // hidden
        public Period Period { get; set; }
        public int? CenterId { get; set; } // Training Center
        public Provider Center { get; set; } // syscode = 6
        public decimal? Cost { get; set; } // Traing Cost: 1000 Pound Per Employee

        public byte? Adwarding { get; set; } // 1- Company  2-Employee

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Curreny { get; set; }
        public float? CurrRate { get; set; } // mid rate
        public byte? CostFlag { get; set; } // 1-Per Employee  2-Total
        public int? ResponsbleId { get; set; } // Responsible Employee
        public Person Responsble { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

}
