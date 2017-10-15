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
        private readonly string _hostname;
        private readonly int _hostport;
        private readonly string _username;
        private readonly string _password;
        private readonly SecurityEnum _security;
        private readonly string _from;

        public NoteVeicoloMailSender()
        {
            _hostname = ConfigurationManager.AppSettings["SmtpHostname"];
            _hostport = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _username = ConfigurationManager.AppSettings["SmtpUser"];
            _password = ConfigurationManager.AppSettings["SmtpPassword"];
            _from = ConfigurationManager.AppSettings["DefaultSender"];
            Enum.TryParse(ConfigurationManager.AppSettings["SmtpSecurity"], out _security);
        }

        [FireMe("Note")]
        public void Send(long idNota, string receiverMail, string connection)
        {
            var emailClient = new SendEmailClient(_hostname, _hostport, _username, _password, _security);
            var messageProvider = new NoteVeicoloMessageProvider((int)idNota, Connections.Strings[connection]);
            var noteVeicoloSender = new NoteVeicoloSender(emailClient.ComposeMessage(_from, receiverMail), messageProvider);
            noteVeicoloSender.Send((int) idNota, receiverMail);
        }
    }
}
