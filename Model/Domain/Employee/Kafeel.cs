using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("Kafeel")]
    public class Kafeel
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }

        [MaxLength(100), Required]
        public string Name{ get; set; }

        public int? AddressId { get; set; }

        [MaxLength(100)]
        public string Tel { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
