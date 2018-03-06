using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Site
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_SiteCode", Order = 1, IsUnique = true)]
        public short SiteType { get; set; } // 1-Customer  2-Vendor

        [Index("IX_SiteCode", Order = 2, IsUnique = true)]
        public int? Code { get; set; }

        [MaxLength(50), Required]
        [Index("IX_SiteName", 1, IsUnique = true)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string Address1 { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }
        public City City { get; set; }

        [ForeignKey("District")]
        public int? DistrictId { get; set; }
        public District District { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }

        [MaxLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [MaxLength(15)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string ContactPerson { get; set; }


        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class SiteToEmp
    {
        [Key, Column(Order =1)]
        public int SiteId { get; set; }
        public Site Site { get; set; }

        [ForeignKey("Employee")]
        [Key, Column(Order = 2)]
        public int EmpId { get; set; }
        public Person Employee { get; set; }
    }
}
