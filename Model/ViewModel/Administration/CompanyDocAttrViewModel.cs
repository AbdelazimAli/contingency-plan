using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CompanyDocAttrViewModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public bool Insert { get; set; }
        public string Attribute { get; set; }
        public byte InputType { get; set; }
        public string CodeName { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
        public string ValueText { get; set; }
        public int? ValueId { get; set; }

        public Guid? StreamId { get; set; }
        public bool IsRequired { set; get; }
    }
}
