using Microsoft.Owin;
using Owin;

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

         //   app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

        }
    }
}
