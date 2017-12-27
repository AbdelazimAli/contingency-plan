using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class EmailAccount
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Email { get; set; } // EnterprizeHr@gmail.com

        [MaxLength(100)]
        public string DisplayName { get; set; } // Enterprize Hr

        [MaxLength(150)]
        public string Host { get; set; } // smtp.gmail.com
        public int Port { get; set; } // 587

        [MaxLength(100)]
        public string Username { get; set; } // EnterprizeHr@gmail.com

        [MaxLength(100)]
        public string Password { get; set; }

        public bool EnableSsl { get; set; } = false; // true
        public bool UseDefaultCredentials { get; set; } = false; // false;
        public int SendOrder { get; set; }
        public int Capacity { get; set; } = 500; // Max emails sent per day
        public DateTime? LastSentDate { get; set; } // Last sent date
        public int TodayCount { get; set; } = 0; // Today emails sent count

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        // Send test email
    }
}
