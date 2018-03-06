using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class MeetSchedual
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        [Column(TypeName = "Time")]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "Time")]
        public TimeSpan EndTime { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public int? EmpId { get; set; } 

        [ForeignKey("EmpId")]
        public Person Speaker { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
