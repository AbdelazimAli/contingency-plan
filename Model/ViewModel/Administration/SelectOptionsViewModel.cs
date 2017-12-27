using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class SelectOptionsViewModel
    {
        public int _Id { get; set; } = 0;
        [Required]
        public string _Name { get; set; }
        public byte IsLookUp { get; set; } //1- lookup code      2-lookup user code      3- objectname
        public string SourceName { get; set; }
        public List<string> ColumnsNames { get; set; }
        public List<string> ColumnsValue { get; set; }
        public int seqId { get; set; }
    }
}
