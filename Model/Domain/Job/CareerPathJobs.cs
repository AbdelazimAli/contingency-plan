using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model.Domain
{
    public class CareerPathJobs
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_CareerPathJobs", IsUnique = true, Order = 1)]
        public int CareerId { get; set; }

        [ForeignKey("CareerId")]
        public CareerPath CareerPath { get; set; }
        public int? Sequence { get; set; }

        [Index("IX_CareerPathJobs", IsUnique = true, Order = 2)]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }

        public byte? MinYears { get; set; }
        public short? Performance { get; set; }

        public int? FormulaId { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
