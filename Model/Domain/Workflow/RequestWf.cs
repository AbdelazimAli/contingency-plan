using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("RequestWf")]
    public class RequestWf
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20), Required]
        [Index("IX_RequestWf", IsUnique = true, Order = 1)]
        public string Source { get; set; } // Leave

        [Index("IX_RequestWf", IsUnique = true, Order = 2)]
        public int SourceId { get; set; } // ex: leave type Id

        public byte HeirType { get; set; } // Hierarchy type 1-Org Chart  2-Org Chart Hierarchy  3-Position Hierarchy  4-Employee-Manager

        //  // if (Hierarchy type == 3) enter Position Hierarchy
        public int? Hierarchy { get; set; }

        [ForeignKey("Hierarchy")]
        public Diagram PositionHierarchy { get; set; }

        // if (Hierarchy type > 1) enter No. of Approvals
        public byte? NofApprovals { get; set; }

        public byte? TimeOutDays { get; set; } // Waiting Days
        public byte? TimeOutAction { get; set; } // Waiting Action: 1- Back to requester  2- Back to previous step 3- Foreword to next step
        public bool ForceUpload { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    // Roles can be used after/while approving
    public class WfRole
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_WfRole", IsUnique = true, Order = 1)]
        public int WFlowId { get; set; }

        [ForeignKey("WFlowId")]
        public RequestWf WorkFlow { get; set; }

        [Index("IX_WfRole", IsUnique = true, Order = 2)]
        public byte Order { get; set; }
        public Guid? RoleId { get; set; } // Role for all
        public short? CodeId { get; set; } // Hierarchy type == 1 Only

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }

}
