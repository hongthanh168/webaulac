using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAuLac.Startup))]
namespace WebAuLac
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
