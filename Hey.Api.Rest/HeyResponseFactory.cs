using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HeyResponseFactory
    {
        private readonly IMethodBinder _methodBinder;

        public HeyResponseFactory(IMethodBinder methodBinder)
        {
            _methodBinder = methodBinder;
        }

        public IHeyResponse Make(IScheduleType prototype)
        {
            HeyRememberDto heyRemember = _methodBinder.CreateDeferredExecution().HeyRemember;
            BinderCanCallTheMethod binderCanCall = new BinderCanCallTheMethod(_methodBinder);
            if (binderCanCall.Can)
            {
                return new OkHeyResponse(_methodBinder, prototype.Prototype());
            }

            if (binderCanCall.ExecutionResultEnum == MethodExecutionResultEnum.Empty)
            {
                return new MethodNotFoundHeyResponse(heyRemember);
            }
            if (!binderCanCall.ParametersOk)
            {
                return new ParametersErrorHeyResponse(heyRemember, binderCanCall.ParametersOkNum);
            }
            return new BindingFailedHeyResponse(heyRemember);
        }
    }
}