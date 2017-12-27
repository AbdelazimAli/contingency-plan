using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using Model.ViewModel;
using System.Web;

namespace WebApp
{
    public class MyHub : Hub
    {
        public void Announce(List<string> message)
        {
            Clients.All.Announce(message);
        }
        public void SendMessage(List<NavBarItemVM> message)
        {
            Clients.User(Context.User.Identity.Name).SendMessage(message);
        }
        public void AppendMessage(NavBarItemVM Message)
        {
            Clients.User(Context.User.Identity.Name).AppendMessage(Message);
        }


    }
}