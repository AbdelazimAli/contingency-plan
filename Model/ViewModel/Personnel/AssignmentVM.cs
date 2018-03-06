using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class AssignmentVM
    {
        public int EmpId { get; set; }
        public int BranchId { get; set; } // Branch = 2
        public int DepartmentId { get; set; } // Department
        public bool IsDepManager { get; set; }
        public int CompanyId { get; set; } // hidden employee company
        public int JobId { get; set; } // filter local jobs for login company or globals jobs 
        public int? PositionId { get; set; } // Position
        public int? ManagerId { get; set; } // Direct Manager (allowed only if position is not selected)
    }
}
