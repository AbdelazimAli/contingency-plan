using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200), Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string SearchName { get; set; }
        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        [MaxLength(15), Column(TypeName = "char")]
        public string Language { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string WebSite { get; set; }
        [MaxLength(500)]
        public string Memo { get; set; }
        public short? Purpose { get; set; }
        [MaxLength(20), Display(Name = "Commercial File No")]
        public string CommFileNo { get; set; }
        [MaxLength(20), Display(Name = "Tax Card No")]
        public string TaxCardNo { get; set; }
        
        [Display(Name = "Tax Authority")]
        public short? TaxAuthority { get; set; }
        public bool Consolidation { get; set; } = false;

        // Social Insurance Information ///
        [MaxLength(20)]
        public string InsuranceNo { get; set; } // Subscription number

        [MaxLength(50)]
        public string Region { get; set; } // Region

        [MaxLength(50)]
        public string Office { get; set; } // Office

        [MaxLength(50)]
        public string Responsible { get; set; } // Responsible Manager
        public short? LegalForm { get; set; } // Legal form  // LegalForm   

        public int? AddressId { get; set; }
        public Address Address { get; set; }
        ///////////////////////////////

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
