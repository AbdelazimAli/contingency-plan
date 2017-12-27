using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class Employement
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Employement", IsUnique = false, Order = 1)]
        [Index("IX_EmployementDate", IsUnique = true, Order = 1)]
        public int EmpId { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int? Sequence { get; set; }

        [MaxLength(20), Required]
        public string Code { get; set; }

        [ForeignKey("EmpId")]
        public Person Person { get; set; }
        public short PersonType { get; set; } // Employement Type

        [MaxLength(30)]
        public string Profession { get; set; }
        /// <summary>
        /// doesn't appear in the form only appears in the index grid
        /// Employment      Start Date      End Date        Status
        /// Employee        01-01-2016      31-11-2016      Inactive     
        /// Ex-Employee     01-12-2016      01-12-2016      Termination
        /// Contract        01-01-2016      30-11-2016      Inactive
        /// Employee        01-12-2016                      Active          Delete
        /// Popup form      Emlopyment:      Start Date:
        /// 
        /// </summary>
        /// 
        [Index("IX_Employement", IsUnique = false, Order = 2)]
        public byte Status { get; set; } // 1- Active   2-Inactive  3-Termination   4-Deleted

        public int? TerminationId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ContIssueDate { get; set; }

        public byte DurInYears { get; set; }
        public byte DurInMonths { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_EmployementDate", IsUnique = true, Order = 2)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public bool AutoRenew { get; set; } = true;
        public int? RemindarDays { get; set; }

        public int? Salary { get; set; }
        public int? Allowances { get; set; }

        [MaxLength(3), Column(TypeName = "char")]
        public string Curr { get; set; }

        [ForeignKey("Curr")]
        public Currency Curreny { get; set; }
        public byte? TicketCnt { get; set; }
        public decimal? TicketAmt { get; set; }
        public int? FromCountry { get; set; }
        public int? ToCountry { get; set; }

        public byte? VacationDur { get; set; }

        public int? SuggestJobId { get; set; } // Suggested Job
        public Job SuggestJob { get; set; } 

        [MaxLength(500)]
        public string JobDesc { get; set; }

        [MaxLength(500)]
        public string BenefitDesc { get; set; }

        [MaxLength(500)]
        public string SpecialCond { get; set; }

        public bool Renew { get; set; } = false;


        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
