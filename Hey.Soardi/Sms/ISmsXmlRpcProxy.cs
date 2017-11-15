using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;

namespace Hey.Soardi.Sms
{
    [XmlRpcUrl("https://secure.apisms.it/xmlrpc/BCP/provisioning.py")]
    public interface ISmsXmlRpcProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("send_sms")]
        XmlRpcStruct[] Send(SendSmsDto smsData);

        [XmlRpcMethod("get_credit")]
        double GetCredit(AuthDto authData);
    }
}
