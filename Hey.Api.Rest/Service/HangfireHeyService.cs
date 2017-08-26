using System;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class HangfireHeyService : IHeyService
    {
        private readonly IHeyExceptionHandler _exceptionHandler;
        //private IServiceResolver _serviceResolver;

        public HangfireHeyService(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IHeyResponse Handle(HeyRememberDto heyRemember)
        {
            //IHangfireService service = _serviceResolver.Find(heyRemember);
            //return service.CreateNewResponse();
            return new RecurringJobService(heyRemember, new ResolveMethodByFireMeAttribute(_exceptionHandler)).CreateNewResponse();
        }
    }
}