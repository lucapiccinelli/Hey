using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SmtpService : ISmtpService
    {
        #region Private
        private readonly Int32 _port;
        private readonly String _server;
        #endregion

        public SmtpService(String server, Int32 port)
        {
            _port = port;
            _server = server;
        }

        public ISmtpClient CreateClient(String username, String password)
        {
            return new SmtpClient(_server, _port, username, password);
        }
    }
}