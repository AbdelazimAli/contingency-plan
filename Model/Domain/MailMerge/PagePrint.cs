using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class PagePrint
    {
        public int Id { get; set; }

        [Index("IX_PagePrint", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Required, MaxLength(30), Column(TypeName = "varchar")]
        [Index("IX_PagePrint", IsUnique = true, Order = 2)]
        public string ObjectName { get; set; }


        [Index("IX_PagePrint", IsUnique = true, Order = 3)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(15), Column(TypeName = "varchar")]
        [Index("IX_PagePrint", IsUnique = true, Order = 4)]
        public string Culture { get; set; }


        [MaxLength(100)]
        [Index("IX_LetterTempl", IsUnique = true)]
        public string LetterTempl { get; set; }

        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
