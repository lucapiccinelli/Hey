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
            return Next(DateTime.Now);
        }

        public DateTime Next(DateTime from)
        {
            return _hey.CronExpression == string.Empty
                ? _hey.When[0]
                : CrontabSchedule.Parse(_hey.CronExpression).GetNextOccurrence(from);
        }
    }
}