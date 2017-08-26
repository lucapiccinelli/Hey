using System;
using System.Linq;
using System.Reflection;
using Hey.Api.Rest.Exceptions;
using Hey.Core.Attributes;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class ResolveMethodByFireMeAttribute : IResolveMethod
    {
        private readonly IHeyExceptionHandler _exceptionHandler;

        public ResolveMethodByFireMeAttribute(IHeyExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IMethodBinder Find(HeyRememberDto heyRemember)
        {
            var assembly = Assembly.Load($"{heyRemember.Domain}");
            string myNamespace = $"{heyRemember.Domain}" +
                                 $"{(heyRemember.Type != string.Empty ? "." : "")}" +
                                 $"{heyRemember.Type}";

            MethodInfo fireMeMethod = assembly
                .GetTypes()
                .Where(t => t.Namespace == myNamespace)
                .SelectMany(t => t.GetMethods())
                .FirstOrDefault(m => HasAttribute(m) && m.GetCustomAttribute<FireMeAttribute>().Id == heyRemember.Id);

            return fireMeMethod != null
                ? (IMethodBinder) new MethodBinder(fireMeMethod, heyRemember, _exceptionHandler)
                : new MethodNotFound(heyRemember);
        }

        private bool HasAttribute(MethodInfo m)
        {
            try
            {
                var attribute = m.GetCustomAttribute<FireMeAttribute>();
                return attribute != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}