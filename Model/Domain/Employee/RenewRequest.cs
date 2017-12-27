using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class RenewRequest
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public Person Emp { get; set; }

        public int CompanyId { get; set; }

        [Required, MaxLength(50), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }

        [MaxLength(250)]
        public string OldValue { get; set; }
        public int? OldValueId { get; set; }

        [MaxLength(250)]
        public string NewValue { get; set; }
        public int? NewValueId { get; set; }
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 

        [MaxLength(250)]
        public string RejectionRes { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string CreatedUser { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
