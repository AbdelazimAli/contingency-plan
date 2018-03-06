using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class BranchViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string LocalName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
    }
}
