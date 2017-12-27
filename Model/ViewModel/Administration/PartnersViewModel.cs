using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class PartnersViewModel
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }

        [Display(Name = "Name"), MaxLength(250), Required]
        public string Name { get; set; }
        public int? AddressId { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [MaxLength(15), Display(Name = "National Name")]
        public string NationalId { get; set; }

        [MaxLength(150), Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [MaxLength(100), Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [MaxLength(50), Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [MaxLength(50), Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public short? Page { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
