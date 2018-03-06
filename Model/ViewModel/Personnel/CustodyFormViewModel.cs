using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class CustodyFormViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int CustodyCatId { get; set; }
        public string Curr { get; set; }
        public float CurrencyRate { get; set; } = 1;
        public byte Status { get; set; } = 100; // 0 - 100
        public int? BranchId { get; set; }
        [MaxLength(20)]
        public string SerialNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        [MaxLength(30)]
        public string ItemCode { get; set; } // Custody code in inventory
        public float Qty { get; set; }
        public bool Freeze { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int Attachments { get; set; }
        public int? JobId { get; set; }
    }
    public class RecievedCustodyForm
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
        public int CustodyId { get; set; }
        public int EmpId { get; set; }
        public decimal PurchaseAmount { get; set; }
        [MaxLength(20)]
        public string SerialNo { get; set; }
        public int CustodyCatId { get; set; } // 1-Office Tools 2-Car
        public DateTime RecvDate { get; set; } // Recieve Date
        public byte RecvStatus { get; set; } // 0 - 100
        public int? BranchId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        [MaxLength(30)]
        public string ItemCode { get; set; } // Custody code in inventory
        public float Qty { get; set; }      
        public int Attachments { get; set; }

    }
    public class DeleverCustodyFormViewModel
    {

        public int Id { get; set; }
        public int EmpCustodyId { get; set; }
        public string Name { get; set; }
        public int CustodyCatId { get; set; } // 1-Office Tools 2-Car
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int CustodyId { get; set; }
        public float Qty { get; set; } 
        public DateTime? delvryDate { get; set; } // Delivery date
        public byte? delvryStatus { get; set; } // 0 - 100
        public byte RecvStatus { get; set; } // 0 - 100
        public DateTime RecvDate { get; set; } // Recieve Date

        public int Attachments { get; set; }
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
