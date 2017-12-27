using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class PeopleQual
    {
        [Key]
        public int Id { get; set; }
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public bool IsQualification { get; set; } = true;

        public int? QualId { get; set; }

        [ForeignKey("QualId")]
        public Qualification Qualification { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public int? SchoolId { get; set; } // Establishment
        public School School { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? FinishDate { get; set; }
        public byte Status { get; set; } = 2; // 1- Ingoing - 2 - Completed
        public short? GradYear { get; set; }
        public short? Grade { get; set; } // look up code Pass - Good - ...
        public decimal? Score { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        // for certifications only
        [MaxLength(20)]
        public string SerialNo { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? GainDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }
        public decimal? Cost { get; set; }
        public short? Awarding { get; set; } // look up code 1-Company  2-Employee  3-Donor
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
