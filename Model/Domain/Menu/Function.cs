using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Function
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Index("IX_FunctionName", IsUnique = true)]
        public string  Name { get; set; }

        public ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
