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
            MethodInfo method = resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Id = "Test"
            });

            Assert.NotNull(method);
            Assert.AreEqual(method.Name, nameof(ResolveMethodByFireMeAttributeTestClass.RetrieveMe));
        }

        [Test]
        public void TestThatTheResolverDoesNotThrowWhenHeCantFindTheMethod()
        {
            ResolveMethodByFireMeAttribute resolver = new ResolveMethodByFireMeAttribute();
            Assert.DoesNotThrow(() => resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Id = "NotValid"
            }));
        }

        [Test]
        public void TestThatWhenTheResolverCantFindTheFireMeMethodTheResultIsNull()
        {
            ResolveMethodByFireMeAttribute resolver = new ResolveMethodByFireMeAttribute();
            MethodInfo method = resolver.Find(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Id = "NotValid"
            });

            Assert.IsNull(method);
        }
    }

    internal class ResolveMethodByFireMeAttributeTestClass
    {
        [FireMe("Test")]
        public void RetrieveMe()
        {
            
        }
    }
}
