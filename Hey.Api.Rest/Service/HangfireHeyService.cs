using System;
using Hey.Api.Rest.Models;
using Hey.Api.Rest.Service.Concrete;

namespace Hey.Api.Rest.Service
{
    public class HangfireHeyService : IHeyService
    {
        private IServiceResolver _serviceResolver;

        public IHeyResponse Handle(HeyRememberDto heyRemember)
        {
            IConcreteService service = _serviceResolver.Find(heyRemember);
            return service.CreateNewTask();
        }
    }
}