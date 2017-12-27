using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class EmpDocBorrow
    {
        public int Id { get; set; }

        [Index("IX_EmpDocBorrow", Order = 1)]
        public int CompanyId { get; set; }

        [Index("IX_EmpDocBorrow", Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime RecvDate { get; set; } // Recieve Date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? delvryDate { get; set; } // Delivery date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpdelvryDate { get; set; } // Expected Delivery date

        [MaxLength(200)]
        public string Purpose { get; set; }

        [MaxLength(200)]
        public string Site { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
