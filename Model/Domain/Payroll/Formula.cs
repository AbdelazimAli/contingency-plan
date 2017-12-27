using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain.Payroll
{
    [Table("Formulas")]
    public class Formula
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(30), Required]
        public string ShortName { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public byte Basis { get; set; } // 1-Hour 2-Day 3-Week 4-Month  5-Year  6-Period
        public byte Result { get; set; } // 1-Money 2-Units  3-Number  4-Result list  5-Message No.

        [MaxLength(3)]
        public string Curr { get; set; } // case of Monetary

        public byte FormType { get; set; } = 1; // 1-Formula Text  2-Stored function

        [MaxLength(1000)]
        public string FormText { get; set; }

        [MaxLength(100)]
        public string StoredName { get; set; }

        public int? InfoId { get; set; } // Search result in table
        public InfoTable Info { get; set; }
    }
}
