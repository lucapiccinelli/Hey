using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Soardi.Sms;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Soardi.Tests
{
    [TestFixture()]
    class NotEnoughCreditEmailTests
    {
        [Test]
        public void TestThat_NotEnoughCreditEmail_CanSend_Emails()
        {
            NotEnoughCreditEmail email = new NotEnoughCreditEmail("luca.picci@gmail.com");
            Assert.DoesNotThrow(() => email.Handle(new NotEnoughCreditDto(0.5)));
        }
    }
}
