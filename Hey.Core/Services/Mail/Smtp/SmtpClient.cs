using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SmtpClient : ISmtpClient
    {
        #region Private
        private readonly System.Net.Mail.SmtpClient _smtpClient;
        #endregion

        public SmtpClient(String server, Int32 port, String username, String password)
        {
            _smtpClient = new System.Net.Mail.SmtpClient
            {
                Port = port,
                Host = server,
                EnableSsl = false,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(username, password),
            };
        }

        public void Send(ISmtpMailMessage message)
        {
            try
            {
                using (var mex = message.ToNetMessage())
                {
                    _smtpClient.Send(mex);
                }
            }
            catch (Exception ex)
            {
                throw new SmtpException(ex);
            }
        }
    }
}