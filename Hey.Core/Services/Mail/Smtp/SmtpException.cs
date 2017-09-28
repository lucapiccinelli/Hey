using System;

namespace Hey.Core.Services.Mail.Smtp
{
    public class SmtpException : Exception
    {
        public SmtpException(String msg) : base(msg) { }

        public SmtpException(String msg, Exception innerException) : base(msg, innerException){}
        
        public SmtpException(Exception innerException) : base(extracInnerExMessage(innerException), innerException) { }

        private static String extracInnerExMessage(Exception innerException)
        {
            Exception ex = innerException;

            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex.Message;
        }
    }
}