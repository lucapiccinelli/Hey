using System;
using Hangfire;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class HangfireHeyService : IHeyService
    {
        private IServiceResolver _serviceResolver;

        public IHeyResponse Handle(HeyRememberDto heyRemember)
        {
            IHangfireService service = _serviceResolver.Find(heyRemember);
            return service.CreateNewTask();
        }
    }
}