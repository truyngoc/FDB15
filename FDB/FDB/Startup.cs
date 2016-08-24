using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FDB.Startup))]
namespace FDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
