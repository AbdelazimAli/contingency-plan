using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.ViewModel
{
    public class UsersViewModel
    {
       
        [Key]
        public string  Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public int DefaultCompany { get; set; }
        public string DefaultCompanyName { get; set; }
        public string DefaultLangauge { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool SSUser { get; set; }
        public string Duration { get; set; }//
        public string PhoneNumber { get; set; }
        public bool Locked { get; set; }

    }
}