using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class AssignOrder
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public Person Emp { get; set; }
        public int ManagerId { get; set; }
        public Person Manager { get; set; }
        public byte Duration { get; set; } // 1-Full day  2-Half day  3-Quarter day

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime AssignDate { get; set; } // Assignment date
        public byte CalcMethod { get; set; } // Calculation Method 1-Monetary 2-Time compensation
        public int? LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; } // Expiry date

        // Tasks /////////////////////////
        [MaxLength(500)]
        public string TaskDesc { get; set; }

        // Workflow /////////////////////////////
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 
        public int? WFlowId { get; set; }

        // Time Stamp /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class AssignOrderTasks
    {
        [Key, Column(Order = 1)]
        public int OrderId { get; set; }
        public AssignOrder Order { get; set; }

        [Key, Column(Order = 2)]
        public int TaskId { get; set; }
        public EmpTask Task { get; set; }
    }
}
