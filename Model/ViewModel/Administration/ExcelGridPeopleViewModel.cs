using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelGridPeopleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string localName { get; set; }
        public string FirstName { get; set; }
        public string Fathername { get; set; }
        public string GFathername { get; set; }
        public string Familyname { get; set; }
        public short? Gender { get; set; }
        public string NationalId { get; set; }
        public DateTime? IdIssueDate { get; set; }
        public string Ssn { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? StartExpDate { get; set; } // Display Experience: 9Y as integer label  
        public int? QualificationId { get; set; }
        public DateTime? BirthDate { get; set; }  // Display Age: 36Y as integer label
        public string BirthLocation { get; set; }
        public int? BirthCountry { get; set; } // from World view one column update three fields
        public int? BirthCity { get; set; }
        public int? BirthDstrct { get; set; }
        public int? Nationality { get; set; }   // Auto display from birth country and can be changed
        public short? MaritalStat { get; set; } // User Code 1-Single 2-Engaged 3-Married 4-Widow 5-Divorced
        public byte? TaxFamlyCnt { get; set; } // Family count in Tax
        public byte? BnftFamlyCnt { get; set; } // Family count in Benefits
        public short? Religion { get; set; } // 1-Muslim 2-Christian
        public string Address { get; set; }
        public string HostAddress { get; set; }
        public int? AddressId { get; set; }
        public int? HoAddressId { get; set; }
        public bool AutoRenew { get; set; }
        public int? RemindarDays { get; set; }
        public string Mobile { get; set; }
        public string HomeTel { get; set; }
        public string EmergencyTel { get; set; }
        public string WorkTel { get; set; }
        public string WorkEmail { get; set; }
        public string OtherEmail { get; set; }
        public byte? MilitaryStat { get; set; } // 1-Performed 2-Exempt 3-Deferred 4-Under age
        public DateTime? MilStatDate { get; set; }
        public string MilitaryNo { get; set; }
        public DateTime? MilResDate { get; set; } // Transfer to Reserve date
        public short? Rank { get; set; } // lookup
        public short? MilCertGrade { get; set; } // Certificate grade -- lookup
        public string PassportNo { get; set; }
        public string VisaNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuePlace { get; set; }
        public string Profession { get; set; }
        public int? KafeelId { get; set; }
        public short? MedicalStat { get; set; } // lookup
        public DateTime? MedStatDate { get; set; }
        public DateTime? InspectDate { get; set; }
        public int? ProviderId { get; set; }
        public short? BloodClass { get; set; } // O-, O+, AB-, AB+, B-, B+, A-, A+
        public string Recommend { get; set; }
        public short? RecommenReson { get; set; }
        public int? LocationId { get; set; }
        public string RoomNo { get; set; }
        public DateTime? SubscripDate { get; set; }
        public decimal? BasicSubAmt { get; set; }
        public decimal? VarSubAmt { get; set; }

        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string Code { get; set; }
        public short? PersonType { get; set; } // Employement Type
        public string EmpProfession { get; set; }
        public DateTime? ContIssueDate { get; set; }

        public byte DurInYears { get; set; }
        public byte DurInMonths { get; set; }
        public DateTime? StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }

        public int? Salary { get; set; }
        public int? Allowances { get; set; }
        public string Curr { get; set; }

        public byte? TicketCnt { get; set; }
        public decimal? TicketAmt { get; set; }
        public int? FromCountry { get; set; }
        public int? ToCountry { get; set; }
        public byte? VacationDur { get; set; }
        public string JobDesc { get; set; }
        public string BenefitDesc { get; set; }
        public string SpecialCond { get; set; }
        public DateTime AssignDate { get; set; } // Assignment Date
        public short AssignStatus { get; set; } // Assignment Status  look user code Assignment
        public int DepartmentId { get; set; } // Department
        public bool IsDepManager { get; set; } = false; // Is Department Manager
        public int JobId { get; set; } // filter local jobs for login company or globals jobs 
        public int? PositionId { get; set; } // Position
        public int? AssignLocation { get; set; }
        public short SysAssignStatus { get; set; }
        public int? GroupId { get; set; } // People Group
        public int? PayrollId { get; set; }
        public short? SalaryBasis { get; set; } // look up user code
        public int? PayGradeId { get; set; }
        public short? Performance { get; set; } // Last performance review
        public int? CareerId { get; set; } // Career Path
        public int? ManagerId { get; set; } // Direct Manager (allowed only if position is not selected)
        public byte? ProbationPrd { get; set; } // Probation period in months
        public byte? NoticePrd { get; set; } // Notice Period in months
        public byte? EmpTasks { get; set; } // 1-Employee whose direct managed  2-Use eligibility criteria
        public bool AssignError { get; set; } = false;


    }
}