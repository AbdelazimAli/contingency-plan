using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Meeting
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(500)]
        public string SubjectDesc { get; set; }
        public short MeetSubject { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime MeetDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [MaxLength(250)]
        public string LocationText { get; set; }
        public int EmpId { get; set; } // Owner

        [ForeignKey("EmpId")]
        public Person Owner { get; set; }
        public byte Status { get; set; } // 1-Created  2-Modified  3-Canceled
        public bool IsUploaded { get; set; }
        public byte LocationType { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class MeetAttendee
    {
        [Key, Column(Order = 1)]
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        [Key, Column(Order = 2)]
        public int EmpId { get; set; } // Attendee

        [ForeignKey("EmpId")]
        public Person Attendee { get; set; }
    }
}
