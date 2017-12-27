using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class RequestWfFormViewModel
    {
       
        public int Id { get; set; }     
        public string Source { get; set; }
        public int SourceId { get; set; } // ex: leave type Id
        public byte HeirType { get; set; } 
        public int? Hierarchy { get; set; }
        public byte? NofApprovals { get; set; }
        public byte? TimeOutDays { get; set; } 
        public byte? TimeOutAction { get; set; } 

    }
}
