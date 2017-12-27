using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PeopleGroupViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public int Icon { get; set; }
        public string PicUrl { get; set; }
        [MaxLength(100), Required]
        public string Name { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
