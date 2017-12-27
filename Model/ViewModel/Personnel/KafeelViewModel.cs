using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public  class KafeelViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        [MaxLength(100), Required]
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
       

    }
}
