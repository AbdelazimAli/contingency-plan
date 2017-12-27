using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class SectionViewModel
    {
        [Key]
        public int Id { get; set; }
        public int FieldSetId { get; set; }
        public string FieldSetDesc { get; set; }
        public int TempId { get; set; }
        public byte? order { get; set; }
        public string name { get; set; }
        public string layout { get; set; } = "form-horizontal";
        public bool Reorderable { get; set; } = true;
        public bool Freez { get; set; }
        public byte? fieldsNumber { get; set; }
        public byte? labelsm { get; set; }
        public byte? labelmd { get; set; }
        public byte? labellg { get; set; }
        public IEnumerable<FormColumnViewModel> fields { get; set; }
    }
}
