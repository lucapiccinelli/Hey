using Hey.Core;

namespace Hey.Soardi.Sms
{
    public class AvvisoPrenotazioneMessageProvider : IMessageProvider
    {
        public string GetText()
        {
            return "Info Soardi, Le ricordiamo l'appuntamento di domani mattina per la riparazione della Sua Vettura.";
        }

        public string GetAbstract()
        {
            return "Promemoria riparazione auto.";
        }
    }
}