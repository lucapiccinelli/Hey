using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Hey.Api.Rest.HangfireDashboardStartup))]

namespace Hey.Api.Rest
{
    public class HangfireDashboardStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireDashboard();
        }
    }
}
