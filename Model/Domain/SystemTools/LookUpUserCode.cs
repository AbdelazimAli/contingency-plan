using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LookUpUserCode
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Index("IX_LookUpUserCode", 1, IsUnique = true)]
        [Index("IX_UserCodeName", 1, IsUnique = true)]
        public string CodeName { get; set; }

        [Index("IX_LookUpUserCode", 2, IsUnique = true)]
        public short CodeId { get; set; }

        public byte SysCodeId { get; set; }
        public SystemCode SysCode { get; set; }

        [MaxLength(150)]
        [Index("IX_UserCodeName", 2, IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
