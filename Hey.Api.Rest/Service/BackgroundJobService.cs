using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Hangfire;
using Hangfire.Server;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;
using Newtonsoft.Json;

namespace Hey.Api.Rest.Service
{
    public class BackgroundJobService : IHangfireService
    {
        private readonly HeyRememberDto _heyRemember;

        public BackgroundJobService(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
        }

        public IHeyResponse CreateNewTask()
        {
            MethodBinder binder = new MethodBinder();
            RecurringJob.AddOrUpdate(() => binder.Call(_heyRemember), Cron.Minutely);
            return null;
        }
    }
}