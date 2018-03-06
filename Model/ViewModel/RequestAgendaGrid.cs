using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class RequestAgendaGrid
    {       
            public IEnumerable<MeetingAgendaViewModel> inserted { get; set; }
            public IEnumerable<MeetingAgendaViewModel> updated { get; set; }
            public IEnumerable<MeetingAgendaViewModel> deleted { get; set; }        
    }
}
