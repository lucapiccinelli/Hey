using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service.Concrete
{
    public class ResolveServiceByAssembly : IServiceResolver
    {
        public IHangfireService Find(HeyRememberDto heyRemember)
        {
            var assembly = Assembly.Load($"{heyRemember.Domain}");
            string typeName = $"{heyRemember.Domain}" +
                              $"{(heyRemember.Type != string.Empty ? "." : "")}" +
                              $"{heyRemember.Type}" +
                              $"{(heyRemember.Id != string.Empty ? "." : "")}" +
                              $"{heyRemember.Id}";
            Type serviceType = assembly.GetType(typeName);
            ConstructorInfo constructorInfo = serviceType.GetConstructor(new[] { typeof(HeyRememberDto) });
            IHangfireService hangfireService = constructorInfo.Invoke(new[] { heyRemember }) as IHangfireService;

            return hangfireService;
        }
    }
}