using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PRINGUP_Proj.Startup))]
namespace PRINGUP_Proj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
