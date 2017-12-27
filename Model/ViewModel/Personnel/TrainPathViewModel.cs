using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class TrainPathViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string LocalName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
    public class TrainPathFormViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool IsLocal { get; set; } = false;
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);  // Effective Date
        public DateTime? EndDate { get; set; }
        public string Summary { get; set; } // Course Summary

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
        // Special Conditions
        public int? Qualification { get; set; }
        public short? QualRank { get; set; }
        public byte? YearServ { get; set; }
        public byte? Age { get; set; }
        public short? Performance { get; set; }
        public int? Formula { get; set; }
        public IEnumerable<TrainPathCourseViewModel> Courses { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class TrainPathCourseViewModel
    {
        public int Id { get; set; } // CourseId
        public int TrainCourse_Id { get; set; }
        public int OldValue
        {
            get
            {
                return Id;
            }
        }
        public int TrainPath_Id { get; set; }
    }
    public class TrainPathCourseVM
    {
        public IEnumerable<TrainPathCourseViewModel> inserted { get; set; }
        public IEnumerable<TrainPathCourseViewModel> updated { get; set; }
        public IEnumerable<TrainPathCourseViewModel> deleted { get; set; }
    }
}
