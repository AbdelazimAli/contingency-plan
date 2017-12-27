using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class EmpRelative
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_EmpRelative", IsUnique = true, Order = 1)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        [MaxLength(150)]
        [Index("IX_EmpRelative", IsUnique = true, Order = 2)]
        public string Name { get; set; }
        public short Relation { get; set; } // Relations in look up code 1-Son 2-Daughter 3-Husband 4-Wife 5-Father 6-Mother

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? BirthDate { get; set; }

        [MaxLength(20)]
        public string NationalId { get; set; }

        [MaxLength(20)]
        public string Telephone { get; set; }

        [MaxLength(20)]
        public string PassportNo { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(30)]
        public string GateIn { get; set; }
        [MaxLength(20)]
        public string Entry { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
