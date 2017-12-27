using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Domain
{
    public class CompanyBranch
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Required]
        public int BranchNo { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        [MaxLength(100)]
        public string Telephone { get; set; }

        [MaxLength(50)] // 100
        public string Email { get; set; }

        // /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
