using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpTaskViewModel
    {
        public int Id { get; set; }
        public int? EmpListId { get; set; }

        public short? TaskNo { get; set; }
        public short? TaskCat { get; set; } // look up list
        public string Description { get; set; }
        public byte Priority { get; set; }
        public byte Status { get; set; } // 0-New  1-Assigned to employee 2-Done 3-Canceled  4-Not done
        public int? SubPeriodId { get; set; }

        public bool Required { get; set; }
        public short? ExpectDur { get; set; } // Expected duration
        public byte? Unit { get; set; } = 3; // 1-Minute 2-Hour 3-Day 4-Week  5-Month
        public int? EmpId { get; set; } = null; // Assigned to employee
        public int? ManagerId { get; set; } = null; // Assigned by
        public DateTime? AssignedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public short Duration { get; set; } // in the same expected unit
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int PrograssBar { get; set; }
    }
}
