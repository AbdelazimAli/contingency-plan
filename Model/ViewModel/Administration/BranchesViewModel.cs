using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class BranchesViewModel
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int BranchNo { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public short? Page { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

}
