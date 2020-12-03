using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NexxtDent.Startup))]
namespace NexxtDent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
