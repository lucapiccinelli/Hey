using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Hey.Api.Rest
{
    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private readonly TimeSpan _jobExpirationTimeout;

        public ProlongExpirationTimeAttribute()
            :this(TimeSpan.FromDays(7))
        {
        }

        public ProlongExpirationTimeAttribute(TimeSpan jobExpirationTimeout)
        {
            _jobExpirationTimeout = jobExpirationTimeout;
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = _jobExpirationTimeout;
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}