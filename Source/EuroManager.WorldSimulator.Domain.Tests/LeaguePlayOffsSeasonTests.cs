using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class LeaguePlayOffsSeasonTests : UnitTestFixture
    {
        [Test]
        public void ShouldScheduleTiesFirstLeg()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date).Ending(A.Date.AddMonths(1))
                .WithPairs(4).Build();

            var tie = season.Ties.ElementAt(2);

            Assert.That(tie.FirstLegDate, Is.GreaterThan(season.StartDate).And.Matches<DateTime>(d => d.DayOfWeek == DayOfWeek.Monday));
        }

        [Test]
        public void ShouldScheduleTiesSecondLeg()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date).Ending(A.Date.AddMonths(1))
                .WithPairs(4).Build();

            var tie = season.Ties.ElementAt(0);

            Assert.That(tie.SecondLegDate, Is.LessThanOrEqualTo(season.EndDate).And.Matches<DateTime>(d => d.DayOfWeek == DayOfWeek.Monday));
        }

        [Test]
        public void ShouldScheduleAllTies()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date).Ending(A.Date.AddMonths(1))
                .WithPairs(4).Build();

            Assert.That(season.Ties.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldScheduleTiesForAllTeams()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date).Ending(A.Date.AddMonths(1))
                .WithPairs(4).Build();

            var tieTeams = season.Ties.SelectMany(t => new Team[] { t.Team1, t.Team2 }).ToArray();

            Assert.That(tieTeams, Is.EquivalentTo(season.Teams));
        }

        [Test]
        public void ShouldScheduleTieFixtures()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date.Next(DayOfWeek.Saturday)).Ending(A.Date.AddMonths(1))
                .WithPairs(4).Build();

            var fixtures = new List<Fixture>();
            season.ScheduleFixtures(f => fixtures.Add(f));

            Assert.That(fixtures.Count, Is.EqualTo(2 * 4));
        }

        [Test]
        public void ShouldFinishWhenAllTiesPlayed()
        {
            LeaguePlayOffsSeason season = A.LeaguePlayOffsSeason.On(DayOfWeek.Monday)
                .Starting(A.Date.Next(DayOfWeek.Saturday)).Ending(A.Date.AddMonths(1))
                .WithPairs(8).Build();

            FixtureSetTestHelper.PlayAllFixtures(season);

            Assert.That(season.IsFinished, Is.True);
        }
    }
}
