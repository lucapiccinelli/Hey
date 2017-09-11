using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Autofac;
using Hangfire;
using Hey.Api.Rest.Exceptions;
using WebApiGlobalConfiguration = System.Web.Http.GlobalConfiguration;
using HangfireGlobalConfiguration = Hangfire.GlobalConfiguration;

namespace Hey.Api.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            WebApiConfig.InitializeLog();
            WebApiGlobalConfiguration.Configure(WebApiConfig.Register);
            WebApiGlobalConfiguration.Configure(WebApiConfig.RegisterDependencies);

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<LogExceptionHandler>().As<IHeyExceptionHandler>();

            HangfireGlobalConfiguration.Configuration
                .UseAutofacActivator(builder.Build())
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
