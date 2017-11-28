using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core;
using Hey.Core.Attributes;
using Hey.Core.Services;
using Hey.Soardi.Model;
using Hey.Soardi.Sms.Exceptions;
using log4net;
using ILogNet = log4net.ILog;

namespace Hey.Soardi.Sms
{
    public class SoardiSmsSender
    {
        private readonly INotEnoughCredit _notEnoughCredit;
        private readonly string _username;
        private readonly string _password;
        private readonly string _from;
        private ILogNet _log;
        

        public SoardiSmsSender()
            :this(new NotEnoughCreditEmail(ConfigurationManager.AppSettings["SmsCreditWatcherEmail"]))
        {
            
        }

        public SoardiSmsSender(INotEnoughCredit notEnoughCredit)
        {
            _notEnoughCredit = notEnoughCredit;
            _username = ConfigurationManager.AppSettings["SmsUser"];
            _password = ConfigurationManager.AppSettings["SmsPassword"];
            _from = ConfigurationManager.AppSettings["DefaultSmsSender"];

            _log = LogManager.GetLogger(GetType());
        }

        [FireMe("Note")]
        public void Send(long idNota, string receiverPhoneNumber, string connection)
        {
            Send(idNota, receiverPhoneNumber, connection, new SmsServiceSender(_username, _password, _from));
        }

        public void Send(long idNota, string receiverPhoneNumber, string connection, ISenderService senderService)
        {
            SendSms(
                receiverPhoneNumber, 
                senderService, 
                new NoteVeicoloMessageProvider((int)idNota, Connections.Strings[connection]));
        }

        [FireMe("Prenotazione")]
        public void SendPrenotazione(long numPreventivo, string receiverPhoneNumber, string connection)
        {
            using (CarrozzeriaDataContext dt = new CarrozzeriaDataContext(Connections.Strings[connection]))
            {
                int veicoliCount = dt.Veicolis.Count(veicolo => veicolo.numPreventivo == numPreventivo);
                _log.Info($"IdVeicolo: {numPreventivo}, count: {veicoliCount}, sede: {connection}");
                if (veicoliCount == 0)
                {
                    _log.Info($"Veicolo non trovato, nessun sms inviato");
                    return;
                }
            }

            SendSms(
                receiverPhoneNumber, 
                new SmsServiceSender(_username, _password, _from),
                new AvvisoPrenotazioneMessageProvider());
        }

        private void SendSms(string receiverPhoneNumber, ISenderService senderService, IMessageProvider messageProvider)
        {
            try
            {
                var noteVeicoloSender = new NoteVeicoloSender(senderService, messageProvider);
                noteVeicoloSender.Send(receiverPhoneNumber);
                _log.Info($"Sms inviato a {receiverPhoneNumber}");
            }
            catch (SmsCreditException e)
            {
                _log.Info($"Rilevato credito insufficiente inviando sms a {receiverPhoneNumber}");
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
