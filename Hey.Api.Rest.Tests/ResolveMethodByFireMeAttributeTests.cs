using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Attributes;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class ResolveMethodByFireMeAttributeTests
    {
        [Test]
        public void TestThatTheResolverCanFindTheFireMeMethod()
        {
            ResolveMethodByFireMeAttribute resolver = new ResolveMethodByFireMeAttribute();
            IMethodBinder method = resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Name = "BindMe"
            });
            
            Assert.AreEqual(method.Name, nameof(ResolveMethodByFireMeAttributeTestClass.RetrieveMe));
        }

        [Test]
        public void TestThatTheResolverReturnCorrectlyWhenCantFindTheMethod()
        {
            ResolveMethodByFireMeAttribute resolver = new ResolveMethodByFireMeAttribute();
            IMethodBinder method = resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Name = "ThisMethodDoesntExists"
            });

            Assert.AreEqual(MethodExecutionResultEnum.Empty, method.Invoke());
        }
    }

    internal class ResolveMethodByFireMeAttributeTestClass
    {
        [FireMe("BindMe")]
        public void RetrieveMe()
        {
            
        }
    }
}
