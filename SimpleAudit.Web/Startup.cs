using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleAudit.Web.Startup))]
namespace SimpleAudit.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
