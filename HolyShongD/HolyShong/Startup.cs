using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HolyShong.Startup))]
namespace HolyShong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
