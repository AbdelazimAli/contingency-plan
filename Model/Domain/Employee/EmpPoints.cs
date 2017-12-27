using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class EmpPoints
    {
        [Key]
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public DisPeriodNo Period { get; set; }
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public int Balance { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
