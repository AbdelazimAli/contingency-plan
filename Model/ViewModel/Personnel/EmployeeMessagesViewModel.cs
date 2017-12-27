using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class EmployeeMessagesViewModel
    {
        public int Id { get; set; }
        public int FromEmpId { get; set; }
        public int ToEmpId { get; set; }
        public string Title { get; set; }
        public string FromEmployee { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; } = false;
        public string MoreInfo { get; set; }
        public string PicUrl { get; set; }
    }
}
