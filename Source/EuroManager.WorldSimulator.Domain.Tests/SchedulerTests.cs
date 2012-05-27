using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Domain;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class SchedulerTests : UnitTestFixture
    {
        private World world;
        private Scheduler scheduler;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            world = new World("Test", DateTime.Now, 2012);
            scheduler = new Scheduler();
        }

        [Test]
        public void ShouldCalculateRoundCountForEvenTeamCount()
        {
            Assert.That(scheduler.CalculateRoundCount(8), Is.EqualTo(7));
        }

        [Test]
        public void ShouldCalculateRoundCountForOddTeamCount()
        {
            Assert.That(scheduler.CalculateRoundCount(5), Is.EqualTo(5));
        }

        [Test]
        public void ShouldScheduleFixturesForSingleStageTournament()
        {
            var roundDates = scheduler.ScheduleRoundDates(new DateTime(2012, 06, 08), new DateTime(2012, 07, 31), DayOfWeek.Wednesday, 1, 3);

            Assert.That(roundDates.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ShouldScheduleFirstRoundInTheFirstWeek()
        {
            DateTime startDate = new DateTime(2012, 08, 01);
            DateTime endDate = new DateTime(2013, 05, 30);

            var roundDates = scheduler.ScheduleRoundDates(startDate, endDate, DayOfWeek.Saturday, 1, 19, 19);
            DateTime firstRoundDate = roundDates.OrderBy(d => d).First();

            Assert.That(firstRoundDate, Is.GreaterThanOrEqualTo(startDate).And.LessThan(startDate.AddDays(7)));
        }

        [Test]
        public void ShouldScheduleLastRoundInTheLastWeek()
        {
            LeagueSeason leagueSeason = CreateLeagueSeason(A.Team.InWorld(world).Repeat(20));
            var fixtures = CreateLeagueSchedule(leagueSeason);
            Fixture lastFixture = fixtures.OrderBy(f => f.Date).Last();

            Assert.That(lastFixture.Date, Is.GreaterThanOrEqualTo(leagueSeason.EndDate.AddDays(-7)).And.LessThan(leagueSeason.EndDate));
        }

        [Test]
        public void ShouldScheduleFixturesAtProvidedDates()
        {
            DateTime[] roundDates = Enumerable.Range(0, 6).Select(i => A.Date.AddDays(3 * i)).ToArray();
            LeagueSeason leagueSeason = CreateLeagueSeason(A.Team.InWorld(world).Repeat(4));

            var fixtures = scheduler.ScheduleLeagueFixtures(leagueSeason, roundDates, true, leagueSeason.Teams);
            var fixtureDates = fixtures.Select(f => f.Date).Distinct();

            Assert.That(fixtureDates, Is.EquivalentTo(roundDates));
        }

        [Test]
        public void ShouldScheduleFixturesForAllTeamsWhenTeamCountIsEven()
        {
            LeagueSeason leagueSeason = CreateLeagueSeason(A.Team.InWorld(world).Repeat(18));
            var fixtures = CreateLeagueSchedule(leagueSeason);

            Assert.That(leagueSeason.Teams, Has.All.Matches<Team>(
                t1 => leagueSeason.Teams.Where(t2 => t2 != t1).All(
                    t2 => fixtures.Count(f => f.Team1 == t1 && f.Team2 == t2) == 1)));
        }

        [Test]
        public void ShouldScheduleFixturesForAllTeamsWhenTeamCountIsOdd()
        {
            LeagueSeason leagueSeason = CreateLeagueSeason(A.Team.InWorld(world).Repeat(11));
            var fixtures = CreateLeagueSchedule(leagueSeason);

            Assert.That(leagueSeason.Teams, Has.All.Matches<Team>(
                t1 => leagueSeason.Teams.Where(t2 => t2 != t1).All(
                    t2 => fixtures.Count(f => f.Team1 == t1 && f.Team2 == t2) == 1)));
        }

        [Test]
        public void ShouldScheduleBreak()
        {
            var roundDates = scheduler.ScheduleRoundDates(new DateTime(2012, 08, 01), new DateTime(2013, 05, 31), DayOfWeek.Saturday, 1, 3, 3);

            Assert.That(roundDates, Has.None.GreaterThan(new DateTime(2012, 08, 21)).And.LessThan(new DateTime(2013, 05, 10)));
        }

        [Test]
        public void ShouldScheduleAtLeastHalfOfRoundsBeforeBreak()
        {
            var roundDates = scheduler.ScheduleRoundDates(new DateTime(2012, 08, 01), new DateTime(2013, 05, 31), DayOfWeek.Saturday, 1, 2, 2, 5, 2, 2);

            Assert.That(roundDates.Skip(9), Has.None.LessThan(new DateTime(2013, 05, 01)));
        }

        [Test]
        public void ShouldScheduleRestOfRoundsAfterBreak()
        {
            var roundDates = scheduler.ScheduleRoundDates(new DateTime(2012, 08, 01), new DateTime(2013, 05, 31), DayOfWeek.Saturday, 1, 2, 2, 5, 2, 2);

            Assert.That(roundDates.Skip(9), Has.None.LessThan(new DateTime(2013, 05, 01)));
        }

        private IEnumerable<Fixture> CreateLeagueSchedule(LeagueSeason leagueSeason)
        {
            return scheduler.ScheduleLeagueFixtures(leagueSeason, leagueSeason.Teams,
                leagueSeason.StartDate, leagueSeason.EndDate, leagueSeason.DayOfWeek, leagueSeason.Frequency);
        }

        private LeagueSeason CreateLeagueSeason(IEnumerable<Team> teams)
        {
            var nationalLeague = new NationalLeague(world, "Test");
            var division = new League(nationalLeague, "Test", 1, DayOfWeek.Saturday, 1);
            return new LeagueSeason(division, new DateTime(2012, 08, 01), new DateTime(2013, 05, 30), teams.ToArray());
        }
    }
}
