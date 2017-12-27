using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LeavePosting
    {
        [Key]
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public bool Posted { get; set; }

        [MaxLength(100)]
        public string Reason { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
