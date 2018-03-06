using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class AddSiteViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? Code { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string Address1 { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }

        public List<int> SiteToEmployees { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }

        [MaxLength(50)]
        public string Telephone { get; set; }

        [MaxLength(15)]
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
        public string CompanyName { get; set; }
        public string LName { get; set; }
        public short SiteType { get; set; }
        public List<string> SiteToEmployeesNames { get; set; }
        public string Site { get; set; }
    }

    public class SiteFormViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? Code { get; set; } 

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short SiteType { get; set; }

        [MaxLength(250)]
        public string Address1 { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }

        [Range(-90, 90, ErrorMessage = "The valid Latitude range is -90 to 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "The valid Longitude range is -180 to 180")]
        public double? Longitude { get; set; }
        public IEnumerable<int> SiteToEmployees { get; set; } = null;

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
        public string CompanyName { get; set; }
        public string LName { get; set; }
    }
}
