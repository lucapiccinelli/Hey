using CookComputing.XmlRpc;

namespace Hey.Soardi.Sms
{
    public class SmsDataDto
    {
        [XmlRpcMember("sender")]
        public string Sender { get; set; }

        [XmlRpcMember("body")]
        public string Body { get; set; }

        [XmlRpcMember("destination")]
        public string Destination { get; set; }

        [XmlRpcMember("id_api")]
        public int IdApi { get; set; }
    }
}