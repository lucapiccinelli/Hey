using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Attributes;
using Hey.Core.Services;
using Hey.Core.Services.Mail;

namespace Hey.Soardi.Mail
{
    class NoteVeicoloMailSender 
    {
        private readonly MailInfo _mailInfo;

        public NoteVeicoloMailSender()
        {
            _mailInfo = MailInfo.FromConfig();
        }

        [FireMe("Note")]
        public void Send(long idNota, string receiverMail, string connection)
        {
            var emailClient = new SendEmailClient(_mailInfo.Hostname, _mailInfo.Hostport, _mailInfo.Username, _mailInfo.Password, _mailInfo.Security);
            var messageProvider = new NoteVeicoloMessageProvider((int)idNota, Connections.Strings[connection]);
            var noteVeicoloSender = new NoteVeicoloSender(emailClient.ComposeMessage(_mailInfo.From, receiverMail), messageProvider);
            noteVeicoloSender.Send(receiverMail);
        }
    }
}
