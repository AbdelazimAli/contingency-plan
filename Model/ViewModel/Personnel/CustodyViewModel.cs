using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class CustodyViewModel
    {
        [Key]
        public int Id { get; set; }
        public int EmpCustodyId { get; set; }
        public string LName { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public string Filename { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public decimal PurchaseAmount { get; set; }
        public bool Disposal { get; set; } // Disposal Custody
        [MaxLength(250)]
        public string Description { get; set; }
        public float CurrencyRate { get; set; } = 1;

        public bool InUse { get; set; } = false;
        public string CustodyCatId { get; set; } // 1-Office Tools 2-Car

        [MaxLength(20)]
        public string SerialNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? RecvDate { get; set; }

        public string  Employee { get; set; }
        public string Location { get; set; }
        public int EmpId { get; set; }
        public float Qty { get; set; } = 0;
        public float? RestQty { get; set; } = 0;
        //[MaxLength(100)]
        //public string Keyword { get; set; } // comma seperate words
        public byte Status { get; set; } = 100;
        public string AttUrl { get; set; }
        public bool Freeze { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
    public class CustodyCategoryViewModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Disposal { get; set; } = false;
        [MaxLength(15)]
        public string Prefix { get; set; }
        public byte? CodeLength { get; set; }
    }
}
