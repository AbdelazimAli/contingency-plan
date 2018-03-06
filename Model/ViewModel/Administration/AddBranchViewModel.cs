using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class AddBranchViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string LName { get; set; }
        public string CompanyName { get; set; }
        public int? Code { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Telephone { get; set; }

        [MaxLength(500)]
        public string Address1 { get; set; }

        [MaxLength(50)]
        public string TimeZone { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string Site { get; set; }
        public string Description { get; set; }
    }
}
