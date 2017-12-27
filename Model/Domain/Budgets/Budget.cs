using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Budget
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Index("IX_Budget", IsUnique = true)]
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int PeriodId { get; set; }
        public Period Period { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
