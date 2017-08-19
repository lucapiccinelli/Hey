using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Hangfire;
using WebApiGlobalConfiguration = System.Web.Http.GlobalConfiguration;
using HangfireGlobalConfiguration = Hangfire.GlobalConfiguration;

namespace Hey.Api.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            WebApiGlobalConfiguration.Configure(WebApiConfig.Register);
            WebApiGlobalConfiguration.Configure(WebApiConfig.RegisterDependencies);
            HangfireGlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseSqlServerStorage("HeyDb");

            _backgroundJobServer = new BackgroundJobServer();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }
    }
}
