using Model.Domain;
using Model.Domain.Notifications;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using WebApp.Controllers.NewApi;

namespace WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "newApi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<WebMobLog>("WebMobLogs");
            builder.EntitySet<Notification>("Notification");
            builder.EntitySet<NotificationVM>("NotificationVM");
            builder.EntitySet<NavBarItemVM>("NavBarItemVM");

            builder.EntitySet<NotifyLetter>("NotifyLetters");
            builder.EntitySet<Person>("People");
            builder.EntitySet<Assignment>("Assignment");
            builder.EntitySet<NotifiyLetterViewModel>("NotifiyLetterViewModel");
            //builder.EntitySet<LetterVM>("LetterVM");
            
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
