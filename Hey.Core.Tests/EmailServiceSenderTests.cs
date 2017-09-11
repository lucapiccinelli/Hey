using System;
using System.Configuration;
using Hey.Core.Services;
using Hey.Core.Services.Mail;
using Moq;
using NUnit.Framework;

namespace Hey.Core.Tests
{
    [TestFixture]
    class EmailServiceSenderTests
    {
        private string _hostname;
        private int _hostport;
        private string _username;
        private string _password;
        private string _from;
        private string _to;
        private SecurityEnum _security;

        [SetUp]
        public void SetUp()
        {
            _hostname = ConfigurationManager.AppSettings["SmtpHostname"];
            _hostport = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _username = ConfigurationManager.AppSettings["SmtpUser"];
            _password = ConfigurationManager.AppSettings["SmtpPassword"];
            _from = ConfigurationManager.AppSettings["DefaultSender"];
            Enum.TryParse(ConfigurationManager.AppSettings["SmtpSecurity"], out _security);

            _to = ConfigurationManager.AppSettings["DefaultReceiver"];
        }

        [Test]
        public void TestSmtpClient()
        {
            SendEmailClient emailClient = new SendEmailClient(_hostname, _hostport, _username, _password, _security);
            ISenderService senderService = emailClient.ComposeMessage(_from);

            Mock<IMessageProvider> messageProvideMock = new Mock<IMessageProvider>();
            messageProvideMock.Setup(provider => provider.GetText()).Returns("Test text");
            messageProvideMock.Setup(provider => provider.GetAbstract()).Returns("Test subject");

            Assert.DoesNotThrow(() => senderService.Send(messageProvideMock.Object, _to));
        }
    }
}
