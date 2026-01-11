using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mitchell.ScmConsoles.Startup))]
namespace Mitchell.ScmConsoles
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
