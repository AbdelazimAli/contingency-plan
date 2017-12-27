using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("MsgTbl")]
    public class MsgTbl
    {
        [Key]
        [Required, MaxLength(15), Column(Order = 1, TypeName = "varchar")]
        public string Culture { get; set; }

        [Key]
        [Required, MaxLength(30), Column(Order = 2, TypeName = "varchar")]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Meaning { get; set; }

        public bool JavaScript { get; set; } = false;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SequenceId { get; set; }
        public bool Logged { get; set; } = false;
    }
}
