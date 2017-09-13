using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Hey.Api.Rest.Schedules;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HangfireJobRepository : IJobRepository
    {
        private JobList<ScheduledJobDto> _scheduled;
        private JobList<ProcessingJobDto> _processing;
        private JobList<FailedJobDto> _failed;
        private readonly IMonitoringApi _hangfire;

        public HangfireJobRepository()
        {
            _hangfire = JobStorage.Current.GetMonitoringApi();
            Refresh();
        }

        public void Refresh()
        {
            _scheduled = _hangfire.ScheduledJobs(0, (int)_hangfire.ScheduledCount());
            _processing = _hangfire.ProcessingJobs(0, (int)_hangfire.ProcessingCount());
            _failed = _hangfire.FailedJobs(0, (int)_hangfire.FailedCount());
        }

        public IScheduleType MakeASchedulePrototype(HeyRememberDto heyRemember)
        {
            return DelayedScheduleType.MakePrototype();
        }

        public List<HeyRememberResultDto> GetJobs(string id)
        {
            List<HeyRememberResultDto> filteredScheduled = _scheduled
                .Select(pair => (HeyRememberDto)pair.Value.Job.Args[0])
                .Where(heyRemember => heyRemember.Id == id)
                .Select(heyRemember => new HeyRememberResultDto(heyRemember, HeyRememberStatus.Scheduled))
                .ToList();

            List<HeyRememberResultDto> filteredFailed = _failed
                .Select(pair => (HeyRememberDto)pair.Value.Job.Args[0])
                .Where(heyRemember => heyRemember.Id == id)
                .Select(heyRemember => new HeyRememberResultDto(heyRemember, HeyRememberStatus.Failed))
                .ToList();

            List<HeyRememberResultDto> filteredProcessing = _processing
                .Select(pair => (HeyRememberDto)pair.Value.Job.Args[0])
                .Where(heyRemember => heyRemember.Id == id)
                .Select(heyRemember => new HeyRememberResultDto(heyRemember, HeyRememberStatus.Processing))
                .ToList();

            var jobs = new List<HeyRememberResultDto>();
            jobs.AddRange(filteredScheduled);
            jobs.AddRange(filteredFailed);
            jobs.AddRange(filteredProcessing);

            return jobs;
        }
    }
}