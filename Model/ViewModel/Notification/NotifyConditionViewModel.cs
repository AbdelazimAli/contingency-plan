using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class NotifyConditionViewModel
    {
        [Key]
        public int Id { get; set; }
        public int Event { get; set; }
        [MaxLength(30)]
        public string EventValue { get; set; }

        [MaxLength(30)]
        public string TableName { get; set; }
        public string ObjectName { get; set; }

        public byte Version { get; set; } = 0;
        public int CompanyId { get; set; }

        [MaxLength(30)]
        public string ColumnName { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public int? SourceId { get; set; } = 0;
        public int? CurrentEmp { get; set; } 
        public byte AlertMeFor { get; set; } // 1-All records  2-Only current record  3-Only records method selected filter
        public DateTime? AlertMeUntil { get; set; }

        [MaxLength(500)]
        public string filter { get; set; }

        // Alert me with
        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Message { get; set; }
        [MaxLength(500)]
        public string EncodedMsg { get; set; }

        [MaxLength(100)]
        public string Fields { get; set; }

        //  [MaxLength(250)]
        // public string Users { get; set; }
        public IEnumerable<string> Users { get; set; }

        [MaxLength(50)]
        public string CustEmail { get; set; }
      //  public bool WebOrMob { get; set; } = true; // Send by: Web/Mobile App
        public bool Sms { get; set; } = false; // Sms
    //    public bool Email { get; set; } = false; // Email
        public bool NotifyRef { get; set; } = false; // Send notification to referenced employee

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string divType { get; set; }
        public bool HasEmpId { get; set; }
    }
    public class NotifyConditionIndexViewModel
    {
        [Key]
        public int Id { get; set; }
        public byte Event { get; set; }

        [MaxLength(30)]
        public string EventValue { get; set; }

        public int CompanyId { get; set; }
        [MaxLength(30)]
        public string ColumnName { get; set; }

        public byte AlertMeFor { get; set; } // 1-All records  2-Only current record  3-Only records method selected filter
        public DateTime? AlertMeUntil { get; set; }

        [MaxLength(500)]
        public string filter { get; set; }

        // Alert me with
        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(1000)]
        public string EncodedMsg { get; set; }
        public string Users { get; set; }

        [MaxLength(50)]
        public string CustEmail { get; set; }
       // public bool WebOrMob { get; set; } = true; // Send by: Web/Mobile App
        public bool Sms { get; set; } = false; // Sms
        //public bool Email { get; set; } = false; // Email
        public bool NotifyRef { get; set; } = false; // Send notification to referenced employee
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class NotifyColumnsViewModel
    {
        public string id { get; set; }
        public string name { get; set; }

        public string value { get; set; }
        public string text { get; set; }

        public string objectName { get; set; }
        public string type { get; set; }
        public string pageType { get; set; }
        public string pageTitle { get; set; }
    }
}
