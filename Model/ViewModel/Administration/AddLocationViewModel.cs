using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class AddLocationViewModel
    {
        public int Id { get; set; }
        public bool IsLocal { get; set; } = true;
        public int? CompanyId { get; set; }
        public string LName { get; set; }
        public string CompanyName { get; set; }
        public int? Code { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public bool IsInternal { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AddressId { get; set; }
        public string Address { get; set; }

        [MaxLength(10)]
        public string TimeZone { get; set; }
        public bool? DaylightSaving { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
