using System;
using System.Reflection;
using System.Web.Http.ExceptionHandling;
using Hangfire.Annotations;
using Hey.Api.Rest.Exceptions;
using Hey.Core.Models;
using Newtonsoft.Json;

namespace Hey.Api.Rest
{
    public class MethodBinder : IMethodBinder
    {
        private readonly MethodInfo _fireMeMethod;
        private readonly HeyRememberDto _heyRemember;
        private readonly IHeyExceptionHandler _exceptionHandler;

        public MethodBinder([NotNull]MethodInfo fireMeMethod, HeyRememberDto heyRemember, IHeyExceptionHandler exceptionHandler = null)
        {
            _fireMeMethod = fireMeMethod;
            _heyRemember = heyRemember;
            _exceptionHandler = exceptionHandler;
        }

        public MethodExecutionResultEnum Invoke()
        {
            return Invoke(new HeyInvokeMethod());
        }

        public MethodExecutionResultEnum Invoke(IBoundMethodConsumer consumer)
        {
            try
            {
                object obj = Activator.CreateInstance(_fireMeMethod.DeclaringType);
                object[] myParams = JsonConvert.DeserializeObject<object[]>(_heyRemember.DomainSpecificData);
                return consumer.Use(_fireMeMethod, obj, myParams);
            }
            catch (Exception e)
            {
                if (_exceptionHandler != null)
                {
                    _exceptionHandler.Handle(e);
                    return MethodExecutionResultEnum.Fail;
                }
                throw;
            }
        }

        public string Name => _fireMeMethod.Name;
        public HeyRememberDeferredExecution CreateDeferredExecution() => new HeyRememberDeferredExecution()
        {
            HeyRemember = _heyRemember
        };
    }
}