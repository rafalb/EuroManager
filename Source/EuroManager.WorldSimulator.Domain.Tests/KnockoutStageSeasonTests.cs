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
    public class KnockoutStageSeasonTests : UnitTestFixture
    {
        [Test]
        public void ShouldScheduleFixturesForAllPairs()
        {
            KnockoutStageSeason season = A.KnockoutStageSeason.WithPairCount(4).Scheduled().Activated().Build();

            var fixtures = new List<Fixture>();
            season.ScheduleFixtures(f => fixtures.Add(f));

            Assert.That(fixtures.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldScheduleFixturesForAllTeams()
        {
            KnockoutStageSeason season = A.KnockoutStageSeason.WithPairCount(4).Scheduled().Activated().Build();

            var fixtures = new List<Fixture>();
            season.ScheduleFixtures(f => fixtures.Add(f));

            var fixtureTeams = fixtures.SelectMany(f => new Team[] { f.Team1, f.Team2 }).ToArray();

            Assert.That(fixtureTeams, Is.EquivalentTo(season.Teams));
        }

        [Test]
        public void ShouldPromoteWinners()
        {
            KnockoutStageSeason season = A.KnockoutStageSeason.WithPairCount(4).Scheduled().Activated().Build();

            var fixtures = new List<Fixture>();
            season.ScheduleFixtures(f => fixtures.Add(f));

            Fixture fixture = fixtures[2];
            season.ApplyResult(A.MatchResult.ForFixture(fixture).WonBy(fixture.Team2).Build());

            Assert.That(season.PromotedTeams, Contains.Item(fixture.Team2));
        }

        [Test]
        public void ShouldFinishWhenAllFixturesPlayed()
        {
            KnockoutStageSeason season = A.KnockoutStageSeason.WithPairCount(4).Scheduled().Activated().Build();
            FixtureSetTestHelper.PlayAllFixtures(season);

            Assert.That(season.IsFinished, Is.True);
        }
    }
}
