using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service;

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
            BinderCanCallTheMethod binderCanCall = new BinderCanCallTheMethod(_methodBinder);
            if (binderCanCall.Can)
            {
                return new OkHeyResponse(_methodBinder, prototype.Prototype());
            }

            if (binderCanCall.ExecutionResultEnum == MethodExecutionResultEnum.Empty)
            {
                return new MethodNotFoundHeyResponse(_methodBinder.CreateDeferredExecution().HeyRemember);
            }
            if (!binderCanCall.ParametersOk)
            {
                return new ParametersErrorHeyResponse(binderCanCall.ParametersOkNum);
            }
            return new BindingFailedHeyResponse();
        }
    }
}