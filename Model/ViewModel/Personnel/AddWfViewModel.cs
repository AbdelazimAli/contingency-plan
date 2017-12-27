using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class AddWfViewModel
    {
        public int WorkFlowId { get; set; }
        public string Source { get; set; } // Leave
        public int SourceId { get; set; } // ex: leave type Id
        public byte HeirType { get; set; } // Hierarchy type 1-Org Chart  2-Org Chart Hierarchy  3-Position Hierarchy  4-Employee-Manager
        public int? Hierarchy { get; set; }
        public byte? NofApprovals { get; set; }
        public byte? TimeOutDays { get; set; } // Waiting Days
        public byte? TimeOutAction { get; set; } // Waiting Action: 1- Back to requester  2- Back to previous step 3- Foreword to next step
        public int? Id { get; set; }
        public short? Sequence { get; set; }
        public byte? Order { get; set; }
        public Guid? RoleId { get; set; }
        public short? CodeId { get; set; } 
        public byte? ApprovalStatus { get; set; }
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
    }
    
}
