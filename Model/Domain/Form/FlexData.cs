using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("FlexData")]
    public class FlexData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index("IX_FlexData", IsUnique = true, Order = 1)]
        public int PageId { get; set; }
        public PageDiv Page { get; set; }

        [Required, MaxLength(30), Index("IX_TableColumnData", IsUnique = true, Order = 1), Column(TypeName = "varchar")]
        public string TableName { get; set; }

        [Required, MaxLength(100), Index("IX_FlexData", IsUnique = true, Order = 2), Index("IX_TableColumnData", IsUnique = true, Order = 2), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }

        [Index("IX_FlexData", IsUnique = true, Order = 3), Index("IX_TableColumnData", IsUnique = true, Order = 3)]
        public int SourceId { get; set; }

        [MaxLength(250)]
        public string Value { get; set; }
        public int? ValueId { get; set; }
    }
}
