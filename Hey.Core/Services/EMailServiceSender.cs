using Hey.Core.Services.Mail.Smtp;

namespace Hey.Core.Services
{
    public class EMailServiceSender : ISenderService
    {
        private readonly ISmtpClient _smtpClient;
        private readonly SmtpMailMessage _message;

        public EMailServiceSender(ISmtpClient smtpClient, SmtpMailMessage message)
        {
            _smtpClient = smtpClient;
            _message = message;
        }

        public void Send(IMessageProvider messageProvider, string receiverString)
        {
            _message.Subject = messageProvider.GetAbstract();
            _message.Body = messageProvider.GetText();
            _message.ToFromString(receiverString);
            _smtpClient.Send(_message);
        }
    }
}