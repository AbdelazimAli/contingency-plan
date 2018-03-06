using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class PeoplesViewModel
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }
        // Basic Data
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string Fathername { get; set; }      
        [MaxLength(20)]
        public string GFathername { get; set; }
        [MaxLength(20)]
        [Required]
        public string Familyname { get; set; }
        public string localName { get; set; }
        public short Gender { get; set; } // 1-Male 2-Female
        [MaxLength(20)]
        public string NationalId { get; set; }
        public DateTime? IdIssueDate { get; set; }

        [MaxLength(20)]
        public string Ssn { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? StartExpDate { get; set; } // Display Experience: 9Y as integer label  
        public int? QualificationId { get; set; }
       
        // Personal Data
        [Column(TypeName = "Date")]
        public DateTime BirthDate { get; set; }  // Display Age: 36Y as integer label
        public int? BirthCountry { get; set; } = 0;// from World view one column update three fields
        public int? BirthCity { get; set; } = 0;
        public int? BirthDstrct { get; set; } = 0;
        public int? Nationality { get; set; }   // Auto display from birth country and can be changed
        public short? MaritalStat { get; set; } // User Code 1-Single 2-Engaged 3-Married 4-Widow 5-Divorced
        public byte? TaxFamlyCnt { get; set; } // Family count in Tax
        public byte? BnftFamlyCnt { get; set; } // Family count in Benefits
        public short? Religion { get; set; } // 1-Muslim 2-Christian

        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(20)]
        public string HomeTel { get; set; }
        [MaxLength(20)]
        public string WorkTel { get; set; }
        [MaxLength(20)]
        public string EmergencyTel { get; set; }
        [MaxLength(50)]
        public string WorkEmail { get; set; }
        [MaxLength(50)]
        public string OtherEmail { get; set; }
        // Military Data
        public byte? MilitaryStat { get; set; }  // 1-Performed 2-Exempt  3-Deferred 4-Under age
        public DateTime? MilStatDate { get; set; }
        [MaxLength(20)]
        public string MilitaryNo { get; set; }
        public DateTime? MilResDate { get; set; } // Transfer to Reserve date
        public short? Rank { get; set; } // lookup
        public short? MilCertGrade { get; set; } // Certificate grade -- lookup

        // Passport Data
        [MaxLength(20)]
        public string PassportNo { get; set; }
        public string VisaNo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? IssueDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }
        public string PicUrl { get; set; }

        [MaxLength(100)]
        public string IssuePlace { get; set; }

        [MaxLength(50)]
        public string Profession { get; set; }

        public int? KafeelId { get; set; }

        // Medical Information
        public short? MedicalStat { get; set; } // lookup

        [Column(TypeName = "Date")]
        public DateTime? MedStatDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? InspectDate { get; set; }
        public int? ProviderId { get; set; }
        public short? BloodClass { get; set; } // O-, O+, AB-, AB+, B-, B+, A-, A+

        // Hiring Information
        [MaxLength(200)]
        public string Recommend { get; set; }
        public short? RecommenReson { get; set; }
        public int Age { get; set; }
        public int? ExpYear { get; set; }
        public int Attachments { get; set; }

        // Record unformation
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int EmpStatus { get; set; }
        public bool HasImage { get; set; }
        public DateTime? SubscripDate { get; set; }
        public decimal? BasicSubAmt { get; set; }
        public decimal? VarSubAmt { get; set; }


        //progress bars
        public string Docs { get; set; } 
        public double profileProgress { get; set; }

        // New
        [MaxLength(20)]
        public string TreatCardNo { get; set; } // Treatment card number
        public PersonStatus Status { get; set; }

        [MaxLength(500)]
        public string Address1 { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? DistrictId { get; set; }
        public District District { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }

        [MaxLength(500)]
        public string HoAddress { get; set; }
        public string BirthLocation { get; set; }
        public string Location { get; set; }
        public string PaperStatus { set; get; }
    }
   public class PersonProfileVM
    {
        public float NofVisible { get; set; }
        public float NofData { get; set; }
    }
   public class EmpLoginDataViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string Culture { get; set; }
        public string CompanyLogo { get; set; }
        public string EmpImg { get; set; }
        public string LocalName { get; set; }

    }
}
