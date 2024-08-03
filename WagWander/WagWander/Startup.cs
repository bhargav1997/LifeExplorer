using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WagWander.Startup))]
namespace WagWander
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
