using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class EmailAccountViewModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Email { get; set; } // EnterprizeHr@gmail.com

        [MaxLength(100)]
        public string DisplayName { get; set; } // Enterprize Hr     
        [MaxLength(100)]
        public string Username { get; set; } // EnterprizeHr@gmail.com

        public bool EnableSsl { get; set; } = true; // true
        public bool UseDefaultCredentials { get; set; } = false; // false;       

    }
}
