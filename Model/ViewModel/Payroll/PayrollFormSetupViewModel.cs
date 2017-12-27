using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class PayrollFormSetupViewModel
    {      
        public int CompanyId { get; set; }       
        public bool MultiCurr { get; set; } = true; // Allow multiple currencies
        public int? DiffDebitAcct { get; set; } // Currency differences debit account     
      
        public int? DiffCreditAcct { get; set; } // Currency differences credit account    
        public int? DebitSettSal { get; set; } // Debit settlements account

        public int? CreditSettSal { get; set; } // Credit settlements account     

        // Payroll grade setting
        public byte GradeName { get; set; } // Payroll grade naming: 1-One segment  2-Multiple segments

        [MaxLength(1)]
        public string Spiltter { get; set; } // . - / \ 

        [MaxLength(100)]
        public string Group { get; set; } // Groups:

        [MaxLength(100)]
        public string Grade { get; set; } // Grades

        [MaxLength(100)]
        public string SubGrade { get; set; } // SubGrades
        public bool AutoApprovSalVal { get; set; } = true;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool submit { get; set; }
    }
}
