using Hey.Core;
using Hey.Core.Services;
using Hey.Soardi.Mail;

namespace Hey.Soardi.Sms
{
    public class NotEnoughCreditEmail : INotEnoughCredit, IMessageProvider
    {
        private readonly MailInfo _mailInfo;
        private NotEnoughCreditDto _notEnoughCreditDto;
        private readonly string _receiverString;

        public NotEnoughCreditEmail(string receiverString)
        {
            _mailInfo = MailInfo.FromConfig();
            _notEnoughCreditDto = new NotEnoughCreditDto(0);
            _receiverString = receiverString;
        }

        public void Handle(NotEnoughCreditDto notEnoughCreditDto)
        {
            _notEnoughCreditDto = notEnoughCreditDto;
            var emailClient = new SendEmailClient(_mailInfo.Hostname, _mailInfo.Hostport, _mailInfo.Username, _mailInfo.Password, _mailInfo.Security);
            var noteVeicoloSender = new NoteVeicoloSender(emailClient.ComposeMessage(_mailInfo.From), this);
            noteVeicoloSender.Send(_receiverString);
        }

        public string GetText()
        {
            return $"Il credito per l'invio degli sms e' {_notEnoughCreditDto.Credit}";
        }

        public string GetAbstract()
        {
            return "Credito Sms esaurito";
        }
    }
}