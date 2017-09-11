using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Services.Mail;
using Hey.Core.Services.Mail.Smtp;

namespace Hey.Core.Services
{
    public class SendEmailClient
    {
        private readonly ISmtpClient _smtpClient;

        public SendEmailClient(string hostname, int hostport, string username, string password, SecurityEnum security)
        {
            var user = new SmtpUser(username, password);

            // Creo il service corretto
            ISmtpService smtpService = GetCorrectSmtpService(hostname, hostport, security);

            // Creo il client SMTP
            _smtpClient = user.CreateClient(smtpService);
        }

        public ISenderService ComposeMessage(string from, string cc = "", string bcc = "", string readReceipt = "", bool htmlBody = false)
        {
            var message = new SmtpMailMessage(from, cc, bcc, readReceipt, null, htmlBody);
            return new EMailServiceSender(_smtpClient, message);
        }

        private static ISmtpService GetCorrectSmtpService(String hostname, Int32 hostport, SecurityEnum security)
        {
            switch (security)
            {
                case SecurityEnum.None:
                    return new SmtpService(hostname, hostport);

                case SecurityEnum.Ssl:
                    return new SslSmtpService(hostname, hostport);

                case SecurityEnum.StartTls:
                    return new StartTlsSmtpService(hostname, hostport);

                default:
                    throw new SmtpException($"Il livello di sicurezza {security} non e' gestito");
            }
        }
    }
}
