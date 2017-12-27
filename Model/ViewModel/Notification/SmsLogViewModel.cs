using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
   public class SmsLogViewModel
    {
        public int Id { get; set; }
        public int NotificatId { get; set; }
        [MaxLength(20)]
        public string SentToUser { get; set; }
        public bool SentOk { get; set; } = false;
        public DateTime SentTime { get; set; }

        [MaxLength(250)]
        public string Error { get; set; }
        public bool ReadOK { get; set; } = false;

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
