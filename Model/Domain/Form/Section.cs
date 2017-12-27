using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        //[NotMapped]
        //public int PageId { get; set; } = 0;

        public int FieldSetId { get; set; }
        public FieldSet FieldSet { get; set; }
        public byte Order { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(15)]
        public string LayOut { get; set; } = "form-horizontal";
        public bool Reorderable { get; set; } = true;
        public bool Freeze { get; set; }

        public byte? FieldsNumber { get; set; }
        public byte? LabelSm { get; set; }
        public byte? LabelMd { get; set; }
        public byte? LabelLg { get; set; }
    }
}
