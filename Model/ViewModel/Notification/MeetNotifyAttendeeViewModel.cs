using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class MeetNotifyAttendeeViewModel
    {
        public int EmpId { get; set; }
        public string Procedure { get; set; } // M :Modified , C:Cancel , S:Send
    }
}
