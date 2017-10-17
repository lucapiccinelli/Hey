using System;
using Hangfire;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    public class HangfireDependentTest
    {
        protected HeyController _heyController;
        protected string _scheduledId;
        protected HeyRememberDto _scheduledHeyRemember;
        protected BackgroundJobServer _backgroundJobServer;
        protected HangfireJobRepository _repository;
        protected string _processingId;
        protected HeyRememberDto _processingHeyRemember;
        protected string _failedId;
        protected HeyRememberDto _failedHeyRemember;
        protected HeyRememberDto _recurringHeyRemember;
        protected string _recurringId;

        [OneTimeSetUp]
        public void SetUp()
        {
            _backgroundJobServer = HangfireConfig.StartHangfire("TestHeyDb");
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            _repository = new HangfireJobRepository();
            var heyService = new HeyService(_repository);
            _heyController = new HeyController(heyService);

            _scheduledId = "1";
            _scheduledHeyRemember = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Name = "GetTests",
                Id = _scheduledId,
                DomainSpecificData = "[]",
                When = new[] { DateTime.Now + TimeSpan.FromMinutes(60) }
            };

            _processingId = "2";
            _processingHeyRemember = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Name = "GetTests",
                Id = _processingId,
                DomainSpecificData = "[]",
                When = new[] { DateTime.Now }
            };

            _failedId = "3";
            _failedHeyRemember = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Name = "FailTests",
                Id = _failedId,
                DomainSpecificData = "[]",
                When = new[] { DateTime.Now }
            };

            _recurringId = "4";
            _recurringHeyRemember = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Name = "RecurringTests",
                Id = _recurringId,
                DomainSpecificData = "[]",
                When = new[] { DateTime.Now },
                CronExpression = "* * * * *",
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _backgroundJobServer.Dispose();
        }
    }
}