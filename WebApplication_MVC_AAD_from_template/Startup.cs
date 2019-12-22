using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace WebApplication_MVC_AAD_from_template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
