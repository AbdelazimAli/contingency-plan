using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;
using System.Diagnostics;
using System.Configuration;
using System.Web.Configuration;
using System.Threading;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            
            // DevExpress Section
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension());
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Required;
            DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Required;
            DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Required;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
           Exception exc = Server.GetLastError();
            Response.Redirect("/Account/ErrorMessage");
        }


        public override void Dispose()
        {
            base.Dispose();
        }
        //protected void Session_End(object sender, EventArgs e)
        //{
        //    if (Session["getUserId"] != null)
        //    {
        //        var db = new ApplicationDbContext();
        //        Configuration conf = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        //        SessionStateSection section = (SessionStateSection)conf.GetSection("system.web/sessionState");
        //        int timeout = (int)section.Timeout.TotalMinutes;
        //        var getid = Session["getUserId"].ToString();

        //        var user = db.UserLogs.Where(b => b.UserId == getid).Select(a => a).OrderByDescending(b => b.Id).FirstOrDefault();
        //        var duration = Convert.ToDateTime(DateTime.Now.AddMinutes(-timeout).ToShortTimeString()).Subtract(Convert.ToDateTime(user.StartTime));
        //        user.EndTime = Convert.ToDateTime(DateTime.Now.AddMinutes(-timeout).ToShortTimeString());
        //        user.Duration = duration;
        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            var Mess = ex.Message;
        //        }
        //    }
        //}

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)



        {
            var culture = new System.Globalization.CultureInfo(User.Identity.GetCulture());
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            if (culture.ToString() =="ar-EG")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
            }
        }

        public void Application_BeingRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            { Response.Flush(); }
        }
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
       
    }
}
