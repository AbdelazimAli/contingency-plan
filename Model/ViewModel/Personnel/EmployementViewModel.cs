using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmployementViewModel
    {

        [Key]
        public int Id { get; set; }
        public int EmpId { get; set; }      
        public short PersonType { get; set; } // Employement Type    
        [MaxLength(30)]
        public string Profession { get; set; }
        public byte Status { get; set; } // 1- Active   2-Inactive  3-Termination   4-Deleted
        public int? TerminationId { get; set; }
        public DateTime ContIssueDate { get; set; } = DateTime.Now;
        public byte DurInYears { get; set; }
        public  byte DurInMonths { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public bool AutoRenew { get; set; } = true;
        public int? RemindarDays { get; set; } = 10;
        public int? Sequence { get; set; }
        public int? Salary { get; set; }
        public int? Allowances { get; set; }
        public byte? TicketCnt { get; set; }
        public decimal? TicketAmt { get; set; }
        public int? FromCountry { get; set; }
        public int? ToCountry { get; set; }
        public string Curr { get; set; }
        public byte? VacationDur { get; set; }

        [MaxLength(500)]
        public string JobDesc { get; set; }

        [MaxLength(500)]
        public string BenefitDesc { get; set; }

        [MaxLength(500)]
        public string SpecialCond { get; set; }
        public string Error { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int SysCodeId { get; set; }

    }
}
