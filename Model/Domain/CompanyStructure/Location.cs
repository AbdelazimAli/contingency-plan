using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public bool IsLocal { get; set; } = true;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int Code { get; set; }

        [MaxLength(50), Required]
        [Index("IX_LocationName", 1, IsUnique = true)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        public bool IsInternal { get; set; } = true;

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public string CostCenter { get; set; }
        //public int? AddressId { get; set; }

        //public Address Address { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(10)]
        public string TimeZone { get; set; }
        public bool? DaylightSaving { get; set; }

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
