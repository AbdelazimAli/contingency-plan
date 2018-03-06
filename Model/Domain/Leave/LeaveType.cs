using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LeaveType
    {
        // Basic Data
        [Key]
        public int Id { get; set; }

        [Index("IX_LeaveTypeCode")]
        public int Code { get; set; }

        [MaxLength(100), Required, Index("IX_LeaveTypeName", IsUnique = true)]
        public string Name { get; set; }
        public short AbsenceType { get; set; } // look up user code AbsenceType
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public bool MustAddCause { get; set; } = false; // Employee must add leave reason

        // Leave Duration Calculation Options (collapsed)
        public bool ExDayOff { get; set; } = true; // Exclude calendar days off from leave duration
        public bool ExHolidays { get; set; } = true; // Exclude official holidays from leave duration
        public bool IncLeavePlan { get; set; } = true; // Excluded from leave plan validations
        public bool AllowFraction { get; set; } = false; // Allow day fractions
        public bool VerifyFraction { get; set; } = false; // Verify day fractions from attendance
        public bool ExWorkflow { get; set; } = false; // Excluded from workflow

        // Custom Leave Options  (collapsed)
        public byte? WaitingMonth { get; set; } // The waiting period in months for new employees
        public byte? MaxDays { get; set; }
        public short? MaxDaysInPeriod { get; set; } // Max allowed days in working life

        public int CalendarId { get; set; }
        public PeriodName Calendar { get; set; }
        //public bool? ExWorkService { get; set; } // Excluded from the actual period of service
        //public bool EffectOnPayroll { get; set; } = false; // not now

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
        public string Branches { get; set; } 

        // Special Conditions
        public short? Gender { get; set; }
        public short? Religion { get; set; }
        public short? MaritalStat { get; set; }
        public int? Nationality { get; set; }
        public short? MilitaryStat { get; set; }

        // Assignment Status
        public bool ChangAssignStat { get; set; } = false; // Change Assignment Status while leave
        public short? AssignStatus { get; set; } // Leave Assignment Status
        public byte? AutoChangStat { get; set; } // Return Assignment Status at the end of leave period
        //1-Automatic return to original status before leave
        //2-Return to original status on attendance
        //3-Manually update status


        // Accrual Plan
        //AccrualBal AccBalDays NofDays Balanace50 Age50 NofDays50  FractionsOpt PostOpt MaxNofDays DiffDaysOpt FirstMonth FirstYear
        public bool HasAccrualPlan { get; set; } = false; // Must Check Balance
        public bool AllowNegBal { get; set; } = false; // Allow negative balance
        public float? Percentage { get; set; } // Can't excced percentage of open balance
       // public byte? YearStartDt { get; set; } // 1- Specific Date   2- Employee Join Date
        public byte? WorkServMethod { get; set; } // 1-From join date   2-From start experience date  3-From last employment date 4-From last assignment  5-All employments  6-All Assignments
        //public byte? StartDay { get; set; } // In Case of Specific Date (1 - 30)
        //public byte? StartMonth { get; set; } // In Case of Specific Date (1 - 12)
        public byte? AccBalDays { get; set; } // Accrual Balance Days 1= FixedDays   2-According to total working services duration
        public byte MonthOrYear { get; set; } = 2; // 1- Month 2-Year
        public bool PercentOfActive { get; set; }
        /// 1- Fixed days       No. of days: 
        /// 2- According to the actual period of service within the year
        /// 3- According to the total period of service
        /// 4- Decision table
        /// 5- Formula
        /// In Case of 2 or 3 draw grid     From Years      To Years       No. of days
        ///                                 0                  1               15
        ///                                 1                  10              21
        ///                                 10                 30              30
        ///                                 30                 99              45
        public float? NofDays { get; set; }  // In Case of 1

        public bool Balanace50 { get; set; } = false; // Has a different accrual balance in specific age?
        public byte? Age50 { get; set; }
        public float? NofDays50 { get; set; }
        //public byte? FractionsOpt { get; set; } /// Fractions day options
                        /// 1- Convert to the nearest fraction 1/4, 1/2 , 3/4
                        /// 2- Convert to the nearest greater integer
                        /// 3- Abbreviated to the nearest integer
                        /// 4- Remove fractions

        public byte? PostOpt { get; set; } // Post balance options
        /// 1- Post all remindar days
        /// 2- Has maximum posted balance in days          No. of days
        /// 3- Has maximum percentage of open balance
        /// 4- Use formula to calculate posted days
        public float? MaxNofDays { get; set; } // In case of 2
        public float? MaxPercent { get; set; } // In case of 3
        // Day limits
        public float? MinLeaveDays { get; set; } // Employee must recieve minimum leave period
        public byte? IncludContinu { get; set; } // Including continuous days

        // In case of 1,2 or 3
        public byte? DiffDaysOpt   { get; set; } // If employee has balance greater than maximum allowed posted balance
                                               /// 1- generate time compensation transaction
                                               /// 2- don't post for this employee and add record in error posting log

        // Annual Leave Options (collapsed)
        //public bool PayBefore { get; set; } = false; // Pay salary before leave?
        //public int? PayrollId { get; set; }
        //public Payroll Payroll { get; set; }
        //public byte? Batch { get; set; } // Batch deductions type
        //                                 /// 1- Deduct from payroll expenses
        //                                 /// 2- Deduct from vacations and provisioning expenses

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class LeaveRange
    {
        [Key]
        public int Id { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public short FromPeriod { get; set; }
        public short ToPeriod { get; set; }
        public float NofDays { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
