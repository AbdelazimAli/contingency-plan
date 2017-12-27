using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CountryViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }      
        public string NameAr { get; set; }        
        public string Nationality { get; set; }      
        public string NationalityAr { get; set; }           
        public string TimeZone { get; set; }
        public bool DayLightSaving { get; set; } = false;
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
