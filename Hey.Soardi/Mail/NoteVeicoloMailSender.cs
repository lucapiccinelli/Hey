using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Attributes;
using Hey.Core.Services;

namespace Hey.Soardi.Mail
{
    class NoteVeicoloMailSender 
    {
        private readonly NoteVeicoloSender _noteVeicoloSender;

        public NoteVeicoloMailSender()
        {
            _noteVeicoloSender = new NoteVeicoloSender(new EMailSenderService());
        }

        [FireMe("Note")]
        public void Send(int idNota, string receiverMail)
        {
            _noteVeicoloSender.Send(idNota, receiverMail);
        }
    }
}
