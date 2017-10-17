using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Api.Rest.Schedules;
using Hey.Core.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class HangfireJobRepositoryTests : HangfireDependentTest
    {
        [Test]
        public void TestWithEmptyCronExpression_ShouldReturnDelayedScheduleType()
        {
            HeyRememberDto heyRemember = new HeyRememberDto
            {
                CronExpression = string.Empty
            };
            IScheduleType scheduleType = _repository.MakeASchedulePrototype(heyRemember);
            Assert.IsInstanceOf(typeof(DelayedScheduleType), scheduleType);
        }
        
        [Test]
        public void TestWithCronExpression_ShouldReturnRecurringScheduleType()
        {
            HeyRememberDto heyRemember = new HeyRememberDto
            {
                CronExpression = "* * * * * *"
            };
            IScheduleType scheduleType = _repository.MakeASchedulePrototype(heyRemember);
            Assert.IsInstanceOf(typeof(RecurringScheduleType), scheduleType);
        }
    }
}
