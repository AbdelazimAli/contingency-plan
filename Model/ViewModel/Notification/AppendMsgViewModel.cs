using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model.ViewModel.Notification
{
    public class AppendMsgViewModel
    {
        public List<string> User { get; set; }
        public Domain.Notifications.Notification Notify { get; set; }
        public string PicUrl { get; set; }
        public short Gender { get; set; }
    }
}
