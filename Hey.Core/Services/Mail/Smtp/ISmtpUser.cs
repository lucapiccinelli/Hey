namespace Hey.Core.Services.Mail.Smtp
{
    public interface ISmtpUser
    {
        ISmtpClient CreateClient(ISmtpService smtpService);
    }
}