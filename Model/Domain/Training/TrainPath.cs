﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model.Domain
{
    public class TrainPath
    {
        // Basic Data
        [Key]
        public int Id { get; set; }

        [MaxLength(150), Required, Index("IX_TrainPath", IsUnique = true)]
        public string Name { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);  // Effective Date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(250)]
        public string Summary { get; set; } // Course Summary

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
        public string Branches { get; set; } // 

        // Special Conditions
        public int? Qualification { get; set; } // Qualification
        public short? QualRank { get; set; } // Qualification Rank
        public byte? YearServ { get; set; } // Total Year in Services
        public byte? Age { get; set; } // Age
        public short? Performance { get; set; } // Last Performance Grade form look up code
        public int? Formula { get; set; }

        // Traing Courses 
        public ICollection<TrainCourse> Courses { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
