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
        private List<RecurringJobDto> _recurring;
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
            _recurring = JobStorage.Current.GetConnection().GetRecurringJobs();
        }

        public IScheduleType MakeASchedulePrototype(HeyRememberDto heyRemember)
        {
            return heyRemember.CronExpression == string.Empty 
                ? DelayedScheduleType.MakePrototype()
                : RecurringScheduleType.MakePrototype();
        }

        public List<HeyRememberResultDto> GetJobs(string id)
        {
            List<HeyRememberResultDto> filteredScheduled = _scheduled
                .Select(pair => new KeyValuePair<string, HeyRememberDto>(pair.Key, (HeyRememberDto)pair.Value.Job.Args[0]))
                .Where(pair => pair.Value.Id == id)
                .Select(pair => new HeyRememberResultDto(pair.Key, pair.Value, HeyRememberStatus.Scheduled))
                .ToList();

            List<HeyRememberResultDto> filteredFailed = _failed
                .Select(pair => new KeyValuePair<string, HeyRememberDto>(pair.Key, (HeyRememberDto)pair.Value.Job.Args[0]))
                .Where(pair => pair.Value.Id == id)
                .Select(pair => new HeyRememberResultDto(pair.Key, pair.Value, HeyRememberStatus.Failed))
                .ToList();

            List<HeyRememberResultDto> filteredProcessing = _processing
                .Select(pair => new KeyValuePair<string, HeyRememberDto>(pair.Key, (HeyRememberDto)pair.Value.Job.Args[0]))
                .Where(pair => pair.Value.Id == id)
                .Select(pair => new HeyRememberResultDto(pair.Key, pair.Value, HeyRememberStatus.Processing))
                .ToList();

            var jobs = new List<HeyRememberResultDto>();
            jobs.AddRange(filteredScheduled);
            jobs.AddRange(filteredFailed);
            jobs.AddRange(filteredProcessing);

            //Recurring
            List<HeyRememberResultDto> filteredRecurring = _recurring
                .Select(recurringDto => new HeyRememberResultDto(recurringDto.Id, (HeyRememberDto)recurringDto.Job.Args[0], HeyRememberStatus.Scheduled))
                .Where(heyRememberRes => heyRememberRes.HeyRemember.Id == id)
                .ToList();
            jobs.AddRange(filteredRecurring);

            return jobs;
        }

        public void DeleteJobs(List<HeyRememberResultDto> heyRemembers)
        {
            heyRemembers
                .ForEach(heyRemembersResult =>
                {
                    if (string.IsNullOrWhiteSpace(heyRemembersResult.HeyRemember.CronExpression))
                    {
                        BackgroundJob.Delete(heyRemembersResult.JobId);
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(heyRemembersResult.JobId);
                    }
                });
        }
    }
}