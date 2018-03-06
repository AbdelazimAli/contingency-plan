using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class SendFormPeople
    {
        [Key, Column(Order = 1)]
        public int SendFormId { get; set; }
        public SendForm SendForm { get; set; }

        [Key, Column(Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }
    }
}
