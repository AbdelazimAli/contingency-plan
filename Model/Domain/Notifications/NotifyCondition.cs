using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class NotifyCondition
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_NotifyCondition2", Order = 2)]
        public Events Event { get; set; }

        [MaxLength(30)]
        public string EventValue { get; set; }

        [MaxLength(30)]
        public string TableName { get; set; }

        [Index("IX_NotifyCondition", Order = 1)]
        [Index("IX_NotifyCondition2", Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(30), Index("IX_NotifyCondition", Order = 2), Column(TypeName = "varchar")]
        public string ObjectName { get; set; }

        [Index("IX_NotifyCondition", Order = 3)]
        public byte Version { get; set; } = 0;

        [MaxLength(30)]
        public string ColumnName { get; set; }

        public byte AlertMeFor { get; set; } // 1-All records  2-Only current record  3-Only records method selected filter

        [Index("IX_NotifyCondition", Order = 4)]
        [Index("IX_NotifyCondition2", Order = 3)]
        public DateTime? AlertMeUntil { get; set; }

        [MaxLength(500)]
        public string filter { get; set; }

        // Alert me with
        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        [MaxLength(500)]
        public string EncodedMsg { get; set; }

        [MaxLength(100)]
        public string Fields { get; set; }

        [MaxLength(250)]
        public string Users { get; set; }

        [MaxLength(50)]
        public string CustEmail { get; set; }
    //    public bool WebOrMob { get; set; } = true; // Send by: Web/Mobile App
        public bool Sms { get; set; } = false; // Send message by Sms
      //  public bool Email { get; set; } = false; // Email
        public bool NotifyRef { get; set; } = false; // Send notification to referenced employee
        
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
