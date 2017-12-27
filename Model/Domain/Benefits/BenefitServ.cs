using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class BenefitServ
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public BenefitServ Parent { get; set; }
        public int Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        public bool IsGroup { get; set; } = false;
        public float? EmpPercent { get; set; }
        public float? CompPercent { get; set; }

        [Index("IX_BenefitServ", Order = 1)]
        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

        [Index("IX_BenefitServ", Order = 2)]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Index("IX_BenefitServ", Order = 3)]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Curreny { get; set; }
    }
}
