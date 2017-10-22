using System;
using Hey.Core.Models;
using NCrontab;

namespace Hey.Core
{
    public class FindDatesFromHeyRemember
    {
        private readonly HeyRememberDto _hey;

        public FindDatesFromHeyRemember(HeyRememberDto hey)
        {
            _hey = hey;
        }

        public DateTime Next()
        {
            DateTime when = _hey.When[0];
            return Next(
                when.Date == DateTime.Today
                    ? DateTime.UtcNow 
                    : when.Subtract(TimeSpan.FromMinutes(1)));
        }

        public DateTime Next(DateTime from)
        {
            return _hey.CronExpression == string.Empty
                ? _hey.When[0]
                : CrontabSchedule.Parse(_hey.CronExpression).GetNextOccurrence(from);
        }
    }
}