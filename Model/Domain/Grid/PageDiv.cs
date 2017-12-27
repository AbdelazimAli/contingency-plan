using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("PageDivs")]
    public class PageDiv
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(15)]
        public string DivType { get; set; } = "Grid";

        [Index("IX_Grid", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Index("IX_Grid", IsUnique = true, Order = 3)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(30), Index("IX_Grid", IsUnique = true, Order = 2), Column(TypeName = "varchar")]
        public string ObjectName { get; set; }

        [MaxLength(30)]
        public string TableName { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public bool HasCustCols { get; set; } = false;
    };
}
