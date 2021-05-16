using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Proyecto.Startup))]
namespace Web_Proyecto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
