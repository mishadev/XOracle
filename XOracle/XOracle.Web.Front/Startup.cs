using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XOracle.Web.Front.Startup))]

namespace XOracle.Web.Front
{
    public partial class Startup
    {
        static Startup()
        {
            Startup_Auth();
            Startup_Factories();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
