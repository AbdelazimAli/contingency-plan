using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class PersonForm
    {
        public int Id { get; set; }
        public int SendFormId { get; set; }
        public SendForm SendForm { get; set; }
        public FlexForm Form { get; set; }
        public int FormId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public int EmpId { get; set; }

        [MaxLength(250)]
        public string Question { get; set; }
        public FlexFormColumn FormColumn { get; set; }
        public int FormColumnId { get; set; }

        [MaxLength(500)]
        public string Answer { get; set; }

        [MaxLength(500)]
        public string OtherText { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
