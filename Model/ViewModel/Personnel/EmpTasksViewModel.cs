using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpTasksViewModel
    {
        public int Id { get; set; }
        public string EmpList { get; set; }
        public short? TaskNo { get; set; }
        public string TaskCategory { get; set; }  // look up list

        [MaxLength(250)]
        public string Description { get; set; }
        public byte Priority { get; set; }
        public byte Status { get; set; } // 0-New  1-Assigned to employee 2-Done 3-Canceled  4-Not done

        public int? SubPeriodId { get; set; }
        public string SubPeriod { get; set; }
        public int? PeriodId { get; set; }
        public string Period { get; set; }


        public int? EmpId { get; set; } // Assigned to employee
        public string Employee { get; set; }
        public bool isStart { get; set; }

        public int? ManagerId { get; set; } // Assigned by
        public string Manager { get; set; }

        public DateTime? AssignedTime { get; set; }
        public int? EmpListId { get; set; }

        public bool Required { get; set; }
        public short? ExpectDur { get; set; } // Expected duration
        public byte? Unit { get; set; } = 3; // 1-Minute 2-Hour 3-Day 4-Week  5-Month

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public short Duration { get; set; } // in the same expected unit
        public int Attachments { get; set; }
        public short? TaskCat { get; set; }

        public bool ChangeEmployee { get; set; }
    }

}
