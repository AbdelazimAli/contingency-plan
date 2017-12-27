using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class ColumnTitle
    {
        [Key, Column(Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Required, MaxLength(15), Key, Column(Order = 2, TypeName = "varchar")]
        public string Culture { get; set; }

        [Required, MaxLength(50), Key, Column(Order = 3, TypeName = "varchar")]
        public string ObjectName { get; set; }

        [Key, Column(Order = 4)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(50), Key, Column(Order = 5, TypeName = "varchar")]
        public string ColumnName { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

    }
}
