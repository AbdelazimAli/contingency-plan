using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class SystemCode
    {
        [Key, Column(Order = 1)]
        [MaxLength(20)]
        public string CodeName { get; set; }

        [Key, Column(Order = 2)]
        public byte SysCodeId { get; set; }

        [MaxLength(20), Required]
        public string SysCodeName { get; set; }
    }
}
