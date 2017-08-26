using System.Reflection;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class MethodNotFound : IMethodBinder
    {
        private readonly HeyRememberDto _heyRemember;

        public MethodNotFound(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
        }

        public MethodExecutionResultEnum Invoke()
        {
            return MethodExecutionResultEnum.Empty;
        }

        public MethodExecutionResultEnum Invoke(IBoundMethodConsumer consumer)
        {
            return MethodExecutionResultEnum.Empty;
        }

        public string Name => string.Empty;
        public HeyRememberDeferredExecution CreateDeferredExecution()
        {
            return new HeyRememberDeferredExecution()
            {
                HeyRemember = _heyRemember
            };
        }
    }
}