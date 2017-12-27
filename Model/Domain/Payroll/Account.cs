using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Domain.Payroll
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_AccountCode", IsUnique = true)]
        [MaxLength(100), Required]
        public string Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public byte AccType { get; set; } // 1-GL Account   2-App Account   3-Cost Center

        //[MaxLength(1), Column(TypeName = "char")]
        //public string Spiltter { get; set; } // . - / \ 

        [MaxLength(10)]
        public string Segment1 { get; set; }

        [MaxLength(10)]
        public string Segment2 { get; set; }

        [MaxLength(10)]
        public string Segment3 { get; set; }

        [MaxLength(10)]
        public string Segment4 { get; set; }

        [MaxLength(10)]
        public string Segment5 { get; set; }

        [MaxLength(10)]
        public string Segment6 { get; set; }

        [MaxLength(10)]
        public string Segment7 { get; set; }

        [MaxLength(10)]
        public string Segment8 { get; set; }

        [MaxLength(10)]
        public string Segment9 { get; set; }

        [MaxLength(10)]
        public string Segment10 { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    [Table("AccountSetup")]
    public class AccountSetup
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; } // 
        public Company Company { get; set; }

        [Index("IX_AccountSetup", IsUnique = true, Order = 1)]
        public byte AccType { get; set; } // 1-GL Account   2-App Account   3-Cost Center

        [MaxLength(1), Column(TypeName = "char")]
        public string Spiltter { get; set; } // . - / \ 

        [Index("IX_AccountSetup", IsUnique = true, Order = 2)]
        public byte Seq { get; set; }   // 1 to 10

        [MaxLength(20)]
        public string Segment { get; set; }
        public byte SegLength { get; set; } // Max 10
    }
}
