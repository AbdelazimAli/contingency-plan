using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Benefit
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Benefit")]
        public int Code { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }  //Ex  Life Insurance, Health Insurance, Dentist 

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public decimal? MonthFees { get; set; }
        public short Coverage { get; set; } // BenefitCover look up code: 1-Employee   2-Employee+Spouse  3-Employee+Spouse+Childern    4-Family
        public byte? MaxFamilyCnt { get; set; }

        public byte EmpAccural { get; set; } // 1- Optional   2-As Employees assigned   3-After employees assigned in months
        public byte? WaitMonth { get; set; }

        // Link to payroll
        // Payroll 
        // Employee share salary item
        // Company share salary item

        // Eligibility Criteria
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
        public string Branches { get; set; } // comma seperated Payroll Grades

        public short BenefitClass { get; set; } // look up user code
        public int? CalenderId { get; set; }
        public PeriodName Calender { get; set; }
        public byte PlanLimit { get; set; } = 1; // Check Plan limit against 1-Company cost   2-Employee cost  3-Total service cost

        public int? BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    } 
}
