using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class SysDisciplinePeriodViewModel
    {
        public int Id { get; set; }
        public byte? SysType { get; set; }
        [MaxLength(250), Required]
        public string Name { get; set; }

       
    }
}
