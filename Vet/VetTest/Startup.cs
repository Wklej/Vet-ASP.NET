using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VetTest.Startup))]
namespace VetTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
