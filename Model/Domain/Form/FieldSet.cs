using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class FieldSet
    {
        [Key]
        public int Id { get; set; }

        public int PageId { get; set; }
        public PageDiv Page { get; set; }

        public byte Order { get; set; }

        [MaxLength(15)]
        public string LayOut { get; set; } = "form-horizontal";
        public bool Reorderable { get; set; } = true;
        public bool Editable { get; set; }
        public bool Collapsable { get; set; }
        public bool HasTag { get; set; }
        public bool Freeze { get; set; }
        public bool Collapsed { get; set; }

        [MaxLength(50)]
        public string Legend { get; set; }
    }

   
}
