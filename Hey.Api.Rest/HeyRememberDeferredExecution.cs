using System.Web.Http.ExceptionHandling;
using Hey.Api.Rest.Exceptions;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HeyRememberDeferredExecution
    {
        private readonly IHeyExceptionHandler _exceptionHandler;

        public HeyRememberDeferredExecution(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
        }

        public MethodExecutionResultEnum Execute(HeyRememberDto heyRemember)
        {
            ResolveMethodByFireMeAttribute resolveMethod = new ResolveMethodByFireMeAttribute(_exceptionHandler);
            IMethodBinder binder = resolveMethod.Find(heyRemember);
            return binder.Invoke();
        }

        public HeyRememberDto HeyRemember { get; set; }
    }
}