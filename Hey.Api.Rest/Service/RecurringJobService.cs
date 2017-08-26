using Hey.Api.Rest.Response;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public class RecurringJobService
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly IResolveMethod _resolveMethod;

        public RecurringJobService(HeyRememberDto heyRemember, IResolveMethod resolveMethod)
        {
            _heyRemember = heyRemember;
            _resolveMethod = resolveMethod;
        }

        public IHeyResponse CreateNewResponse()
        {
            IMethodBinder methodBinder = _resolveMethod.Find(_heyRemember);
            return new HeyResponseFactory(methodBinder).Make(RecurringHeyResponse.MakePrototype());
        }
    }
}