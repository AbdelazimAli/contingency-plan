using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApp.Models;

namespace WebApp.Extensions
{
    public class CustAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var db = new UserContext();

            bool authorize = false;

            var user = db.Users.Where(p => p.Email.Substring(0, 3) == "mah").Select(p => p.UserName).FirstOrDefault();

            if (user != null && user == httpContext.User.Identity.GetUserName())
            {
                authorize = true;
            }

            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}