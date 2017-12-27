using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
  public class FlexColumnsViewModel
    {
        public int Id { get; set; }

        [Required]
        public int PageId { get; set; }
        public byte ColumnOrder { get; set; }
        [Required, MaxLength(100)]
        public string ColumnName { get; set; }
        public string TableName { get; set; }
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
        public string Title { get; set; }
    }
}
