using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("AspNetRoles")]
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256), Required]
        public string Name { get; set; }
        public bool SSRole { get; set; }
    }
}
