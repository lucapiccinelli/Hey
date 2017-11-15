using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Attributes;
using Hey.Core.Services;
using Hey.Soardi.Sms.Exceptions;

namespace Hey.Soardi.Sms
{
    public class NoteVeicoloSmsSender
    {
        private readonly INotEnoughCredit _notEnoughCredit;
        private readonly string _username;
        private readonly string _password;
        private readonly string _from;

        public NoteVeicoloSmsSender()
            :this(new NotEnoughCreditEmail(ConfigurationManager.AppSettings["SmsCreditWatcherEmail"]))
        {
            
        }

        public NoteVeicoloSmsSender(INotEnoughCredit notEnoughCredit)
        {
            _notEnoughCredit = notEnoughCredit;
            _username = ConfigurationManager.AppSettings["SmsUser"];
            _password = ConfigurationManager.AppSettings["SmsPassword"];
            _from = ConfigurationManager.AppSettings["DefaultSender"];
        }

        [FireMe("Note")]
        public void Send(long idNota, string receiverPhoneNumber, string connection)
        {
            Send(idNota, receiverPhoneNumber, connection, new SmsServiceSender(_username, _password, _from));
        }

        public void Send(long idNota, string receiverPhoneNumber, string connection, ISenderService senderService)
        {
            try
            {
                var messageProvider = new NoteVeicoloMessageProvider((int)idNota, Connections.Strings[connection]);
                var noteVeicoloSender = new NoteVeicoloSender(senderService, messageProvider);
                noteVeicoloSender.Send(receiverPhoneNumber);
            }
            catch (SmsCreditException e)
            {
                OnNotEnoughCredit(new NotEnoughCreditDto(e.Credit));
                throw;
            }
        }

        private void OnNotEnoughCredit(NotEnoughCreditDto notEnoughCreditDto)
        {
            _notEnoughCredit.Handle(notEnoughCreditDto);
        }
    }
}
