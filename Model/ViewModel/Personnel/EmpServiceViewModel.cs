using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpServiceViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string  Period { get; set; }
        public int? BeneficiaryId { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalCompCost { get; set; }
        public decimal? TotalEmpCost { get; set; }
        public int? PeriodId { get; set; }
        public decimal? Balance { get; set; }
        public string PlanName { get; set; }
        public byte PlanLimit { get; set; } = 1;
    }
}
