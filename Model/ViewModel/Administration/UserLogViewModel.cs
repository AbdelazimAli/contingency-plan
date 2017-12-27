using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class UserLogViewModel
    {
       
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Duration { get; set; }
        public int CompanyId { get; set; }
        
    }
}
