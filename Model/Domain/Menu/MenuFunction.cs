using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class MenuFunction
    {
        [Key, Column(Order = 1)]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        [Key, Column(Order = 2)]
        public int FunctionId { get; set; }
        public Function Function { get; set; }
    }
}
