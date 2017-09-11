using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SslSmtpClient : ISmtpClient
    {
        #region Const
        private const String SSL = "http://schemas.microsoft.com/cdo/configuration/smtpusessl";
        private const String HOST = "http://schemas.microsoft.com/cdo/configuration/smtpserver";
        private const String PORT = "http://schemas.microsoft.com/cdo/configuration/smtpserverport";
        private const String AUTH = "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate";
        private const String USERNAME = "http://schemas.microsoft.com/cdo/configuration/sendusername";
        private const String PASSWORD = "http://schemas.microsoft.com/cdo/configuration/sendpassword";
        private const String TIMEOUT = "http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout";
        #endregion

        #region Private

        private readonly Int32 _port;
        private readonly String _server;
        private readonly String _username;
        private readonly String _password;
        #endregion

        public SslSmtpClient(String server, Int32 port, String username, String password)
        {
            _port = port;
            _server = server;
            _username = username;
            _password = password;
        }

#pragma warning disable 0618
        public void Send(ISmtpMailMessage message)
        {
            try
            {
                System.Web.Mail.MailMessage mex = message.ToWebMessage();

                configureMessageForChannelSecurity(mex);

                System.Web.Mail.SmtpMail.SmtpServer = _server;
                System.Web.Mail.SmtpMail.Send(mex);
            }
            catch (Exception ex)
            {
                throw new SmtpException(ex);
            }
        }

        private void configureMessageForChannelSecurity(System.Web.Mail.MailMessage message)
        {
            message.Fields.Add(HOST, _server);
            message.Fields.Add(PORT, _port);
            message.Fields.Add(SSL, "true");
            message.Fields.Add(AUTH, "1"); // Use basic clear-text authentication. 
            message.Fields.Add(USERNAME, _username);
            message.Fields.Add(PASSWORD, _password);
            message.Fields.Add(TIMEOUT, 90);
        }
#pragma warning restore 0618
    }
}