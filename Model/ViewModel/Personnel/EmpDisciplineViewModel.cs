using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmpDisciplineViewModel
    {
        public int Id { get; set; } = 0;
        public int PhotoId { get; set; }

        public string EmpId { get; set; }     
        public int DiscplinId { get; set; } // Violiation
        [DataType(DataType.Date)]
        public DateTime ViolDate { get; set; } // Violiation Date

        public int? SuggDispline { get; set; }  // Suggested Discipline

        // In Case of DisplinType = 5-Suspension 6-Deny promotion
        public byte? SuggPeriod { get; set; }  // Suggested period in months

        // In Case of DisplinType = 4: Deduction from salary
        public float? SuggNofDays { get; set; }

        public int? ActDispline { get; set; } // Actual Discipline

        public byte? ActualPeriod { get; set; }  // Actual period in months
        // In Case of DisplinType = 4: Deduction from salary
        public float? ActualNofDays { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }       
        public IEnumerable<int> IWitness { get; set; }
        public string Witness { get; set; }
        [MaxLength(500)]
        public string Summary { get; set; }
        [MaxLength(500)]
        public string Defense { get; set; }
        public short? DeductPoint { get; set; } // in case of point period

        [DataType(DataType.Date)]
        public DateTime? EffectEDate { get; set; } // in case of open period
        public string DescionNo { get; set; }
        public DateTime? DescionDate { get; set; }
        public int? Manager { get; set; }
        public int? PeriodId { get; set; }
        public int DeptId { get; set; }
        public int? BranchId { get; set; }
        public int? SectorId { get; set; }
        public int? PositionId { get; set; }
        public bool HasImage { get; set; }
        public int EmpStatus { get; set; }
        public int empId { get; set; }
        public string Discplin { get; set; }
    }
}
