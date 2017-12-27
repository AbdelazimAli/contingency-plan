using Model.Domain;
using Model.Domain.Payroll;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelPeopleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string localName { get; set; }
        public string FirstName { get; set; }
        public string Fathername { get; set; }
        public string GFathername { get; set; }
        public string Familyname { get; set; }
        public string Gender { get; set; }
        public string NationalId { get; set; }
        public string IdIssueDate { get; set; }
        public string Ssn { get; set; }
        public string JoinDate { get; set; }
        public string StartExpDate { get; set; } 
        public string QualificationId { get; set; }
        public string Qualification { get; set; }
        public int CompanyId { get; set; }
        public string AutoRenew { get; set; }
        public string RemindarDays { get; set; }
        public string BirthDate { get; set; }  
        public string BirthLocation { get; set; } 
        public string Nationality { get; set; }  
        public string MaritalStat { get; set; } 
        public string TaxFamlyCnt { get; set; } 
        public string BnftFamlyCnt { get; set; } 
        public string Religion { get; set; } 
        public string AddressId { get; set; }
        public string Address { get; set; }
        public string HoAddressId { get; set; }
        public string HostAddress { get; set; }
        public string Mobile { get; set; }
        public string HomeTel { get; set; }
        public string EmergencyTel { get; set; }
        public string WorkTel { get; set; }
        public string WorkEmail { get; set; }
        public string OtherEmail { get; set; }
        public string MilitaryStat { get; set; } 
        public string MilStatDate { get; set; }
        public string MilitaryNo { get; set; }
        public string MilResDate { get; set; } 
        public string Rank { get; set; } 
        public string MilCertGrade { get; set; } 
        public string PassportNo { get; set; }
        public string VisaNo { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string IssuePlace { get; set; }
        public string Profession { get; set; }
        public string KafeelId { get; set; }
        public string Kafeel { get; set; }
        public string MedicalStat { get; set; }
        public string MedStatDate { get; set; }
        public string InspectDate { get; set; }
        public string ProviderId { get; set; }
        public int Provider { get; set; }
        public string BloodClass { get; set; } 
        public string Recommend { get; set; }
        public string RecommenReson { get; set; }
        public string LocationId { get; set; }
        public string RoomNo { get; set; }
        public bool HasImage { get; set; } = false;
        public string SubscripDate { get; set; }
        public string BasicSubAmt { get; set; }
        public string VarSubAmt { get; set; }

        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string Sequence { get; set; }
        public string Code { get; set; }
        public string PersonType { get; set; } 
        public string ContIssueDate { get; set; }
        public string EmpProfession { get; set; }

        public string DurInYears { get; set; }
        public string DurInMonths { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Salary { get; set; }
        public string Allowances { get; set; }
        public string Curr { get; set; }
        public string Curreny { get; set; }
        public string TicketCnt { get; set; }
        public string TicketAmt { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public string VacationDur { get; set; }
        public int? SuggestJobId { get; set; } 
        public string SuggestJob { get; set; }
        public string JobDesc { get; set; }
        public string BenefitDesc { get; set; }
        public string SpecialCond { get; set; }
        public string AssignDate { get; set; } 
        public string AssignStatus { get; set; } 
        public string DepartmentId { get; set; } 
        public string Department { get; set; } 

        public string IsDepManager { get; set; } 
        public string JobId { get; set; } 
        public string PositionId { get; set; }
       
        public string GroupId { get; set; }

        // Salary Information
        public string PayrollId { get; set; }
        public string SalaryBasis { get; set; }
        public string PayGradeId { get; set; }
        public string Performance { get; set; } 
        public string CareerId { get; set; }
        public string AssignLocation { get; set; }
        public string ManagerId { get; set; }
        public string ProbationPrd { get; set; }
        public string NoticePrd { get; set; } 
        public string EmpTasks { get; set; }
        public string PeopleGroups { get; set; } 
        public string Payrolls { get; set; } 
        public string Jobs { get; set; } 
        public string Employments { get; set; } 
        public string CompanyStuctures { get; set; }
        public string Positions { get; set; } 
        public string PayrollGrades { get; set; } 
        public string Locations { get; set; } 
    }
}
