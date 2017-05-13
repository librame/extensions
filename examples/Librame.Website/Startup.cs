using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Librame.Website.Startup))]
namespace Librame.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
