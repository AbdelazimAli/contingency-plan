using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class HRLetterLog
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_HRLetterLog", Order = 1)]
        public int LetterId { get; set; }
        public HRLetter Letter { get; set; }

        [Index("IX_HRLetterLog", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
