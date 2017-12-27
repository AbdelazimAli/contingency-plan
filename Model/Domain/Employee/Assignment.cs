using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class Assignment
    {
        // Assignment Information
        [Key]
        public int Id { get; set; }

        [Index("IX_EmpAssignment", IsUnique = true, Order = 1)]
        public int EmpId { get; set; }

        [Index("IX_EmpAssignment", IsUnique = true, Order = 2), Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public DateTime AssignDate { get; set; } // Assignment Date

        [Index("IX_EmpAssignment", IsUnique = true, Order = 3), Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = new DateTime(2999, 1, 1); // End Date 

        [Required, MaxLength(20)]
        public string Code { get; set; }
               
        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public short AssignStatus { get; set; } // Assignment Status  look user code Assignment                        
        //public bool Primary { get; set; } // Employee can have only one Active & Primary Assignment
        public short SysAssignStatus { get; set; }

        [Index("IX_AssignmentBranch")]
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public int DepartmentId { get; set; } // Department
        public CompanyStructure Department { get; set; } // company structure for login company

        [Index("IX_AssignmentCompany")]
        public int CompanyId { get; set; } // hidden employee company
        public bool IsDepManager { get; set; } = false; // Is Department Manager
        public int JobId { get; set; } // filter local jobs for login company or globals jobs 
        public Job Job { get; set; } // Job
        public int? PositionId { get; set; } // Position
        public Position Position { get; set; } // Optional filter positions only from selected job
        public int? LocationId { get; set; } // filter local locations for login company or globals locations ((IsLocal == false) || (IsLocal && CompanyId == 0)) && IsInternal == true;
        public Location Location { get; set; } // Location
        public int? GroupId { get; set; } // People Group

        [ForeignKey("GroupId")]
        public PeopleGroup PeopleGroup { get; set; }

        // Salary Information
        public int? PayrollId { get; set; }
        public Payrolls Payroll { get; set; }
        public short? SalaryBasis { get; set; } // look up user code
        public int? PayGradeId { get; set; }

        [ForeignKey("PayGradeId")]
        public PayrollGrade PayrollGrade { get; set; }
       
        public short? Performance { get; set; } // Last performance review
        //public short? Frequency { get; set; }
        //public int? PayrollLadder { get; set; }

        // Administration Information
        public int? CareerId { get; set; } // Career Path

        [ForeignKey("CareerId")]
        public CareerPath CareerPath { get; set; }
        public int? ManagerId { get; set; } // Direct Manager (allowed only if position is not selected)
        public byte? ProbationPrd { get; set; } // Probation period in months
        public byte? NoticePrd { get; set; } // Notice Period in months

        // Eligibility Criteria
        public byte? EmpTasks { get; set; } // 1-Employee whose direct managed  2-Use eligibility criteria
        [MaxLength(50)]
        public string PeopleGroups { get; set; } // comma seperated PeopleGroups

        [MaxLength(50)]
        public string Payrolls { get; set; } // comma seperated Payrolls

        [MaxLength(50)]
        public string Jobs { get; set; } // comma seperated Jobs

        [MaxLength(50)]
        public string Employments { get; set; } // comma seperated Employments

        [MaxLength(50)]
        public string CompanyStuctures { get; set; } // comma seperated CompanyStuctures: Departments

        [MaxLength(50)]
        public string Positions { get; set; } // comma seperated Positions

        [MaxLength(50)]
        public string PayrollGrades { get; set; } // comma seperated Payroll Grades

        [MaxLength(50)]
        public string Locations { get; set; } // comma seperated Payroll Grades

        // Salary Items

        // Distributions of cost centers

        // Distributions of salary payments 

        // Provisions

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
