using Hey.Api.Rest.Response;
using Hey.Api.Rest.Schedules;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class DelayedJobService : IHangfireService
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly IResolveMethod _resolveMethod;

        public DelayedJobService(HeyRememberDto heyRemember, IResolveMethod resolveMethod)
        {
            _heyRemember = heyRemember;
            _resolveMethod = resolveMethod;
        }

        public IHeyResponse CreateNewResponse()
        {
            IMethodBinder methodBinder = _resolveMethod.Find(_heyRemember);
            return new HeyResponseFactory(methodBinder).Make(DelayedScheduleType.MakePrototype());
        }
    }
}