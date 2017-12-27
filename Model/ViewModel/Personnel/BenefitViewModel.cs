using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
  public class BenefitViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int  Code { get; set; }
        public string LocalName { get; set; }
       
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
       
        public DateTime? EndDate { get; set; }
    }
    public class BenefitFormViewModel
    {
       
        public int Id { get; set; }
        public int? Code { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }  
        public bool IsLocal { get; set; } = false;

        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }

        public decimal? MonthFees { get; set; }
        public short Coverage { get; set; } // BenefitCover look up code: 1-Employee   2-Employee+Spouse  3-Employee+Spouse+Childern    4-Family
        public byte? MaxFamilyCnt { get; set; }

        public byte EmpAccural { get; set; } // 1- Optional   2-As Employees assigned   3-After employees assigned in months
        public byte? WaitMonth { get; set; }
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
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public short BenefitClass { get; set; } 
        public int? CalenderId { get; set; }
        public byte PlanLimit { get; set; } = 1;
    }
    public class BenefitPlanViewModel
    {
        public int Id { get; set; }
        public int BenefitId { get; set; }
        public string PlanName { get; set; }

        public float? EmpPercent { get; set; }
        public decimal? EmpAmount { get; set; }
        public float? CompPercent { get; set; }
        public decimal? CompAmount { get; set; }
        public decimal? CoverAmount { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public IEnumerable<string> BenefitServs { get; set; }
       // public ICollection<BenefitServ> BenefitServs { get; set; }



    }

    public class BenefitPlanVM
    {
        public IEnumerable<BenefitPlanViewModel> inserted { get; set; }
        public IEnumerable<BenefitPlanViewModel> updated { get; set; }
        public IEnumerable<BenefitPlanViewModel> deleted { get; set; }
    }
}
