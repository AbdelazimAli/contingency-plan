using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class DisplinRange
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_DisplinRange", IsUnique = true, Order = 1)]
        public int DisPeriodId { get; set; }

        [ForeignKey("DisPeriodId")]
        public DisplinPeriod DisplinPeriod { get; set; }

        [Index("IX_DisplinRange", IsUnique = true, Order = 2)]
        public int FromPoint { get; set; }
        public int ToPoint { get; set; }
        public float Percentage { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
