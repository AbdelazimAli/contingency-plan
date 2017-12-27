using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class LookUpTitles
    {
        [MaxLength(15), Key, Column(Order = 1, TypeName = "varchar")]
        public string Culture { get; set; }

        [Key, Column(Order = 2, TypeName = "varchar")]
        [MaxLength(20)]
        public string CodeName { get; set; }

        [Key, Column(Order = 3)]
        public short CodeId { get; set; }

        [MaxLength(30)]
        [Column(Order = 4, TypeName = "varchar")]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
    }
}
