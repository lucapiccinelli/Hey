using System;

namespace Hey.Soardi.Sms.Exceptions
{
    public class SmsCreditException : Exception
    {
        public double Credit { get; }

        public SmsCreditException(string msg, double credit)
            :base(msg)
        {
            Credit = credit;
        }
    }
}