using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpDisciplineFormViewModel
    {
        public int Id { get; set; } = 0;
        public int EmpId { get; set; }
        public int DiscplinId { get; set; } // Violiation
        [DataType(DataType.Date)]
        public DateTime ViolDate { get; set; } // Violiation Date
        public int? InvestigatId { get; set; }
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
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int Attachments { get; set; }
    }
}
