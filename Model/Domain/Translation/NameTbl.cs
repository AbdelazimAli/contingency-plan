using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("NamesTbl")]
    public class NameTbl
    {
        [Required, MaxLength(15), Key, Column(Order = 1, TypeName = "varchar")]
        public string Culture { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
