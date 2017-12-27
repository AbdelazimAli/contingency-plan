using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class CheckList
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1); // Start effect date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; } // End effect date

        public bool IsLocal { get; set; } = false;
        public bool Default { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public byte ListType { get; set; } // 1- Employment Checklist  2- New Employee Orientation   3- Termination checklist
        public short Duration { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class ChecklistTask
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_ChecklistTask", Order = 1, IsUnique = true)]
        public int ListId { get; set; }

        [ForeignKey("ListId")]
        public CheckList Checklist { get; set; }

        [Index("IX_ChecklistTask", Order = 2, IsUnique = true)]
        public short TaskNo { get; set; }
        public short? TaskCat { get; set; } // look up list

        [MaxLength(250)]
        public string Description { get; set; }
        public byte Priority { get; set; }

        public bool Required { get; set; } = false;

        public short? ExpectDur { get; set; }
        public byte? Unit { get; set; } // 1-Minute 2-Hour 3-Day 4-Week  5-Month
        public int? EmpId { get; set; } // Assigned to

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
