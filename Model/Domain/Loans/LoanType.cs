using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class LoanType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        [Index("IX_LoanType")]
        public string Name { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
