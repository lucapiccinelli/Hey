﻿using System;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Schedules;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class HangfireHeyService : IHeyService
    {
        private readonly IHeyExceptionHandler _exceptionHandler;

        public HangfireHeyService(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IHeyResponse Handle(HeyRememberDto heyRemember)
        {
            return new FindMethodService(heyRemember, new ResolveMethodByFireMeAttribute(_exceptionHandler))
                .CreateNewResponse(DelayedScheduleType.MakePrototype());
        }
    }
}