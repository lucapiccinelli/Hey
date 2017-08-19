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
    [TestFixture()]
    class ResolveServiceByAssemblyTests
    {
        [Test]
        public void TestThatTheResolverCanIstanciateTheConcreteClass()
        {
            ResolveServiceByAssembly resolver = new ResolveServiceByAssembly();
            IConcreteService service = resolver.Find(new HeyRememberDto()
            {
                Domain = "Api.Rest.Tests",
                Type = "",
                Id = "TestService"
            });

            Assert.IsInstanceOf<TestService>(service);
        }
    }

    internal class TestService : IConcreteService
    {
        public TestService(HeyRememberDto heyRemember)
        {
            
        }

        public IHeyResponse CreateNewTask()
        {
            return null;
        }
    }
}
