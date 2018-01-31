using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Discountapp.MVC.Startup))]
namespace Discountapp.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
