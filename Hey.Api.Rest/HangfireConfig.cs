﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web;
using Autofac;
using Hangfire;
using Hey.Api.Rest.Exceptions;
using Microsoft.SqlServer.Management.Smo.Wmi;
using HangfireGlobalConfiguration = Hangfire.GlobalConfiguration;

namespace Hey.Api.Rest
{
    public static class HangfireConfig
    {
        public static BackgroundJobServer StartHangfire(string dbName)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<LogExceptionHandler>().As<IHeyExceptionHandler>();
            builder.RegisterType<HeyRememberDeferredExecution>();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 10, OnAttemptsExceeded = AttemptsExceededAction.Delete});

            HangfireGlobalConfiguration.Configuration
                .UseAutofacActivator(builder.Build())
                .UseLog4NetLogProvider()
                .UseSqlServerStorage(dbName);

            return new BackgroundJobServer(new BackgroundJobServerOptions());
        }

        private static void StartSqlServer()
        {
            using (ServiceController service = new ServiceController("MSSQL$SQLEXPRESS"))
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(2000);

                if (service.Status == ServiceControllerStatus.Stopped || service.Status == ServiceControllerStatus.Paused)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
        }
    }
}