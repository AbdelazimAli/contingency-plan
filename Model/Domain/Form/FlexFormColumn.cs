using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class FlexFormColumn
    {
        public int Id { get; set; }
        public int FlexFSId { get; set; }

        [ForeignKey("FlexFSId")]
        public FlexFormFS FlexformFS { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public byte ColumnOrder { get; set; }
        public byte InputType { get; set; } // Number, Text, Radio buttons, Checkboxes
        public bool ShowTextBox { get; set; } // show Other (for Radio buttons, Checkboxes)

        [MaxLength(1000)]
        public string Selections { get; set; }
        public bool ShowHint { get; set; }
        [MaxLength(100)]
        public string Hint { get; set; }

        [MaxLength(100)]
        public string Answer { get; set; } // Hidden

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
