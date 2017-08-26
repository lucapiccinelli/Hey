using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HeyRememberDeferredExecution
    {
        public MethodExecutionResultEnum Execute(HeyRememberDto heyRemember)
        {
            ResolveMethodByFireMeAttribute resolveMethod = new ResolveMethodByFireMeAttribute();
            IMethodBinder binder = resolveMethod.Find(heyRemember);
            return binder.Invoke();
        }

        public HeyRememberDto HeyRemember { get; set; }
    }
}