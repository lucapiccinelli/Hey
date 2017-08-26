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

        public IHeyResponse Make(IHeyResponse prototype)
        {
            BinderCanCallTheMethod binderCanCall = new BinderCanCallTheMethod(_methodBinder);
            if (binderCanCall.Can)
            {
                return prototype.Prototype(_methodBinder);
            }

            if (binderCanCall.ExecutionResultEnum == MethodExecutionResultEnum.Empty)
            {
                return new MethodNotFoundHeyResponse();
            }
            if (!binderCanCall.ParametersOk)
            {
                return new ParametersErrorHeyResponse(binderCanCall.ParametersOkNum);
            }
            return new BindingFailedHeyResponse();
        }
    }
}