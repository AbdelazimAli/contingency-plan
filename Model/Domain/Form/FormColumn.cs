using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class FormColumn
    {
        [Key]
        public int Id { get; set; }

        //[NotMapped]
        //public int PageId { get; set; } = 0;

        [Index("IX_FormColumns", IsUnique = true, Order = 1)]
        public int SectionId { get; set; }
        public Section Section { get; set; }

        public byte? ColumnOrder { get; set; }

        [Required, MaxLength(50), Index("IX_FormColumns", IsUnique = true, Order = 2), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }
        public bool isVisible { get; set; } = true;
        public byte? Sm { get; set; }
        public byte? Md { get; set; }
        public byte? Lg { get; set; }

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
        public string HtmlAttribute { get; set; }

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

        [MaxLength(100)]
        public string Formula { get; set; }
    }
}
