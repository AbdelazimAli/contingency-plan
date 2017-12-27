using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class DistrictViewModel
    {
        public int Id { get; set; }  
        public int CityId { get; set; }
        public City City { get; set; }      
        public string Name { get; set; }        
        public string NameAr { get; set; }

    }
}
