using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmploymentPaper_ToNotify
    {
        public Guid Stream_Id { set; get; }
        public string PaperFileName { set; get; }
        public string DocTypeName { set; get; }
        public DateTime NotifyDate { set; get; }
        public int EmpID { set; get; }
        public int? CompanyId { set; get; }
        public DateTime? ExpiryDate { set; get; }
        public bool AlreadyNotified { set; get; }
    }
}
