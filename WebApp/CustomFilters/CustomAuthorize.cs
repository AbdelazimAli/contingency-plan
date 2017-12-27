using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.CustomFilters
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            UserContext db = new UserContext();
            bool authorize = false;

            var correctuser = db.Users.Where(a => a.Email.Substring(0,3) == "Seh").Select(p => p.UserName).FirstOrDefault();

            if (correctuser != null && correctuser == httpContext.User.Identity.Name)
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

    public class MyCustomAuthorize : System.Web.Http.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }

}