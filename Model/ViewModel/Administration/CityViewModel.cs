using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
     public class CityViewModel
    {
        public int Id { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
       
        public string Name { get; set; }
        public string NameAr { get; set; }

    }
}
