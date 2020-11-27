using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hey.Api.Rest;
using log4net;
using log4net.Config;
using Microsoft.Owin.Hosting;

namespace Hey.Service
{
    public partial class HeyService : ServiceBase
    {
        private IDisposable _disposable;
        private BackgroundJobServer _backgroundJobServer;
        private ILog _log;

        public HeyService()
        {
            InitializeComponent();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
            string appdatadir = Environment.GetEnvironmentVariable("LOCALAPPDATA");
#endif
            try
            {
                string serviceHost = ConfigurationManager.AppSettings["HeyServiceHost"] ?? "localhost";
                string servicePort = ConfigurationManager.AppSettings["HeyServicePort"] ?? "60400";
                string serviceProtocol = ConfigurationManager.AppSettings["HeyServiceProtocol"] ?? "http";

                var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                XmlConfigurator.Configure(new FileInfo(Path.Combine(assemblyFolder, "log.config")));
                _log = LogManager.GetLogger(GetType());
                _backgroundJobServer = HangfireConfig.StartHangfire("HeyDb");
                _disposable = WebApp.Start<Startup>(url: $"{serviceProtocol}://{serviceHost}:{servicePort}/");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                Stop();
            }
        }

        protected override void OnStop()
        {
            _backgroundJobServer?.Dispose();
            _disposable.Dispose();
        }
    }
}
