namespace Hey.Core.Services
{
    public interface ISenderService
    {
        void Send(IMessageProvider messageProvider, string receiverString);
    }
}
