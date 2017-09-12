using System.Web.Http.ExceptionHandling;
using Hey.Api.Rest.Exceptions;
using Hey.Core.Models;
using log4net;

namespace Hey.Api.Rest
{
    public class HeyRememberDeferredExecution
    {
        private readonly IHeyExceptionHandler _exceptionHandler;
        private readonly ILog _log;

        public HeyRememberDeferredExecution(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
            _log = LogManager.GetLogger(GetType());
        }

        public MethodExecutionResultEnum Execute(HeyRememberDto heyRemember)
        {
            ResolveMethodByFireMeAttribute resolveMethod = new ResolveMethodByFireMeAttribute(_exceptionHandler);
            IMethodBinder binder = resolveMethod.Find(heyRemember);
            MethodExecutionResultEnum result = binder.Invoke();
            _log.Info($"execution of {heyRemember}: {result}");
            if (result != MethodExecutionResultEnum.Ok)
            {
                throw new DeferredExecutionException($"The execution of {heyRemember} failed: see logs for details");
            }

            return result;
        }

        public HeyRememberDto HeyRemember { get; set; }
    }
}