using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Api.Rest.Service;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Api.Rest.Tests
{
    [TestFixture()]
    class HangfireHeyServiceTests
    {
        [Test]
        public void TestHandlingOfARememberRequest()
        {
            HeyService service = new HeyService();
        }
    }
}
