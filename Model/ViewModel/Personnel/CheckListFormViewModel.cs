using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class CheckListFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LocalName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1); // Start effect date
        public DateTime? EndDate { get; set; } // End effect date
        public bool IsLocal { get; set; } = false;
        public bool Default { get; set; } = false;
        public int? CompanyId { get; set; }
        public byte ListType { get; set; } // 1- Employment Checklist  2- New Employee Orientation   3- Termination checklist
        public short Duration { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class CheckListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1); // Start effect date
        public DateTime? EndDate { get; set; } // End effect date
        public bool Default { get; set; } = false;
        public short Duration { get; set; }
        
    }

    public class CheckListTaskViewModel
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public short TaskNo { get; set; }
        public short? TaskCat { get; set; } // look up list
        public string Description { get; set; }
        public byte Priority { get; set; }

        public bool Required { get; set; } = false;

        public short? ExpectDur { get; set; }
        public byte? Unit { get; set; } // 1-Minute 2-Hour 3-Day 4-Week  5-Month
        public int? EmpId { get; set; } // Assigned to
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
    public class CheckListTaskVM
    {
        public IEnumerable<CheckListTaskViewModel> inserted { get; set; }
        public IEnumerable<CheckListTaskViewModel> updated { get; set; }
        public IEnumerable<CheckListTaskViewModel> deleted { get; set; }
    }


}
