using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Notification")]
        public int CompanyId { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }

        [MaxLength(128)]
        public string SourceId { get; set; }
        public int? ConditionId { get; set; }
        public NotifyCondition Condition { get; set; }
        public int? EmpId { get; set; }
        public int? RefEmpId { get; set; }
    }

    public class WebMobLog
    {
        [Key]
        public int Id { get; set; }
        public int NotificatId { get; set; }
        public Notification Notificat { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        [Index("IX_WebMobCompany")]
        public int CompanyId { get; set; }

        [MaxLength(20)]
        [Index("IX_WebMobLog")]
        public string SentToUser { get; set; }
        public DateTime SentTime { get; set; }
        public bool MarkAsRead { get; set; } = false;
    }

    public class EmailLog
    {
        [Key]
        public int Id { get; set; }
        public int NotificatId { get; set; }
        public Notification Notificat { get; set; }

        [MaxLength(20)]
        [Index("IX_EmailLog")]
        public string SentToUser { get; set; }
        public bool SentOk { get; set; } = false;
        public DateTime SentTime { get; set; }

        [MaxLength(250)]
        public string Error { get; set; }
    }

    public class SmsLog
    {
        [Key]
        public int Id { get; set; }
        public int NotificatId { get; set; }
        public Notification Notificat { get; set; }

        [MaxLength(20)]
        [Index("IX_SmsLog")]
        public string SentToUser { get; set; }
        public bool SentOk { get; set; } = false;
        public DateTime SentTime { get; set; }

        [MaxLength(250)]
        public string Error { get; set; }
    }
}
