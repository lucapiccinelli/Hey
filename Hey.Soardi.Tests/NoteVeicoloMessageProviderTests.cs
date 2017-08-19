using System;
using NUnit.Framework;

namespace Hey.Soardi.Tests
{
    [TestFixture]
    public class NoteVeicoloMessageProviderTests
    {
        [Test]
        public void TestThatMessageProviderCanRetreiveTheCorrectString()
        {
            NoteVeicoloMessageProvider messageProvider = new NoteVeicoloMessageProvider(10343);
            string txt = messageProvider.GetText();

            Assert.AreEqual("BANANA", txt);
        }
    }
}