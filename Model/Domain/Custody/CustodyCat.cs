using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class CustodyCat
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        [Index("IX_CustodyCat", IsUnique = true)]
        public string Name { get; set; }

        public bool Disposal { get; set; } = false;

        [MaxLength(15)]
        public string Prefix { get; set; }

        public byte? CodeLength { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
