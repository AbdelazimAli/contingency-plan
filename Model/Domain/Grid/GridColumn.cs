using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("GridColumns")]
    public class GridColumn
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_GridColumns", IsUnique = true, Order = 1)]
        public int GridId { get; set; }
        public PageDiv Grid { get; set; }

        public byte ColumnOrder { get; set; }

        [Required, MaxLength(50), Index("IX_GridColumns", IsUnique = true, Order = 2), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }

        public bool isVisible { get; set; } = true;
        public short DefaultWidth { get; set; }
        [MaxLength(20)]
        public string ColumnType { get; set; }
        public bool Required { get; set; } = false;
        public int? Min { get; set; }
        public int? Max { get; set; }

        [MaxLength(100)]
        public string Pattern { get; set; }
        public short? MaxLength { get; set; }
        public byte? MinLength { get; set; }

        [MaxLength(100)]
        public string PlaceHolder { get; set; }

        [MaxLength(100)]
        public string Custom { get; set; }

        [MaxLength(20)]
        public string InputType { get; set; }

        [MaxLength(20)]
        public string OrgInputType { get; set; }

        [MaxLength(50)]
        public string DefaultValue { get; set; }

        public bool IsUnique { get; set; }

        [MaxLength(100)]
        public string UniqueColumns { get; set; }

        [MaxLength(20)]
        public string CodeName { get; set; }
    }
}
