using Hey.Core;
using Hey.Core.Services;

namespace Hey.Soardi
{
    class NoteVeicoloSender
    {
        private readonly ISenderService _senderService;
        private readonly IMessageProvider _messageProvider;
        
        public NoteVeicoloSender(ISenderService senderService, IMessageProvider messageProvider)
        {
            _senderService = senderService;
            _messageProvider = messageProvider;
        }

        public void Send(int idNota, string receiverString)
        {
            _senderService.Send(_messageProvider, receiverString);
        }
    }
}
