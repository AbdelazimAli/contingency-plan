using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class DeptJobLeavePlan
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Index("IX_DeptLeavePlan", Order = 1, IsUnique = true)]
        public int DeptId { get; set; }

        [ForeignKey("DeptId")]
        public CompanyStructure Department { get; set; }

        [Index("IX_DeptLeavePlan", Order = 2, IsUnique = true)]
        public int JobId { get; set; }
        public Job Job { get; set; }

        [Index("IX_DeptLeavePlan", Order = 3, IsUnique = true)]
        public DateTime FromDate { get; set; }

        [Index("IX_DeptLeavePlan", Order = 4, IsUnique = true)]
        public DateTime ToDate { get; set; }
        public float MinAllowPercent { get; set; } = 0;
        public byte Stars { get; set; } = 0;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
