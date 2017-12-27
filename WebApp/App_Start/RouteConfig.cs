﻿using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Controllers.Api;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "LogPattern",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
          );

       
        }
    }
}
