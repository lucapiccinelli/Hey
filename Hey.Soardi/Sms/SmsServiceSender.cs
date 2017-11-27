using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;
using Hey.Core;
using Hey.Core.Services;
using Hey.Core.Tests;
using Hey.Soardi.Sms.Exceptions;

namespace Hey.Soardi.Sms
{
    public class SmsServiceSender : ISenderService 
    {
        private const int IdApi = 5;
        private const string ItalyPrefix = "+39";

        private readonly string _smsUser;
        private readonly string _smsPassword;
        private readonly string _senderNumber;
        private readonly ISmsXmlRpcProxy _smsProxy;
        private PhoneNumberNormalizer _normalizePhoneNumber;


        public SmsServiceSender(string smsUser, string smsPassword, string senderNumber)
            :this(smsUser, smsPassword, senderNumber, XmlRpcProxyGen.Create<ISmsXmlRpcProxy>())
        {
        }

        public SmsServiceSender(string smsUser, string smsPassword, string senderNumber, ISmsXmlRpcProxy proxy)
        {
            _smsUser = smsUser;
            _smsPassword = smsPassword;
            _senderNumber = senderNumber;
            _smsProxy = proxy;

            _normalizePhoneNumber = new PhoneNumberNormalizer();
        }

        public void Send(IMessageProvider messageProvider, string receiverString)
        {
            double credit = _smsProxy.GetCredit(new AuthDto()
            {
                AuthLogin = _smsUser,
                AuthPassword = _smsPassword
            });

            if (credit >= 1)
            {
                XmlRpcStruct[] status = _smsProxy.Send(new SendSmsDto()
                {
                    AuthLogin = _smsUser,
                    AuthPassword = _smsPassword,
                    Sms = new []
                    {
                        new SmsDataDto()
                        {
                            Destination = NormalizeDestination(receiverString),
                            Sender = NormalizeSender(_senderNumber),
                            Body = MakeText(messageProvider),
                            IdApi = IdApi
                        }, 
                    }
                });
            }
            else
            {
                throw new SmsCreditException($"Credito insufficente: {credit}", credit);
            }
        }

        private string NormalizeSender(string senderNumber)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes($"+{NormalizePhoneNumber(senderNumber)}"));
        }

        private string NormalizeDestination(string receiverString)
        {
            return NormalizePhoneNumber(receiverString);
        }
        private string NormalizePhoneNumber(string phoneNumber)
        {
            return _normalizePhoneNumber.Normalize(phoneNumber);
        }

        private string MakeText(IMessageProvider messageProvider)
        {
            string text = $"{messageProvider.GetAbstract()}{Environment.NewLine}{messageProvider.GetText()}";
            return Convert.ToBase64String(Encoding.Default.GetBytes(text));
        }
    }
}
