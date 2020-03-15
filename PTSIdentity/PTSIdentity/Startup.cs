using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PTSIdentity.Startup))]
namespace PTSIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
