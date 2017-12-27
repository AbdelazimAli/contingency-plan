using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("V_WF_TRANS")]
    public class WF_TRANS
    {
        public int WFlowId { get; set; }

        [Key, Column(Order = 1)]
        public string Source { get; set; }

        [Key, Column(Order = 2)]
        public int SourceId { get; set; }

        [Key, Column(Order = 3)]
        public int DocumentId { get; set; }
        public short Sequence { get; set; }
        public byte Order { get; set; }
        public Guid? RoleId { get; set; } // Role for all
        public short? CodeId { get; set; } // Hierarchy type == 1 Only
        public int? DeptId { get; set; } //
        public int? PositionId { get; set; } // Hierarchy type == 3 Only
        public int? EmpId { get; set; } // Hierarchy type == 4 Only
        public int CompanyId { get; set; } 
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public byte ApprovalStatus { get; set; }
        public string Message { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}