﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Domain
{
    public class CompanyPartner
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(250), Required]
        public string Name { get; set; }
        public int? AddressId { get; set; }

        [MaxLength(15)]
        public string NationalId { get; set; }

        [MaxLength(150)]
        public string JobTitle { get; set; }

        [MaxLength(100)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
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
