using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Menu
    {
        public Menu()
        {
            this.Grids = new HashSet<PageDiv>();
        }

        [Key]
        public int Id { get; set; }

        [Index("IX_MenuSort", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public byte Version { get; set; } = 0;
        public short Sequence { get; set; } = 0;

        [MaxLength(500)]
        public string WhereClause { get; set; }

        [MaxLength(50), Required]
        [Index("IX_MenuName")]
        public string Name { get; set; }

        public int Order { get; set; }
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }
        [MaxLength(100)]
        public string Url { get; set; }
        public ICollection<PageDiv> Grids { get; set; }
        public bool IsVisible { get; set; } = true;

        [MaxLength(50)]
        [Index("IX_MenuSort", IsUnique = true, Order = 2)]
        public string Sort { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        [MaxLength(255)]
        public string ColumnList { get; set; }

        public byte NodeType { get; set; } = 0;
        public bool SSMenu { get; set; } = false;
        public bool Config { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
