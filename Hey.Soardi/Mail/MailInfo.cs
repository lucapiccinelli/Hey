using System;
using System.Configuration;
using Hey.Core.Services.Mail;

namespace Hey.Soardi.Mail
{
    public class MailInfo
    {
        public string From { get; }
        public SecurityEnum Security { get; }
        public string Hostname { get; }
        public int Hostport { get; }
        public string Username { get; }
        public string Password { get; }

        private MailInfo(string hostname, int hostport, string username, string password, string from, SecurityEnum security)
        {
            From = from;
            Security = security;
            Hostname = hostname;
            Hostport = hostport;
            Username = username;
            Password = password;
        }

        public static MailInfo FromConfig()
        {
            SecurityEnum security;
            string hostname = ConfigurationManager.AppSettings["SmtpHostname"];
            int hostport = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string username = ConfigurationManager.AppSettings["SmtpUser"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string from = ConfigurationManager.AppSettings["DefaultSender"];
            Enum.TryParse(ConfigurationManager.AppSettings["SmtpSecurity"], out security);

            return new MailInfo(hostname, hostport, username, password, from, security);
        }
    }
}