using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FieldSetViewModel
    {
        public string legendTitle;

        [Key]
        public int Id { get; set; }

        public int PageId { get; set; }
        public byte order { get; set; }
        public string layout { get; set; } = "form-horizontal";
        public bool Reorderable { get; set; } = true;
        public bool LabelEditable { get; set; }
        public bool Collapsable { get; set; }
        public bool HasFieldSetTag { get; set; }
        public bool Freez { get; set; }
        public bool Collapsed { get; set; }
        public string legend { get; set; }

        public  IEnumerable<SectionViewModel> Sections { get; set; }

    }
}
