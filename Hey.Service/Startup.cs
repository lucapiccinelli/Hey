using System.Web.Http;
using Hey.Api.Rest;
using Owin;

namespace Hey.Service
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Initialize(config);
            appBuilder.UseWebApi(config);
        }
    }
}
