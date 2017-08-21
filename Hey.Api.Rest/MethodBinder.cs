using System;
using System.Reflection;
using Hey.Core.Models;
using Newtonsoft.Json;

namespace Hey.Api.Rest
{
    public class MethodBinder
    {
        private HeyRememberDto _heyRemember;

        public void Call(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
            MethodInfo method = new ResolveMethodByFireMeAttribute().Find(heyRemember);
            object obj = Activator.CreateInstance(method.DeclaringType);
            object[] myParams = JsonConvert.DeserializeObject<object[]>(heyRemember.DomainSpecificData);
            method.Invoke(obj, myParams);
        }
    }
}