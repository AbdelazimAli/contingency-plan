using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class TrainCourseViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
    public class TrainCourseFormViewModel
    {
       
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool IsLocal { get; set; } = false;
      //  public int? CompanyId { get; set; }
        public short? CourseCat { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);  
        public DateTime? EndDate { get; set; }
        public string Summary { get; set; }
        public string Whom { get; set; }
        public string Requirements { get; set; } 
        public short? PlannedHours { get; set; }
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
        public string PrevCourses { get; set; }
        public IEnumerable<int> IPrevCourses { get; set; }
        public int? Qualification { get; set; } // Qualification
        public short? QualRank { get; set; } // Qualification Rank
        public byte? YearServ { get; set; } // Total Year in Services
        public byte? Age { get; set; } // Age
        public short? Performance { get; set; } // Last Performance Grade form look up code
        public int? Formula { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
