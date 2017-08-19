using Hey.Core.Services;

namespace Hey.Soardi
{
    class NoteVeicoloSender
    {
        private readonly ISenderService _senderService;

        public NoteVeicoloSender(ISenderService senderService)
        {
            _senderService = senderService;
        }

        public void Send(int idNota, string receiverString)
        {
            var messageProvider = new NoteVeicoloMessageProvider(idNota);
            _senderService.Send(messageProvider, receiverString);
        }
    }
}
