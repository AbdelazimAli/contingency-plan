using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class NotificationMenuViewModel
    {   
        [Key]
        public int Id { get; set; }
        public byte TransType { get; set; } // 1-Insert 2-Update 3-Delete

        [MaxLength(30)]
        public string TableName { get; set; }

        public byte Version { get; set; } = 0;

        [MaxLength(30)]
        public string ColumnName { get; set; }

        [MaxLength(500)]
        public string Condition { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string Body { get; set; }
        public bool WebOrMob { get; set; } = true; // Send by: Web/Mobile App
        public bool Sms { get; set; } = false; // Sms
        public bool Email { get; set; } = false; // Email
        public bool NotifyRef { get; set; } = false; // Send notification to referenced employee
        public IEnumerable<int> Users { get; set; }
    }

}
