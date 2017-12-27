using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
     public class AssignmentGridViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public int? SectorId { get; set; } 
        public string Employee { get; set; }
        public Double Docs { get; set; }
        public string Code { get; set; }
        public DateTime? EmployementDate { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string Location { get; set; }
        public int EmpStatus { get; set; }
        public int Age { get; set; }
        public int? Qualification { get; set; }
        public int? PersonType { get; set; }
        public string Job { get; set; }
        public int? JobId { get; set; }
        public string Department { get; set; }
        public int? DepartmentId { get; set; }
        public string Position { get; set; }
        public int? PositionId { get; set; }
        public int? ManagerId { get; set; }
        public int? LocationId { get; set; }
        public int? Supervisor { get; set; }
        public bool HasImage { get; set; }
        public string PicUrl { get; set; }
        public bool isActive { get; set; }
        public int MyProperty { get; set; }
        public int? PayrollId { get; set; }
        public int? PayGradeId { get; set; }
        public int? GroupId { get; set; }
        public int Gender { get; set; }
        public bool IsDeptManger { get; set; }
        public int? Nationality { get; set; }
        public string Attachement { get; set; }
    }
}
