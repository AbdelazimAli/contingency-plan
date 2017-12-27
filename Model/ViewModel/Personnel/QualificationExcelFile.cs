using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class QualificationExcelFile
    {
        public int Id { get; set; }
        public int Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public string QualGroupId { get; set; }

        public string Rank { get; set; }
        public string Category { get; set; }
       
    }
}
