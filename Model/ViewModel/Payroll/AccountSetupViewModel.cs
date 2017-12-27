using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class AccountSetupViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public byte AccType { get; set; } // 1-GL Account   2-App Account   3-Cost Center
        [MaxLength(1)]
        public string Spiltter { get; set; } // . - / \ 
        public byte Seq { get; set; }   // 1 to 10
        [MaxLength(20)]
        public string Segment { get; set; }
        public byte SegLength { get; set; } // Max 10
    }
}
