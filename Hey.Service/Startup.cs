using System;
using System.Configuration;
using System.Web.Http;
using Hangfire;
using Hey.Api.Rest;
using Microsoft.Owin;
using Owin;

namespace Hey.Service
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            int jobExpirationDays = int.Parse(ConfigurationManager.AppSettings["HeyServiceExpirationDays"] ?? "7");

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Initialize(config, TimeSpan.FromDays(jobExpirationDays));
            appBuilder.UseWebApi(config);
            appBuilder.UseHangfireDashboard();
        }
    }
}
