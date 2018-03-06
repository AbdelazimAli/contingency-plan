using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class NotifyLetter
    {
        public int Id { get; set; }
        public int CompanyId { get; set; } // Segement by company Id
        public DateTime NotifyDate { get; set; }
        public int EmpId { get; set; }
        public Person Emp { get; set; }

        [Index("IX_NotifyLetter", Order = 1)]
        [MaxLength(128)]
        public string SourceId { get; set; }

        [MaxLength(30)]
        [Index("IX_NotifyLetter", Order = 2)]
        public string NotifySource { get; set; } // ContractRenew  ContractTerminate  MeetingCreate  MeetingModify  MeetingCancel Dicipline
        public bool Sent { get; set; }
        public bool read { get; set; }
        public DateTime? ReadTime { get; set; }
        public DateTime EventDate { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
