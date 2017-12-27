using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class EmpChkList
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public int? EmpId { get; set; } // Employee
        public int? CompanyId { get; set; } // Employee

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public int? ManagerId { get; set; }
        public Person Manager { get; set; }

        public int? ListId { get; set; }

        [ForeignKey("ListId")]
        public CheckList ParentList { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime ListStartDate { get; set; } = new DateTime(2000, 1, 1); // List start date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ListEndDate { get; set; } // List end date & Duration

        public byte ListType { get; set; } // 1- Employment Checklist  2- New Employee Orientation   3- Termination checklist 4-Tasks list
        public byte Status { get; set; } = 0; // 0-New 1-Done  2-Canceled

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
