using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineAdmission.Startup))]
namespace OnlineAdmission
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
