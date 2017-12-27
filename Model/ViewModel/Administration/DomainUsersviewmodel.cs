using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class DomainUsersViewModel
    {
        public string DomainName { get; set; }
        public bool Checked { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
    }
}