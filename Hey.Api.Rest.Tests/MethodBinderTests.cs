using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Attributes;
using Hey.Core.Models;
using Moq;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    class MethodBinderTests
    {

        [Test]
        public void TestThatBaseNativeTypesCandBeBindedAndInvoked()
        {
            MethodBinder binder = new MethodBinder();

            binder.Call(new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Type = "",
                Id = "BindMe",
                DomainSpecificData = "[1, \"banana\"]"
            });

            Assert.AreEqual(1, BindingTester.L);
            Assert.AreEqual("banana", BindingTester.S);
        }
    }

    internal class BindingTester
    {
        public static string S = "";
        public static long L = 0;

        [FireMe("BindMe")]
        public void TestMyBinding(long l, string s)
        {
            S = s;
            L = l;
        }
    }
}
