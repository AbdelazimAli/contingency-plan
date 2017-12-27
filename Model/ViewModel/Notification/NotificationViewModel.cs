using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool ReadOK { get; set; } = false;
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }
        public int? ConditionId { get; set; }
        public string SentToUser { get; set; }
    }
}
