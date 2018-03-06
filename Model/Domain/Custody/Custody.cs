using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("Custody")]
    public class Custody
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_CustodyCode", Order = 1)]
        [Index("IX_CustodyName", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }

        [MaxLength(30)]
        [Index("IX_CustodyCode", Order = 2)]
        public string Code { get; set; }

        [MaxLength(150), Required]
        [Index("IX_CustodyName", IsUnique = true, Order = 2)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public decimal PurchaseAmount { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Model.Domain.Payroll.Currency Currency { get; set; }
        public float CurrencyRate { get; set; } = 1;

        public CustodyCat CustodyCat { get; set; }
        public int CustodyCatId { get; set; } // 1-Office Tools 2-Car
        public int Sequence { get; set; }

        public int? JobId { get; set; }
        public Job Job { get; set; }

        [MaxLength(20)]
        public string SerialNo { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public bool InUse { get; set; } = false;

        [MaxLength(30)]
        public string ItemCode { get; set; } // Custody code in inventory

        public byte Status { get; set; } = 100; // 0 - 100
        public float Qty { get; set; } = 0;
        public bool Freeze { get; set; } = false;

        public int? BranchId { get; set; }
        public Branch Branch { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
