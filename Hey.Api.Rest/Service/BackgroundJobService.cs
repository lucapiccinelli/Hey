using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Hangfire;
using Hangfire.Server;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;
using Newtonsoft.Json;

namespace Hey.Api.Rest.Service
{
    public class BackgroundJobService : IHangfireService
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly IResolveMethod _resolveMethod;

        public BackgroundJobService(HeyRememberDto heyRemember, IResolveMethod resolveMethod)
        {
            _heyRemember = heyRemember;
            _resolveMethod = resolveMethod;
        }

        public IHeyResponse CreateNewResponse()
        {
            IMethodBinder methodBinder = _resolveMethod.Find(_heyRemember);
            return new HeyResponseFactory(methodBinder).Make(BackgroundHeyResponse.MakePrototype());
        }
    }
}