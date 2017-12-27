using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class FlexColumn
    {
        [Key]
        public int Id { get; set; }

        [Required, Index("IX_FlexColumn", IsUnique = true, Order = 1)]
        public int PageId { get; set; }
        public PageDiv Page { get; set; }

        public byte ColumnOrder { get; set; }

        [Required, MaxLength(30), Index("IX_TableColumn", IsUnique = true, Order = 1), Column(TypeName = "varchar")]
        public string TableName { get; set; }

        [Required, MaxLength(100), Index("IX_FlexColumn", IsUnique = true, Order = 2), Index("IX_TableColumn", IsUnique = true, Order = 2), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }
        public bool isVisible { get; set; } = true;
        public byte InputType { get; set; }
        [MaxLength(20)]
        public string CodeName { get; set; }
        public bool Required { get; set; } = false;
        public int? Min { get; set; }
        public int? Max { get; set; }

        [MaxLength(100)]
        public string Pattern { get; set; }

        [MaxLength(100)]
        public string PlaceHolder { get; set; }
        public bool IsUnique { get; set; }

        [MaxLength(100)]
        public string UniqueColumns { get; set; }
    }
}
