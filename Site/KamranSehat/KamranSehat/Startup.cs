using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KamranSehat.Startup))]
namespace KamranSehat

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
