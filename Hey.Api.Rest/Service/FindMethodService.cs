using Hey.Api.Rest.Response;
using Hey.Api.Rest.Schedules;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class FindMethodService
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly IResolveMethod _resolveMethod;

        public FindMethodService(HeyRememberDto heyRemember, IResolveMethod resolveMethod)
        {
            _heyRemember = heyRemember;
            _resolveMethod = resolveMethod;
        }

        public IHeyResponse CreateNewResponse(IScheduleType schedulePrototype)
        {
            IMethodBinder methodBinder = _resolveMethod.Find(_heyRemember);
            return new HeyResponseFactory(methodBinder).Make(schedulePrototype, new CreatedHttpReturn());
        }

        public IHeyResponse UpdateResponse(IScheduleType schedulePrototype)
        {
            IMethodBinder methodBinder = _resolveMethod.Find(_heyRemember);
            return new HeyResponseFactory(methodBinder).Make(schedulePrototype, new CreatedHttpReturn());
        }
    }
}