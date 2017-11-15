using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;
using Hey.Core;
using Hey.Soardi.Sms;
using Hey.Soardi.Sms.Exceptions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Soardi.Tests
{
    [TestFixture()]
    class SmsServiceSenderTests
    {
        private Mock<IMessageProvider> _messageProvideMock;

        [OneTimeSetUp]
        public void SetUp()
        {
            _messageProvideMock = new Mock<IMessageProvider>();
            _messageProvideMock.Setup(provider => provider.GetAbstract()).Returns("ciao");
            _messageProvideMock.Setup(provider => provider.GetText()).Returns("mi piacciono le banane");
        }

        [Ignore("Is sending on the real service, expending real credit!!!")]
        [TestCase("3479686512")]
        [TestCase("393479686512")]
        public void TestSmsSend(string destinationNumber)
        {
            SmsServiceSender smsSender = new SmsServiceSender("", "", "+39035970378");
            Assert.DoesNotThrow(() => smsSender.Send(_messageProvideMock.Object, destinationNumber));
        }
        
        [Test]
        public void TestSmsSend_OnTheRealService_WithWrongPermission_ShouldThrow()
        {
            SmsServiceSender smsSender = new SmsServiceSender("Ciao", "Ciao", "+39035970378");
            Assert.Throws<XmlRpcFaultException>(() => smsSender.Send(_messageProvideMock.Object, "3479686512"));
        }

        [TestCase(0)]
        [TestCase(0.5)]
        public void IfNotEnoughCredit_ShouldThrow_CreditException(double credit)
        {
            Mock<ISmsXmlRpcProxy> proxyMock = new Mock<ISmsXmlRpcProxy>();
            proxyMock.Setup(proxy => proxy.GetCredit(It.IsAny<AuthDto>())).Returns(credit);

            SmsServiceSender smsSender = new SmsServiceSender("user", "password", "+39035970378", proxyMock.Object);
            Assert.Throws<SmsCreditException>(() => smsSender.Send(_messageProvideMock.Object, "3479686512"));
        }
    }
}
