using CookComputing.XmlRpc;

namespace Hey.Soardi.Sms
{
    public class AuthDto
    {
        [XmlRpcMember("authlogin")]
        public string AuthLogin { get; set; }

        [XmlRpcMember("authpasswd")]
        public string AuthPassword { get; set; }
    }
}