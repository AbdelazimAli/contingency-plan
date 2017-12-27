using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class EmpTask
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? EmpListId { get; set; }

        [ForeignKey("EmpListId")]
        public EmpChkList EmpChklist { get; set; }
        public short? TaskNo { get; set; }
        public short? TaskCat { get; set; } // look up list

        [MaxLength(250)]
        public string Description { get; set; }
        public byte Priority { get; set; }
        public byte Status { get; set; } // 0-New  1-Assigned to employee 2-Done 3-Canceled  4-Not done

        public bool Required { get; set; }
        public short? ExpectDur { get; set; } // Expected duration
        public byte? Unit { get; set; } = 3; // 1-Minute 2-Hour 3-Day 4-Week  5-Month
        public int? EmpId { get; set; } // Assigned to employee

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public int? ManagerId { get; set; } // Assigned by

        [ForeignKey("ManagerId")]
        public Person Manager { get; set; }

        public int? SubPeriodId { get; set; }
        public SubPeriod SubPeriod { get; set; }

        public DateTime? AssignedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public short Duration { get; set; } // in the same expected unit

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
