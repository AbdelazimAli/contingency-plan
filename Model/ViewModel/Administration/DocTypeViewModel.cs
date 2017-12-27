using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class DocTypeViewModel
    {
        public int Id { get; set; }

        [MaxLength(30), Required]
        public string Name { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
        public bool HasExpiryDate { get; set; }
        public int? CompanyId { get; set; }
        public bool IsLocal { get; set; }

        public int RequiredOpt { get; set; }
        public string LocalName { get; set; }
    }
}
