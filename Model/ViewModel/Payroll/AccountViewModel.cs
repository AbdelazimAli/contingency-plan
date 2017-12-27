using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class AccountViewModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public bool IsLocal { get; set; } = false; 
        public int? CompanyId { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public byte AccType { get; set; } // 1-GL Account   2-App Account   3-Cost Center
              
        [MaxLength(10)]
        public string Segment1 { get; set; }

        [MaxLength(10)]
        public string Segment2 { get; set; }

        [MaxLength(10)]
        public string Segment3 { get; set; }

        [MaxLength(10)]
        public string Segment4 { get; set; }

        [MaxLength(10)]
        public string Segment5 { get; set; }

        [MaxLength(10)]
        public string Segment6 { get; set; }

        [MaxLength(10)]
        public string Segment7 { get; set; }

        [MaxLength(10)]
        public string Segment8 { get; set; }

        [MaxLength(10)]
        public string Segment9 { get; set; }

        [MaxLength(10)]
        public string Segment10 { get; set; }

       
    }
}
