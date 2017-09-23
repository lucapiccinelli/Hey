using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service;
using log4net;
using log4net.Config;
using WebApiContrib.Formatting.Jsonp;

namespace Hey.Api.Rest
{
    public static class WebApiConfig
    {
        public static void Initialize(HttpConfiguration config)
        {
            InitializeLog();
            Register(config);
            RegisterDependencies(config);
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{ext}",
                defaults: new { id = RouteParameter.Optional, ext = "json" }
            );

            config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");

            var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            config.Formatters.Insert(0, jsonpFormatter);

            jsonpFormatter.AddUriPathExtensionMapping("jsonp", "text/javascript");

            //config.Formatters.JsonFormatter.MediaTypeMappings
            //    .Add(new RequestHeaderMapping("Accept",
            //        "text/html",
            //        StringComparison.InvariantCultureIgnoreCase,
            //        true,
            //        "application/json"));
        }

        public static void RegisterDependencies(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<LogExceptionHandler>().As<IHeyExceptionHandler>().InstancePerRequest();
            builder.RegisterType<HangfireJobRepository>().As<IJobRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HeyService>().As<IHeyService>().InstancePerLifetimeScope();

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static void InitializeLog()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            ILog log = LogManager.GetLogger(typeof(WebApiConfig));
            log.Info("~".PadLeft(100, '~'));
            log.Info($"hey version: {version}");
        }
    }
}
