using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Hey.Api.Rest.Models;

namespace Hey.Api.Rest.Service.Concrete
{
    public class ResolveServiceByAssembly : IServiceResolver
    {
        public IConcreteService Find(HeyRememberDto heyRemember)
        {
            var assembly = Assembly.Load($"Hey.{heyRemember.Domain}");
            string typeName = $"Hey.{heyRemember.Domain}" +
                              $"{(heyRemember.Type != string.Empty ? "." : "")}" +
                              $"{heyRemember.Type}" +
                              $"{(heyRemember.Id != string.Empty ? "." : "")}" +
                              $"{heyRemember.Id}";
            Type serviceType = assembly.GetType(typeName);
            ConstructorInfo constructorInfo = serviceType.GetConstructor(new[] { typeof(HeyRememberDto) });
            IConcreteService concreteService = constructorInfo.Invoke(new[] { heyRemember }) as IConcreteService;

            return concreteService;
        }
    }
}