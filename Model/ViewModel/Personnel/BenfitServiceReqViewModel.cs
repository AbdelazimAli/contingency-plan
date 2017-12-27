using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class BenfitServiceReqViewModel
    {
     
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public int ProviderId { get; set; }
        public string ServiceName { get; set; }
        public decimal? ServCost { get; set; }
     
        public string Curr { get; set; }          
        public decimal? EmpCost { get; set; }
        public decimal? CompanyCost { get; set; }
      
        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ServStartDate { get; set; }

        public DateTime? ServEndDate { get; set; }
    } 
}
