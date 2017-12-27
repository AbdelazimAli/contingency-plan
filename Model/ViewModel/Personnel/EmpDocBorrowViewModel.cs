using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpDocBorrowViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public int DocId { get; set; }
        public IEnumerable<string> Document { get; set; }
        public DateTime RecvDate { get; set; } // Recieve Date
        public DateTime? delvryDate { get; set; } // Delivery date
        public DateTime? ExpdelvryDate { get; set; } // Expected Delivery date

        [MaxLength(250)]
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
        public string AttUrl { get; set; }
    }
    public class EmpDocBorrowFormViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public int DocId { get; set; }
        public List<int> Document { get; set; }
        public DateTime RecvDate { get; set; } // Recieve Date
        public DateTime? ExpdelvryDate { get; set; } // Expected Delivery date
        public DateTime? delvryDate { get; set; } // Delivery date

        [MaxLength(250)]
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
