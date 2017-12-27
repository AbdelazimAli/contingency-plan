using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Address", IsUnique = true, Order = 1)]
        [MaxLength(500), Display(Name = "Address 1"), Required]
        public string Address1 { get; set; }

        [Index("IX_Address", IsUnique = true, Order = 2)]
        [MaxLength(100), Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Index("IX_Address", IsUnique = true, Order = 3)]
        [MaxLength(100), Display(Name = "Address 3")]
        public string Address3 { get; set; }

        [ForeignKey("Country")]
        [Display(Name = "The Location")]
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }
        public City City { get; set; }

        [ForeignKey("District")]
        public int? DistrictId { get; set; }
        public District District { get; set; }

        [MaxLength(15), Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [MaxLength(50), Display(Name = "Telephone")]
        [RegularExpression("([0-9]+)", ErrorMessage ="Invalid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
