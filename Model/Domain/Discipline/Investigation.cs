using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Investigation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime InvestDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime ViolDate { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Accident { get; set; }

        [MaxLength(1000)]
        public string Defense { get; set; }

        public int? ViolationId { get; set; }
        public Discipline Violation { get; set; }

        [MaxLength(1000)]
        public string InvestResult { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class InvestigatEmp
    {
        [Key, Column(Order =1)]
        public int InvestigatId { get; set; }

        [ForeignKey("InvestigatId")]
        public Investigation Investigation { get; set; }

        [Key, Column(Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        // hidden field to distinct employees
        public byte EmpType { get; set; } // 1-Violation Employee  2-Witness Employee  3-Investigation Judge
    }
}
