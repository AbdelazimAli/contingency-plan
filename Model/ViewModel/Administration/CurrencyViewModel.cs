using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class CurrencyViewModel
    {
        //map Code prop in Currency Domain
        //Normal, Downward, Rounding Up

        public string Id { get; set; }
        public string Code { get; set; }       
        public string Name { get; set; }
        public string Symbol { get; set; } 
        public bool Suffix { get; set; } = true; 
        public string Isocode { get; set; } 

        public bool? Referenced { get; set; } 

        public byte? CalcRoundRule { get; set; } 
        public byte? PayRoundRule { get; set; }
        public int  CalcRoundRuleText { get; set; }
        public int  PayRoundRuleText { get; set; }
        public byte RoundMethod { get; set; } = 1;
        public string  RoundMethodText
        {
            get
            {
                switch (RoundMethod)
                {
                    case 2:
                        return "Downward";
                    case 3:
                        return "Rounding Up";
                    default:
                        return "Normal";
                }
            }
        }

        public byte Decimals { get; set; }
        public bool IsMultiplyBy { get; set; }
        public float MidRate { get; set; }

    }
}
