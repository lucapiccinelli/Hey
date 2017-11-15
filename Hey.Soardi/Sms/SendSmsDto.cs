using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Hey.Soardi.Sms
{
    public class SendSmsDto : AuthDto
    {
        public SendSmsDto()
        {
            Sms = new SmsDataDto[0];
        }

        [XmlRpcMember("sms")]
        public SmsDataDto[] Sms { get; set; }
    }
}