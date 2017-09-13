using System;
using Hangfire;
using WebApiGlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace Hey.Api.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            WebApiGlobalConfiguration.Configure(WebApiConfig.Initialize);
            _backgroundJobServer = HangfireConfig.StartHangfire("HeyDb");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }
    }
}
