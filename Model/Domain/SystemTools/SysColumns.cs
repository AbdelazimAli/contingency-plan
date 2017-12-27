using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("v_syscolumns")]
    public class SysColumns
    {
        [Key, Column(Order = 1)]
        public string obj_name { get; set; }

        [Key, Column(Order = 2)]
        public string column_name { get; set; }
        public string sch_name { get; set; }
        public int column_order { get; set; }
        public string data_type { get; set; }
        public int? max_length { get; set; }
        public byte num_length { get; set; } // byte
        public int is_required { get; set; }
        public int is_visible { get; set; }
        public string input_type { get; set; }
    }
}
