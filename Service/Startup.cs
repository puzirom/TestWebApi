using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TestWebApi.Service.Startup))]
namespace TestWebApi.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}