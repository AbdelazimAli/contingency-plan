using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_BranchCode", Order = 1, IsUnique = true)]
        [Index("IX_BranchName", Order = 1, IsUnique = true)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Index("IX_BranchCode", Order = 2, IsUnique = true)]
        public int? Code { get; set; }

        [MaxLength(50), Required]
        [Index("IX_BranchName", 2, IsUnique = true)]
        public string Name { get; set; }

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

        [MaxLength(50)]
        public string Telephone { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }
        
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
