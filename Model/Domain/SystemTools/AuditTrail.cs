using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class AudiTrail
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_AudiTrail", Order = 1)]
        public int CompanyId { get; set; }

        [Required, MaxLength(50), Index("IX_AudiTrail", Order = 2), Column(TypeName = "varchar")]
        public string ObjectName { get; set; }

        [Index("IX_AudiTrail", Order = 3)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(50), Column(TypeName = "varchar")]
        public string ColumnName { get; set; }

        [MaxLength(128)]
        [Index("IX_AudiTrailSourceId")]
        public string SourceId { get; set; }

        public byte Transtype { get; set; }

        [MaxLength(250)]
        public string ValueBefore { get; set; }

        [MaxLength(250)]
        public string ValueAfter { get; set; }

        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public enum TransType { Insert = 1, Update, Delete };
}
