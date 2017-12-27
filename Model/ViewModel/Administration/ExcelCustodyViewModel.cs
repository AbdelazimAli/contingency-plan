using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelCustodyViewModel
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string ItemCode { get; set; } 
        public string LocationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PurchaseAmount { get; set; }
        public string Description { get; set; }
        public string InUse { get; set; } 
        public string CustodyCatId { get; set; } 
        public string SerialNo { get; set; }
        public string Curr { get; set; }
        public string PurchaseDate { get; set; }
        public string Status { get; set; }
        public string Freeze { get; set; } 


    }
}
