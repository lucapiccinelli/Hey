namespace Hey.Core.Services.Mail.Smtp
{
    public interface ISmtpMailMessage
    {
        System.Net.Mail.MailMessage ToNetMessage();

#pragma warning disable 0618
        System.Web.Mail.MailMessage ToWebMessage();
#pragma warning restore 0618

        void Save(string filename);
    }
}