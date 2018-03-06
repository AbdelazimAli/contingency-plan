using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpChkListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
        public short Gender { get; set; }
        public string Description { get; set; }
        public int? EmpId { get; set; } // Employee
        public int? ListId { get; set; }
        public DateTime ListStartDate { get; set; } = new DateTime(2000, 1, 1); // List start date
        public DateTime? ListEndDate { get; set; } // List end date & Duration
        public byte ListType { get; set; } // 1- Employment Checklist  2- New Employee Orientation   3- Termination checklist
        public byte Status { get; set; } = 0; // 0-New 1-Done  2-Canceled
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int CompanyId { get; set; }
        public string Employee { get; set; }
        public int PrograssBar { get; set; }
        public int Count { get; set; }
        public int? ManagerId { get; set; }
    }
}
