using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SmtpUser : ISmtpUser
    {
        #region Fields
        private readonly String _username;
        private readonly String _password;
        #endregion

        public SmtpUser(String username, String password)
        {
            _username = username;
            _password = password;
        }

        public ISmtpClient CreateClient(ISmtpService smtpService)
        {
            return smtpService.CreateClient(_username, _password);
        }
    }
}