using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveTypeFormViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public short AbsenceType { get; set; } // look up user code AbsenceType
        public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime? EndDate { get; set; }
        public bool HasAccrualPlan { get; set; } = false;
        public short? MaxDaysInPeriod { get; set; } // Max allowed days in working life
        public bool MustAddCause { get; set; } = false; // Employee must add leave reason
        public bool ExDayOff { get; set; } = true; // Exclude calendar days off from leave duration
        public bool ExHolidays { get; set; } = true; // Exclude official holidays from leave duration
        public bool AllowFraction { get; set; } = false; // Allow day fractions
        public bool VerifyFraction { get; set; } = false; // Verify day fractions from attendance
        public byte? WaitingMonth { get; set; } // The waiting period in months for new employees
        public byte? MaxDays { get; set; }
        public bool AllowNegBal { get; set; } = false; // Allow negative balance
        public float? Percentage { get; set; } // Can't excced percentage of open balance
        public IEnumerable<int> IPeopleGroups { get; set; } // comma seperated PeopleGroups
        public IEnumerable<int>   IPayrolls { get; set; } // comma seperated Payrolls
        public IEnumerable<int> IJobs { get; set; } // comma seperated Jobs   
        public IEnumerable<int> IEmployments { get; set; } // comma seperated Employments
        public IEnumerable<int> ICompanyStuctures { get; set; }
        public IEnumerable<int> IPositions { get; set; } 
        public IEnumerable<int> IPayrollGrades { get; set; } 
        public IEnumerable<int> IBranches { get; set; }
        public string  PeopleGroups { get; set; } // comma seperated PeopleGroups
        public string  Payrolls { get; set; } // comma seperated Payrolls
        public string Jobs { get; set; } // comma seperated Jobs   
        public string Employments { get; set; } // comma seperated Employments
        public string CompanyStuctures { get; set; }
        public string Positions { get; set; }
        public string PayrollGrades { get; set; }
        public string Branches { get; set; }
        public short? Gender { get; set; }
        public short? Religion { get; set; }
        public short? MaritalStat { get; set; }
        public int? Nationality { get; set; }
        public short? MilitaryStat { get; set; }
        public byte? AccBalDays { get; set; }                           
        public float? NofDays { get; set; } 
        public bool Balanace50 { get; set; } = false; 
        public byte? Age50 { get; set; }
        public float? NofDays50 { get; set; }
        public byte? PostOpt { get; set; } 
        public float? MaxNofDays { get; set; } 
        public byte? DiffDaysOpt { get; set; }
        //Added by Mamdouh
        public short? AssignStatus { get; set; }
        public bool ChangAssignStat { get; set; } = false;
        public byte? AutoChangStat { get; set; }
        public byte? WorkServMethod { get; set; }
        public float? MaxPercent { get; set; }
        public float? MinLeaveDays { get; set; } 
        public byte? IncludContinu { get; set; }
        public bool PercentOfActive { get; set; }
        //end
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte? FrequencyId { get; set; }
        public int? OpenCalendarId { get; set; }
        public int CalendarId { get; set; }
        public byte? MonthOrYear { get; set; } // 1- Month 2-Year
        public bool IncLeavePlan { get; set; } = true;
        public bool ExWorkflow { get; set; }
    }
}
