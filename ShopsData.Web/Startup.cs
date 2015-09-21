using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopsData.Web.Startup))]
namespace ShopsData.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
