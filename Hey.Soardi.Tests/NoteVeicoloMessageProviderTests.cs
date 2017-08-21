using System;
using System.Net.NetworkInformation;
using NUnit.Framework;

namespace Hey.Soardi.Tests
{
    [TestFixture]
    public class NoteVeicoloMessageProviderTests
    {
        [Test]
        public void TestThatMessageProviderCanRetreiveTheCorrectString()
        {
            var ping = new Ping();
            PingReply reply = ping.Send("192.168.0.200");
            if (reply.Status != IPStatus.Success)
            {
                Assert.Ignore("La VPN e' spenta");
            }

            NoteVeicoloMessageProvider messageProvider = new NoteVeicoloMessageProvider(10343);
            string txt = messageProvider.GetText();

            Assert.AreEqual("BANANA", txt);
        }
    }
}