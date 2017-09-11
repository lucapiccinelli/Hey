using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SslSmtpService : ISmtpService
    {
        #region Private
        private readonly Int32 _port;
        private readonly String _server;
        #endregion

        public SslSmtpService(String server, Int32 port)
        {
            _port = port;
            _server = server;
        }

        public ISmtpClient CreateClient(String username, String password)
        {
            return new SslSmtpClient(_server, _port, username, password);
        }
    }
}