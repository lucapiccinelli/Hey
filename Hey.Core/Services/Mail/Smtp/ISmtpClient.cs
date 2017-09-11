namespace Hey.Core.Services.Mail.Smtp
{
    public interface ISmtpClient
    {
        void Send(ISmtpMailMessage message);
    }
}