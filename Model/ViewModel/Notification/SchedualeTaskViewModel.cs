using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
   public class SchedualeTaskViewModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
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
