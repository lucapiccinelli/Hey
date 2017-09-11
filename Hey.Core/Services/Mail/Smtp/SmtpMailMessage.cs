using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SmtpMailMessage : ISmtpMailMessage
    {
        private readonly bool _htmlBody;

        #region Field
        private readonly String _from;
        public string Body { get; set; }

        public List<string> To { get; private set; }

        private readonly List<String> _cc;
        private readonly List<String> _bcc;
        private readonly List<String> _attachments;
        private readonly List<string> _readNotificationAddresses;
        private List<string> _to;

        #endregion

        /// <summary>
        /// Crea un nuovo messaggio
        /// </summary>
        /// <param name="from">Indirizzo mittente</param>
        /// <param name="to">Indirizzi destinazione separati da virgola</param>
        /// <param name="cc">Indirizzi copia separati da virgola</param>
        /// <param name="bcc">Indirizzi copia nascosta separati da virgola</param>
        /// <param name="readNotificationAddress">[Opzionale] Richiesta della ricevuta di lettura</param>
        /// <param name="attachments">[Opzionale] Lista degli allegati</param>
        /// <param name="htmlBody"></param>
        /// /// 
        public SmtpMailMessage(string from, string cc, string bcc, string readNotificationAddress = null, List<string> attachments = null, bool htmlBody = false)
        {
            _htmlBody = htmlBody;
            _from = from?.Trim();
            _cc = splitCommaAddresses(cc?.Trim());
            _bcc = splitCommaAddresses(bcc?.Trim());
            _readNotificationAddresses = splitCommaAddresses(readNotificationAddress?.Trim());
            
            _attachments = attachments ?? new List<String>();
        }

        public string Subject {get; set;}

        public void AddAttachments(String path)
        {
            _attachments.Add(path);
        }

        public void ToFromString(string to)
        {
            To = splitCommaAddresses(to?.Trim());
        }

        public void Save(string filename)
        {
            // luca20150814:
            // L' unica maniera che ho trovato per salvare il file è utilizzando ancora Net.Mail.SmtpClient 
            // specificando DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory. E' possibile dirgli
            // solo il nome dell directory e non il nome del file; per questo motivo uso una directory
            // temporanea locale e poi sposto l'unico file presente rinominandolo.

            using (MailMessage message = ToNetMessage())
            {
                string tmpdirname = Path.GetRandomFileName();
                Directory.CreateDirectory(tmpdirname);
                try
                {
                    var client = new System.Net.Mail.SmtpClient()
                    {
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = Path.GetFullPath(tmpdirname),
                    };
                    client.Send(message);

                    string[] files = Directory.GetFiles(tmpdirname);
                    if (files.Length == 1)
                    {
                        string tmpFilename = files[0];
                        File.Move(tmpFilename, filename);
                    }
                    else
                    {
                        throw new SmtpException("Errore in fase di salvataggio della mail nella cartella temporanea");
                    }
                }
                finally
                {
                    Directory.Delete(tmpdirname, true);
                }
            }
        }

        public System.Net.Mail.MailMessage ToNetMessage()
        {
            var message = new System.Net.Mail.MailMessage
            {
                Body = Body,
                Subject = Subject,
                IsBodyHtml = _htmlBody,
                From = new System.Net.Mail.MailAddress(_from),
            };

            // Inserisco i destinatari
            foreach (String to in To ?? new List<String>())
                message.To.Add(new System.Net.Mail.MailAddress(to));

            // Inserisco i CC
            foreach (String to in _cc ?? new List<String>())
                message.CC.Add(new System.Net.Mail.MailAddress(to));

            // Inserisco i BCC
            foreach (String to in _bcc ?? new List<String>())
                message.Bcc.Add(new System.Net.Mail.MailAddress(to));

            // Inserisco le mail per la notifica di lettura
            foreach (String readReceipt in _readNotificationAddresses ?? new List<String>())
                message.Headers.Add("Disposition-Notification-To", readReceipt);

            // Inserisco gli allegati
            foreach (String att in _attachments ?? new List<String>())
                message.Attachments.Add(new System.Net.Mail.Attachment(att));

            return message;
        }

#pragma warning disable 0618
        public System.Web.Mail.MailMessage ToWebMessage()
        {
            var message = new System.Web.Mail.MailMessage()
            {
                From = _from,
                Body = Body,
                Subject = Subject,
                To = semiColonJoinAddress(To),
                Cc = semiColonJoinAddress(_cc),
                Bcc = semiColonJoinAddress(_bcc),
                BodyFormat = _htmlBody ? System.Web.Mail.MailFormat.Html : System.Web.Mail.MailFormat.Text,
            };

            foreach (String readReceipt in _readNotificationAddresses ?? new List<String>())
                message.Headers.Add("Disposition-Notification-To", readReceipt);

            foreach (String att in _attachments ?? new List<String>())
                message.Attachments.Add(new System.Web.Mail.MailAttachment(Path.GetFullPath(att)));

            return message;
        }

#pragma warning restore 0618

        #region Private Helpers

        /// <summary>
        /// Splitta la stringa in ingresso sul carattere ',' e crea la lista
        /// </summary>
        private List<String> splitCommaAddresses(String str)
        {
            try
            {
                // Stringa vuota --> Lista vuota
                if (str == null)
                    return new List<String>();

                var values = str.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                return new List<String>(values);
            }
            catch (Exception ex)
            {
                throw new SmtpException(String.Format("Errore nell'interpretazione della lista degli indirizzi corrente: {0}", str), ex);
            }
        }

        private String semiColonJoinAddress(List<String> addressLis)
        {
            return (addressLis != null) ? String.Join(";", addressLis.ToArray()) : String.Empty;
        }
        #endregion
    }
}