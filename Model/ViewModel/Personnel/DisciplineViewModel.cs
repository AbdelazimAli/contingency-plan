using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class DisciplineViewModel
    {

        public int Id { get; set; }

        // Basic Data
        public int? Code { get; set; }

        [MaxLength(250), Required]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public short DisciplineClass { get; set; } // look up code 

        // Discipline Period
        public int? PeriodId { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
    public class DisplinRepeatViewModel
    {
        public int Id { get; set; }

        public int DisplinId { get; set; }

        public byte RepNo { get; set; }

        public short DisplinType { get; set; } // look up user code 1-Verbal alert 2-Written Alert 3-Warning 4-Deduction from salary 5-Suspension 6-Deny promotion 7-Dismissal 8-Dismissal without reward

        // In Case of DisplinType = 4: Deduction from salary
        public float? NofDays { get; set; }

        // In Case of DisplinType = 5 or 6: Deduction from salary
        public byte? DenyPeriod { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
