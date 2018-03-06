using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class WfTrans
    {
        [Key]
        public int Id { get; set; }
        public int WFlowId { get; set; }

        [MaxLength(20)]
        [Index("IX_WfTrans", IsUnique = true, Order = 1)]
        public string Source { get; set; }

        [Index("IX_WfTrans", IsUnique = true, Order = 2)]
        public int SourceId { get; set; } 

        [Index("IX_WfTrans", IsUnique = true, Order = 3)]
        public int DocumentId { get; set; }

        [Index("IX_WfTrans", IsUnique = true, Order = 4)]
        public short Sequence { get; set; }
        public byte Order { get; set; }
        public Guid? RoleId { get; set; } // Role for all
        public short? CodeId { get; set; } // Hierarchy type == 1 Only
        public int? DeptId { get; set; } //
        public int? PositionId { get; set; } // Hierarchy type == 3 Only
        public int? EmpId { get; set; } // Hierarchy type == 4 Only
        public int CompanyId { get; set; } // Branch = 2
        public int? BranchId { get; set; } // Branch = 2
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; } 
        public int? AuthEmp { get; set; } 
        public byte ApprovalStatus { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
