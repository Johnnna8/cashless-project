using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nmct.ssa.cashlessproject.itcompany.Startup))]
namespace nmct.ssa.cashlessproject.itcompany
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
