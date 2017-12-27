using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.CustomFilters
{
    public class RedirectOnError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {

            if (filterContext.Exception is HttpAntiForgeryException && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                var urlH = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                var rc = new RequestContext(filterContext.HttpContext, filterContext.RouteData);
                string url = RouteTable.Routes.GetVirtualPath(rc, new RouteValueDictionary(new { Controller = "Home", action = "Index" })).VirtualPath;
                if (filterContext.HttpContext.Request.Params["ReturnUrl"] != null && urlH.IsLocalUrl(filterContext.HttpContext.Request.Params["ReturnUrl"]))
                {
                    url = filterContext.HttpContext.Request.Params["ReturnUrl"];
                }
                filterContext.HttpContext.Response.Redirect(url, true);
            }
            else {

                base.OnException(filterContext);
            }
        }
    }
}