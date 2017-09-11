using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public interface ISmtpService
    {
        ISmtpClient CreateClient(String username, String password);
    }
}