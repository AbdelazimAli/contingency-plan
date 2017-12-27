using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class JobClassViewModel
    {
        public int Id { get; set; }
        public bool IsLocal { get; set; } = false;
        public string JobClassCode { get; set; }

        public int? CompanyId { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
