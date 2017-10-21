using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Core.Tests
{
    [TestFixture]
    class FindDatesFromHeyRememberTest
    {
        [TestCase("30 21 * * *", "21/10/2017 21:30")]
        [TestCase("30 21 * * 2", "24/10/2017 21:30")]
        [TestCase("30 12 21 10 *", "21/10/2018 12:30")]
        [TestCase("", "21/10/2017 15:30")]
        public void GivenAHeyRemember_WithACronExpression_NextDateShouldEvaluteAsFollows(string cronExpression, string expectedDateStr)
        {
            DateTime startingDate = DateTime.Parse("21/10/2017 15:30");
            HeyRememberDto hey = new HeyRememberDto()
            {
                When = new [] {startingDate},
                CronExpression = cronExpression
            };

            DateTime expected = DateTime.Parse(expectedDateStr);
            FindDatesFromHeyRemember findDates = new FindDatesFromHeyRemember(hey);
            DateTime nextDate = findDates.Next(startingDate);

            Assert.AreEqual(expected, nextDate);
        }
    }
}
