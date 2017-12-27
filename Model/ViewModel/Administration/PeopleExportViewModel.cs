using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class PeopleExportViewModel
    {
        public List<FormList> Gender { get; set; }
        public List<FormList> QualificationId { get; set; }
        public List<FormList> BirthLocation { get; set; }
        public List<FormList> Nationality { get; set; }
        public List<FormList> MaritalStat { get; set; }
        public List<FormList> Religion { get; set; }
        public List<FormList> MedicalStat { get; set; }
        public List<FormList> BloodClass { get; set; }
        public List<FormList> RecommenReson { get; set; }
        public List<FormList> SalaryBasis { get; set; }
        public List<FormList> PersonType { get; set; }
        public List<FormList> AssignStatus { get; set; }
        public List<FormList> MilitaryStat { get; set; }
        public List<FormList> ProviderId { get; set; }
        public List<FormList> LocationId { get; set; }
        public List<FormList> AssignLocation { get; set; }
        public List<FormList> KafeelId { get; set; }
        public IEnumerable Curr { get; set; }
        public List<FormList> ToCountry { get; set; }
        public List<FormList> FromCountry { get; set; }
        public List<FormList> JobId { get; set; }
        public List<FormList> DepartmentId { get; set; }
        public List<FormList> PayrollId { get; set; }
        public List<FormList> PositionId { get; set; }
        public List<FormList> PayGradeId { get; set; }
        public List<FormList> GroupId { get; set; }
        public List<FormList> CareerId { get; set; }
        public List<FormList> Performance { get; set; }
        public List<FormList> ManagerId { get; set; }
        public List<FormList> MilCertGrade { get; set; }
        public List<FormList> Rank { get; set; }
        public List<FormList> EmpTasks { get; set; }
        public List<FormList> DefaultGradeId { get; set; }
        public List<FormList> Frequency { get; set; }
        public List<FormList> TimeZone { get; set; }
        public List<FormList> CustodyCatId { get; set; }

    }
}
