using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Mailtoken
    {

        [Key]
        [Required, MaxLength(30), Column(Order = 1, TypeName = "varchar")]
        public string ObjectName { get; set; }

        [Key]
        [Required, MaxLength(15), Column(Order = 2, TypeName = "varchar")]
        public string Culture { get; set; }

        [Key]
        [Required, MaxLength(30), Column(Order = 3, TypeName = "varchar")]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Meaning { get; set; }
    }
}
