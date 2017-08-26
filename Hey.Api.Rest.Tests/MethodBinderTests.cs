using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Hey.Api.Rest.Exceptions;
using Hey.Core.Attributes;
using Hey.Core.Models;
using Moq;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class MethodBinderTests
    {
        private MethodInfo _methodInfo;

        [SetUp]
        public void SetUp()
        {
            _methodInfo = typeof(BindingTester).GetMethod(nameof(BindingTester.TestMyBinding));
        }

        [Test]
        public void TestThatBaseNativeTypesCandBeBindedAndInvoked()
        {
            MethodBinder binder = new MethodBinder(_methodInfo, new HeyRememberDto()
            {
                DomainSpecificData = "[1, \"banana\"]"
            });

            MethodExecutionResultEnum result = binder.Invoke();

            Assert.AreEqual(1, BindingTester.L);
            Assert.AreEqual("banana", BindingTester.S);
            Assert.AreEqual(MethodExecutionResultEnum.Ok, result);
        }

        [Test]
        public void TestThatWhenTheMethodCantBeBoundTheExceptionHandlerIsCalled()
        {
            Mock<IHeyExceptionHandler> exceptionHandlerMock = new Mock<IHeyExceptionHandler>();

            MethodBinder binder = new MethodBinder(_methodInfo, new HeyRememberDto()
            {
                DomainSpecificData = "[1, 2]"
            }, exceptionHandlerMock.Object);

            MethodExecutionResultEnum result = binder.Invoke();
            exceptionHandlerMock.Verify(handler => handler.Handle(It.IsAny<Exception>()));
            Assert.AreEqual(MethodExecutionResultEnum.Fail, result);
        }

        [Test]
        public void TestThatWhenTheMethodCantBeBoundTheAndNoExceptionHandlerItMustThrow()
        {
            MethodBinder binder = new MethodBinder(_methodInfo, new HeyRememberDto()
            {
                DomainSpecificData = "[1, 2]"
            });

            Assert.Throws(Is.InstanceOf<Exception>(), () => binder.Invoke());
        }

        [Test]
        public void TestThatWhenGivenAConsumerItPassesTheCorrectMethodAndArguments()
        {
            MethodBinder binder = new MethodBinder(_methodInfo, new HeyRememberDto()
            {
                DomainSpecificData = "[1, \"banana\"]"
            });

            object[] myParams = {1L, "banana"};

            Mock<IBoundMethodConsumer> methodConsumer = new Mock<IBoundMethodConsumer>();
            binder.Invoke(methodConsumer.Object);

            methodConsumer.Verify(consumer => consumer.Use(_methodInfo, It.IsAny<BindingTester>(), It.Is<object[]>(objects => objects.Intersect(myParams).Count() == objects.Length)));
        }



        [Test]
        public void TestThatAMethodWithNoArgumentsCanBeInvoked()
        {
            MethodBinder binder = new MethodBinder(
                typeof(BindingTester).GetMethod(nameof(BindingTester.EmptyBindableMethod)), 
                new HeyRememberDto());
            
            binder.Invoke();
            Assert.AreEqual("Ok", BindingTester.S);
        }
    }

    internal class BindingTester
    {
        public static string S = "";
        public static long L = 0;
        
        public void TestMyBinding(long l, string s)
        {
            S = s;
            L = l;
        }

        public void EmptyBindableMethod()
        {
            S = "Ok";
        }
    }
}
