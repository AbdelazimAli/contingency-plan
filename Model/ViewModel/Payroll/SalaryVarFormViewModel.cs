using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class SalaryVarFormViewModel
    {
        [Key]
        public string Id { get; set; }
        public int PayrollId { get; set; }
        public int PayPeriodId { get; set; }
        public byte Status { get; set; } = 0; // 0-New 1-Approved 2-Deleted
        public int SalItemId { get; set; }     
        public decimal Amount { get; set; }
        public string Curr { get; set; }
        [MaxLength(20)]
        public string Approvedby { get; set; }
        public Guid Reference { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool submit  { get; set; }

        public IEnumerable<int> IPeopleGroups { get; set; } // comma seperated PeopleGroups
        public IEnumerable<int> IPayrolls { get; set; } // comma seperated Payrolls
        public IEnumerable<int> IJobs { get; set; } // comma seperated Jobs   
        public IEnumerable<int> IEmployments { get; set; } // comma seperated Employments
        public IEnumerable<int> ICompanyStuctures { get; set; }
        public IEnumerable<int> IPositions { get; set; }
        public IEnumerable<int> IPayrollGrades { get; set; }
        public IEnumerable<int> ILocations { get; set; }
        public string PeopleGroups { get; set; } // comma seperated PeopleGroups
        public string Payrolls { get; set; } // comma seperated Payrolls
        public string Jobs { get; set; } // comma seperated Jobs   
        public string Employments { get; set; } // comma seperated Employments
        public string CompanyStuctures { get; set; }
        public string Positions { get; set; }
        public string PayrollGrades { get; set; }
        public string Locations { get; set; }



    }
}
