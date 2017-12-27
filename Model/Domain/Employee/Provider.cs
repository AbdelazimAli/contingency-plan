using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{ // Providers
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public short ProviderType { get; set; } // lookup user code

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        [MaxLength(100)]
        public string Manager { get; set; }

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
