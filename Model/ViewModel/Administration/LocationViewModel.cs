using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public bool IsLocal { get; set; } = true;
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsInternal { get; set; } = true;
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string LocalName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
