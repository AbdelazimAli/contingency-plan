using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class RoleMenu
    {
        [Key, Column(Order = 1), MaxLength(128)]
        public string RoleId { get; set; }

        [Key, Column(Order = 2)]
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        public byte? DataLevel { get; set; }

        public ICollection<Function> Functions { get; set; }

        [MaxLength(500)]
        public string WhereClause { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
