using Hey.Core;
using Hey.Core.Services;
using Hey.Soardi.Sms;
using Hey.Soardi.Sms.Exceptions;
using Moq;
using NUnit.Framework;

namespace Hey.Soardi.Tests
{
    [TestFixture]
    class NoteVeicoloSmsSenderTests
    {
        [Test]
        public void TestThat_IfNotEnoughCredit_ShouldSendAnEmail_ToInformAboutThis()
        {
            Mock<ISenderService> senderServiceMock = new Mock<ISenderService>();
            senderServiceMock
                .Setup(service => service.Send(It.IsAny<IMessageProvider>(), It.IsAny<string>()))
                .Throws(new SmsCreditException("credito insufficiente", 0));

            Mock<INotEnoughCredit> notEnoughCreditMock = new Mock<INotEnoughCredit>();

            NotEnoughCreditDto notEnoughCreditDto = new NotEnoughCreditDto(0);
            NoteVeicoloSmsSender smsSender = new NoteVeicoloSmsSender(notEnoughCreditMock.Object);

            Assert.Throws<SmsCreditException>(() => smsSender.Send(0, "3479686512", "Carrozzeria2017", senderServiceMock.Object));
            notEnoughCreditMock.Verify(notEnoughCredit => notEnoughCredit.Handle(It.Is<NotEnoughCreditDto>(dto => dto.Credit == notEnoughCreditDto.Credit)), Times.Once);
        }
    }
}
