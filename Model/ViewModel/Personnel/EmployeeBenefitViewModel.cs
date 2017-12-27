using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmployeeBenefitViewModel
    {
        public int Id { get; set; }
        public int BenefitId { get; set; }
        public string  BenefitName { get; set; }
        public int EmpId { get; set; }
        public int sys { get; set; }
        public int? BeneficiaryId { get; set; }
        public string  BeneficiaryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int BenefitPlanId { get; set; }
        public string BenefitPlanName { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class EmpRelativeViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string RelationName { get; set; }
        public short Relation { get; set; } // Relations in look up code 1-Son 2-Daughter 3-Husband 4-Wife 5-Father 6-Mother
        public DateTime? BirthDate { get; set; }
        public string NationalId { get; set; }
        public string Telephone { get; set; }
        public string PassportNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string GateIn { get; set; }
        public string Entry { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
