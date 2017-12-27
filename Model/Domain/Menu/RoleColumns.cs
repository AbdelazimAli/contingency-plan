using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class RoleColumns
    {
        [Key, Column(Order = 1)]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Required, MaxLength(50), Key, Column(Order = 2, TypeName = "varchar")]
        public string ObjectName { get; set; }

        [Key, Column(Order = 3)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(128), Key, Column(Order = 4)]
        public string RoleId { get; set; }

        [Required, MaxLength(50), Key, Column(Order = 5, TypeName = "varchar")]
        public string ColumnName { get; set; }

        public bool isVisible { get; set; } = false;
        public bool isEnabled { get; set; } = false;
    }
}
