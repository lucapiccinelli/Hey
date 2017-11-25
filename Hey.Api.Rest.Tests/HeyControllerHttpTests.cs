using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Hangfire;
using Hey.Core;
using Hey.Core.Attributes;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture()]
    class HeyControllerHttpTests : HangfireDependentTest
    {
        [Test, Order(0)]
        public void TestDeleteOfANotExistingJob()
        {
            IHttpActionResult result = _heyController.Delete(_scheduledId);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Order(1)]
        public void TestUpdateOfANotExistingJob()
        {
            IHttpActionResult result = _heyController.Put(_scheduledId, _scheduledHeyRemember);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Order(10)]
        public void TestGetOfAScheduledJobById()
        {
            _heyController.Post(_scheduledHeyRemember);
            _repository.Refresh();

            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_scheduledId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Scheduled, result.First().Status);
            Assert.AreEqual(_scheduledHeyRemember, result.First().HeyRemember);
        }


        [Test, Order(11)]
        public void TestGetOfAFailedJobById()
        {
            HttpTestsClass.executed = false;
            _heyController.Post(_failedHeyRemember);
            while (!HttpTestsClass.executed) { }
            Thread.Sleep(1000);
            _repository.Refresh();
            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_failedId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Failed, result.First().Status);
            Assert.AreEqual(_failedHeyRemember, result.First().HeyRemember);
        }

        [Test, Order(12)]
        public void TestGetOfAProcessingJobById()
        {
            HttpTestsClass.executed = false;
            _heyController.Post(_processingHeyRemember);
            while (!HttpTestsClass.executed){}
            _repository.Refresh();
            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_processingId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Processing, result.First().Status);
            Assert.AreEqual(_processingHeyRemember, result.First().HeyRemember);
        }

        [Test, Order(13)]
        public void TestGetOfASuccededJobById()
        {
            HttpTestsClass.executed = false;
            HttpTestsClass.done = false;
            _heyController.Post(_succededHeyRemember);
            while (!HttpTestsClass.done) { }
            Thread.Sleep(2000);
            _repository.Refresh();
            //IEnumerable<HeyRememberResultDto> result = _heyController.GetWithSucceded(_succededId);
            //Assert.AreEqual(1, result.Count());
            //Assert.AreEqual(HeyRememberStatus.Succeded, result.First().Status);
            //Assert.AreEqual(_succededHeyRemember, result.First().HeyRemember);
        }

        [Test, Order(14)]
        public void TestGetOfARecurringJobById()
        {
            _heyController.Post(_recurringHeyRemember);
            _repository.Refresh();

            HeyRememberDto expectedHeyRemember = new HeyRememberDto(_recurringHeyRemember)
            {
                When = {[0] = new FindDatesFromHeyRemember(_recurringHeyRemember).Next()}
            };

            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_recurringId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Recurring, result.First().Status);
            Assert.AreEqual(expectedHeyRemember, result.First().HeyRemember);
        }

        [Test, Order(20)]
        public void TestUpdateOfAScheduledJob()
        {
            HeyRememberDto scheduledCopy = new HeyRememberDto(_scheduledHeyRemember);
            scheduledCopy.When[0] += TimeSpan.FromMinutes(60);
            IHttpActionResult resultAction = _heyController.Put(_scheduledId, scheduledCopy);
            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<HeyRememberDto>>(resultAction);

            _repository.Refresh();

            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_scheduledId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Scheduled, result.First().Status);
            Assert.AreEqual(scheduledCopy, result.First().HeyRemember);
        }

        [Test, Order(21)]
        public void TestUpdateOfARecurringJob()
        {
            HeyRememberDto recurringCopy = new HeyRememberDto(_recurringHeyRemember);
            recurringCopy.When[0] += TimeSpan.FromMinutes(60);
            IHttpActionResult resultAction = _heyController.Put(_recurringId, recurringCopy);

            recurringCopy.When[0] = new FindDatesFromHeyRemember(recurringCopy).Next();

            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<HeyRememberDto>>(resultAction);

            _repository.Refresh();

            IEnumerable<HeyRememberResultDto> result = _heyController.Get(_recurringId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(HeyRememberStatus.Recurring, result.First().Status);
            Assert.AreEqual(recurringCopy, result.First().HeyRemember);
        }

        [Test, Order(100)]
        public void TestDeleteOfAScheduledJobById()
        {
            IHttpActionResult result = _heyController.Delete(_scheduledId);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test, Order(101)]
        public void TestDeleteOfAFailedJobById()
        {
            IHttpActionResult result = _heyController.Delete(_failedId);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test, Order(102)]
        public void TestDeleteOfASuccededJobById()
        {
            IHttpActionResult result = _heyController.Delete(_succededId);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test, Order(103)]
        public void TestDeleteOfARecurringJobById()
        {
            IHttpActionResult result = _heyController.Delete(_recurringId);
            Assert.IsInstanceOf<OkResult>(result);
        }
    }

    internal class HttpTestsClass
    {
        public static bool executed = false;
        public static bool done = false;
        public static int executionCount = 0;

        [FireMe("GetTests")]
        public void MethodToFire()
        {
            executed = true;
            Thread.Sleep(2000);
            done = true;
        }

        [FireMe("GetSuccessTests")]
        public void MethodToFireWIthSuccess()
        {
            done = true;
        }

        [FireMe("FailTests")]
        public void FailMethod()
        {
            executed = true;
            throw new Exception("blaaaaaaaaaaaaaaaaaaaaaa");
        }

        [FireMe("RecurringTests")]
        public void RecurringMethod()
        {
            ++executionCount;
        }
    }
}
