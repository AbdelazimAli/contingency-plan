using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Payroll
{
    public class Currency
    {
        [Key, MaxLength(3), Column(TypeName = "char")]
        public string Code { get; set; }

        [Required, MaxLength(30), Index("IX_CurrencyName", IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(3)]
        public string Symbol { get; set; } // $
        public bool Suffix { get; set; } = true; //  $100 or 100$

        [MaxLength(3), Column(TypeName = "char")]
        public string Isocode { get; set; } // Iso Code

        public bool? Referenced { get; set; } // Reference Currency for Triangulation like dollar and Euro

        public byte? CalcRoundRule { get; set; } // Calculation Rounding Rule 0.001, 0.01, 0.1, 0.25, 0.5, 1, 10
        public byte? PayRoundRule { get; set; } // Payment Rounding Rule 0.001, 0.01, 0.1, 0.25, 0.5, 1, 10
        public byte RoundMethod { get; set; } = 1; // Normal, Downward, Rounding Up
        public byte Decimals { get; set; }
        public bool IsMultiplyBy { get; set; }
        public float MidRate { get; set; }
    }
}
