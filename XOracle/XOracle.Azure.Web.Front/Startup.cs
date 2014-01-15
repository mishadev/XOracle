using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XOracle.Azure.Web.Front.Startup))]

namespace XOracle.Azure.Web.Front
{
    public partial class Startup
    {
        static Startup()
        {
            Startup_Auth();
            Startup_Factories();
            Startup_Enums();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
