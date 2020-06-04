using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuiMinhDuc_Lab456.Startup))]
namespace BuiMinhDuc_Lab456
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
