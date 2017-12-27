using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.ViewModel.Personnel
{

    public class HRCalendarViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }     
        public DateTime StartDate { get; set; }      
        public DateTime? EndDate { get; set; }
        public bool IsLocal { get; set; } = false;
        public bool Default { get; set; } = false;
        public int? CompanyId { get; set; }
        public byte PeriodLength { get; set; } = 1; // Period length in years
        public byte SubPeriodCount { get; set; } = 1; // 0,1,2,3,4,6
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class PeriodsViewModel
    {
        public int Id { get; set; }
        public int PeriodNo { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte Status { get; set; }
        public int CalendarId { get; set; }
        
    }
    public class SubPeriodsViewModel
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }      
        public int SubPeriodNo { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte Status { get; set; }
        public int? PayDueId { get; set; }
        public DateTime? CalcSalaryDate { get; set; } //Calulation date  must be working day
        public DateTime? PaySalaryDate { get; set; }
        public PayrollDue PayDue { get; set; }
    }

    public class PeriodListViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int typeId { get; set; }
    }
}
