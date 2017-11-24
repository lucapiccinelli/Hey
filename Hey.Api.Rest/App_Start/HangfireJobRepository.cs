using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Hey.Api.Rest.Schedules;
using Hey.Core;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HangfireJobRepository : IJobRepository
    {
        private JobList<ScheduledJobDto> _scheduled;
        private JobList<ProcessingJobDto> _processing;
        private JobList<FailedJobDto> _failed;
        private JobList<SucceededJobDto> _succeded;
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
            _succeded = _hangfire.SucceededJobs(0, (int)_hangfire.SucceededListCount());
            _recurring = JobStorage.Current.GetConnection().GetRecurringJobs();
        }

        public IScheduleType MakeASchedulePrototype(HeyRememberDto heyRemember)
        {
            return heyRemember.CronExpression == string.Empty 
                ? DelayedScheduleType.MakePrototype()
                : RecurringScheduleType.MakePrototype();
        }

        public List<HeyRememberResultDto> GetJobs(string id, bool listSucceded = false)
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

            if (listSucceded)
            {
                List<HeyRememberResultDto> succededProcessing = _succeded
                    .Select(pair => new KeyValuePair<string, HeyRememberDto>(pair.Key, (HeyRememberDto)pair.Value.Job.Args[0]))
                    .Where(pair => pair.Value.Id == id)
                    .Select(pair => new HeyRememberResultDto(pair.Key, pair.Value, HeyRememberStatus.Succeded))
                    .ToList();

                jobs.AddRange(succededProcessing);
            }

            //Recurring
            List<HeyRememberResultDto> filteredRecurring = _recurring
                .Select(recurringDto =>
                {
                    HeyRememberDto nextDateHeyRember = (HeyRememberDto)recurringDto.Job.Args[0];
                    nextDateHeyRember.When[0] = new FindDatesFromHeyRemember(nextDateHeyRember).Next();
                    return new HeyRememberResultDto(recurringDto.Id, nextDateHeyRember, HeyRememberStatus.Recurring);
                })
                .Where(heyRememberRes => heyRememberRes.HeyRemember.Id == id)
                .ToList();
            jobs.InsertRange(0, filteredRecurring);

            return jobs;
        }

        public void DeleteJobs(List<HeyRememberResultDto> heyRemembers)
        {
            heyRemembers
                .ForEach(heyRemembersResult =>
                {
                    if (heyRemembersResult.Status != HeyRememberStatus.Recurring)
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