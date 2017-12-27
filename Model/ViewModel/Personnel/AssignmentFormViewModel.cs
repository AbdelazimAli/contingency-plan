using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class AssignmentFormViewModel
    {
      
        public int Id { get; set; }

        public string Employee { get; set; }
        public int EmpId { get; set; }
        public int? BranchId { get; set; }
        public int? SectorId { get; set; }
        public string BranchName { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public bool Active { get; set; } = true; // Employee has only one active assignment for this code

        [DataType(DataType.Date)]
        public DateTime AssignDate { get; set; } // Assignment Date

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } 

        [Required, MaxLength(20)]
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public short AssignStatus { get; set; } // Assignment Status  look user code Assignment
                                                //public bool Primary { get; set; } // Employee can have only one Active & Primary Assignment
        public short SysAssignStatus { get; set; }
        public int DepartmentId { get; set; } // Department
        public int CompanyId { get; set; } // hidden login company
        public bool IsDepManager { get; set; } = false; // Is Department Manager
        public int JobId { get; set; } // filter local jobs for login company or globals jobs 
        public int? PositionId { get; set; } // Position
      
        public int? LocationId { get; set; } // filter local locations for login company or globals locations ((IsLocal == false) || (IsLocal && CompanyId == 0)) && IsInternal == true;
        public int? GroupId { get; set; } // People Group

     
        // Salary Information
        public int? PayrollId { get; set; }
      
        public short? SalaryBasis { get; set; } // look up user code
        public int? PayGradeId { get; set; }

       
        public int? CareerId { get; set; } // Career Path
        public int? ManagerId { get; set; } // Direct Manager (allowed only if position is not selected)
        public byte? ProbationPrd { get; set; } // Probation period in months
        public byte? NoticePrd { get; set; } // Notice Period in months

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
        public byte? EmpTasks { get; set; }
        public short? Performance { get; set; }



    }
}
