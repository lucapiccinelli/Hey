using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class HeyResponseFactoryTests
    {
        [Test]
        public void TestThatIfTheMethodIsCallableICreatesTheOkResponse()
        {
            MethodInfo method = typeof(FactoryBindingTest).GetMethod(nameof(FactoryBindingTest.EmptyBindableMethod));
            MethodBinder binder = new MethodBinder(method, new HeyRememberDto());

            HeyResponseFactory factory = new HeyResponseFactory(binder);
            IHeyResponse response = factory.Make(BackgroundHeyResponse.MakePrototype());

            Assert.IsInstanceOf<BackgroundHeyResponse>(response);
        }

        [Test]
        public void TestThatIfTheMethodIsNotCallableICreatesTheNotOkResponse()
        {
            HeyResponseFactory factory = new HeyResponseFactory(new MethodNotFound(new HeyRememberDto()));
            IHeyResponse response = factory.Make(BackgroundHeyResponse.MakePrototype());

            Assert.IsInstanceOf<MethodNotFoundHeyResponse>(response);
        }

        [Test]
        public void TestThatIfTheMethodHasErrorOnParameterItReturnsNotOkResponse()
        {
            MethodInfo method = typeof(FactoryBindingTest).GetMethod(nameof(FactoryBindingTest.WithParametersBindableMethod));
            MethodBinder binder = new MethodBinder(method, new HeyRememberDto());

            HeyResponseFactory factory = new HeyResponseFactory(binder);
            IHeyResponse response = factory.Make(BackgroundHeyResponse.MakePrototype());

            Assert.IsInstanceOf<ParametersErrorHeyResponse>(response);
        }

        [Test]
        public void TestThatIfTheMethodHasErrorOnParameterTypeItReturnsNotOkResponseWithTheNumberOfTheParameterThatGotError()
        {
            MethodInfo method = typeof(FactoryBindingTest).GetMethod(nameof(FactoryBindingTest.WithParametersBindableMethod));
            MethodBinder binder = new MethodBinder(method, new HeyRememberDto()
            {
                DomainSpecificData = "[1, \"banana\"]"
            });

            HeyResponseFactory factory = new HeyResponseFactory(binder);
            IHeyResponse response = factory.Make(BackgroundHeyResponse.MakePrototype());

            Assert.IsInstanceOf<ParametersErrorHeyResponse>(response);
            Assert.AreEqual(1, ((ParametersErrorHeyResponse) response).ParamNum);
        }
    }

    internal class FactoryBindingTest
    {
        public void EmptyBindableMethod()
        {
            
        }

        public void WithParametersBindableMethod(long x, float y)
        {

        }
    }
}
