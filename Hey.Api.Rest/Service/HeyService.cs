using System;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Schedules;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class HeyService : IHeyService
    {
        private readonly IHeyExceptionHandler _exceptionHandler;

        public HeyService(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IHeyResponse Create(HeyRememberDto heyRemember)
        {
            return new FindMethodService(heyRemember, new ResolveMethodByFireMeAttribute(_exceptionHandler))
                .CreateNewResponse(DelayedScheduleType.MakePrototype());
        }
    }
}