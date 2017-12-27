using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelFormLocationViewModel
    {
        public int Id { get; set; }
        public string IsLocal { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsInternal { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LName { get; set; }
        public string CostCenter { get; set; }      
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string DaylightSaving { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
