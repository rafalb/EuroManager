using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using NUnit.Framework;

namespace EuroManager.Common.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests : UnitTestFixture
    {
        [Test]
        public void ShouldRecognizeSameDays()
        {
            DateTime date1 = new DateTime(2012, 05, 03);
            DateTime date2 = new DateTime(2012, 05, 03, 13, 41, 00);

            Assert.That(date1.IsSameDay(date2), Is.True);
        }

        [Test]
        public void ShouldRecognizeDifferentDays()
        {
            DateTime date1 = new DateTime(2012, 05, 03);
            DateTime date2 = new DateTime(2011, 05, 03);

            Assert.That(date1.IsSameDay(date2), Is.False);
        }

        [Test]
        public void ShouldGiveNextWeekday()
        {
            DateTime saturday = new DateTime(2012, 04, 21);
            DateTime nextMonday = saturday.Next(DayOfWeek.Monday);

            Assert.That(nextMonday, Is.EqualTo(saturday.AddDays(2)));
        }

        [Test]
        public void ShouldGiveSameDayWhenWeekdayMatches()
        {
            DateTime saturday = new DateTime(2012, 04, 21);
            DateTime nextSaturday = saturday.Next(DayOfWeek.Saturday);

            Assert.That(nextSaturday, Is.EqualTo(saturday));
        }
    }
}
