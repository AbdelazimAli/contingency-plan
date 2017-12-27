using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
   public class MeetingViewModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public string MeetSubject { get; set; }
        public string Time { get; set; }
        public DateTime MeetDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public List<int> MeetingAttendee { get; set; }
    }
    public class MeetingAgendaViewModel
    {
        public int Id { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Time { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? EmpId { get; set; } // Owner

    }
    public class MeetingFormViewModel
    {

        public int Id { get; set; }

        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public string MeetSubject { get; set; }
        public string Time { get; set; }
        public DateTime MeetDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled
        public List<int> MeetingAttendee { get; set; }
        public int? LocationId { get; set; }
    }
}
