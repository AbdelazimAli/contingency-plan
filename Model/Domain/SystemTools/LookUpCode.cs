using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("LookUpCode", Schema = "dbo")]
    public class LookUpCode
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [Index("IX_Name", 1, IsUnique = true)]
        [Index("IX_LookUpCode", 1, IsUnique = true)]
        public string CodeName { get; set; }

        [Index("IX_LookUpCode", 2, IsUnique = true)]
        public short CodeId { get; set; }

        [MaxLength(150)]
        [Index("IX_Name", 2, IsUnique = true)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }

        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public bool Protected { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
