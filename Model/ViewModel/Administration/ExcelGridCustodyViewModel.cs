using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelGridCustodyViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string Curr { get; set; }
        public int CustodyCatId { get; set; } // 1-Office Tools 2-Car
        public int Sequence { get; set; }
        public int? JobId { get; set; }
        public string SerialNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool InUse { get; set; } = false;
        public byte Status { get; set; } = 100; 
        public int? LocationId { get; set; }
        public bool Freeze { get; set; } = false;

    }
}
