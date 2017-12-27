using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class ExcelGridFormLocationViewModel
    {
        public int Id { get; set; }
        public bool IsLocal { get; set; } = true;
      
        public int Code { get; set; }
        public string Name { get; set; }
        public string LName { get; set; }
        public string Description { get; set; }
        public bool IsInternal { get; set; } = true;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CostCenter { get; set; }
        //public int? AddressId { get; set; }

        //public Address Address { get; set; }

        public string Address { get; set; }

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
