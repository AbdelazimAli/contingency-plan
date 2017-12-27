using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class SchedualTask
    {
        [Key]
        public int EventId { get; set; }

        [MaxLength(30)]
        public string EventName { get; set; }

        [MaxLength(50)]
        public string EventUrl { get; set; }
        public bool Enabled { get; set; } = true;
        public bool StopOnError { get; set; } = false;
        public DateTime? LastStartDate { get; set; }
        public DateTime? LastEndDate { get; set; }
        public DateTime? LastSuccessDate { get; set; }
        public int PeriodInMinutes { get; set; }
        public int CompanyId { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
