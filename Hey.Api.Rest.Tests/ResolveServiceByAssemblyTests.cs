using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Api.Rest.Service;
using Hey.Api.Rest.Service.Concrete;
using Hey.Core.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class ResolveServiceByAssemblyTests
    {
        [Test]
        public void TestThatTheResolverCanIstanciateTheConcreteClass()
        {
            ResolveServiceByAssembly resolver = new ResolveServiceByAssembly();
            IHangfireService service = resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Id = "TestService"
            });

            Assert.IsInstanceOf<TestService>(service);
        }
    }

    internal class TestService : IHangfireService
    {
        public TestService(HeyRememberDto heyRemember)
        {
            
        }

        public IHeyResponse CreateNewResponse()
        {
            return null;
        }
    }
}
