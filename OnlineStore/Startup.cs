using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineStore.Startup))]
namespace OnlineStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
