using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class EmpCustody
    {
        public int Id { get; set; }

        [Index("IX_EmpCustodyCompany", Order = 1)]
        public int CompanyId { get; set; }

        [Index("IX_EmpCustodyCompany", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public int CustodyId { get; set; }
        public Custody Custody { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime RecvDate { get; set; } // Recieve Date
        public byte RecvStatus { get; set; } // 0 - 100
        public float Qty { get; set; } // 0

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? delvryDate { get; set; } // Delivery date
        public byte? delvryStatus { get; set; } // 0 - 100

        [MaxLength(250)]
        public string Notes { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
