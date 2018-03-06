using Hangfire;
using Microsoft.Owin;
using Owin;
using WebApp.Filters;

[assembly: OwinStartup(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        // my version
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            GlobalConfiguration.Configuration.UseSqlServerStorage("HrContext");
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new CustomHangFireAuthorizationFilter() },
            });
            app.UseHangfireServer();
        }
    }
}
