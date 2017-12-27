using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
   public class PayrollGradesViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }

        [MaxLength(60), Required]
        public string Name { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        //[MaxLength(100)]
        public string Points { get; set; } // comma seperated point (PayScale lookup code)
        public IEnumerable<int> Point { get; set; }
        public IEnumerable<string> PointName { get; set; }

        [MaxLength(20)]
        public string Group { get; set; }

        [MaxLength(20)]
        public string Grade { get; set; }

        [MaxLength(20)]
        public string SubGrade { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
