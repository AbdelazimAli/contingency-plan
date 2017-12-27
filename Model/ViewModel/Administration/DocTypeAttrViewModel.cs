using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class DocTypeAttrViewModel
    {
        public int Id { get; set; }
        //[MaxLength(100), Required]
        public string Attribute { get; set; }
        public int TypeId { get; set; }
        [MaxLength(20)]
        public string CodeName { get; set; }      
        public byte InputType { get; set; }
      
    }
}
