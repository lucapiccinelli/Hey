using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Attributes;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture()]
    class HeyControllerHttpTests
    {
        private HeyController _heyController;
        private string _scheduledId;
        private HeyRememberDto _scheduledHeyRemember;
        private BackgroundJobServer _backgroundJobServer;

        [SetUp]
        public void SetUp()
        {
            _backgroundJobServer = HangfireConfig.StartHangfire();
            var repository = new HangfireJobRepository();
            var heyService = new HeyService(repository);
            _heyController = new HeyController(heyService);

            _scheduledId = "1";
            _scheduledHeyRemember = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Name = "GetTests",
                Id = _scheduledId,
                DomainSpecificData = "[]",
                When = new []{DateTime.Now + TimeSpan.FromMinutes(60)}
            };

            _heyController.Post(_scheduledHeyRemember);
            repository.Refresh();
        }

        [TearDown]
        public void TearDown()
        {
            _backgroundJobServer.Dispose();
        }

        [Test]
        public void TestGetOfAScheduledJobById()
        {
            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_scheduledId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Scheduled, result.First().Status);
            Assert.AreEqual(_scheduledHeyRemember, result.First().HeyRemember);
        }
    }

    internal class GetTestsClass
    {
        [FireMe("GetTests")]
        public void MethodToFire()
        {
            
        }
    }
}
