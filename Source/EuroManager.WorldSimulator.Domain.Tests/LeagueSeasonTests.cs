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
    public class LeagueSeasonTests : UnitTestFixture
    {
        [Test]
        public void ShouldScheduleFixtures()
        {
            var leagueSeason = A.LeagueSeason.Build();

            var fixtures = new List<Fixture>();
            leagueSeason.ScheduleFixtures(f => fixtures.Add(f));

            Assert.That(fixtures, Is.Not.Empty);
        }

        [Test]
        public void ShouldApplyResultToTeamStats()
        {
            var leagueSeason = A.LeagueSeason.Build();

            var fixtures = new List<Fixture>();
            leagueSeason.ScheduleFixtures(f => fixtures.Add(f));

            Fixture fixture = fixtures[3];
            leagueSeason.ApplyResult(A.MatchResult.ForFixture(fixture).Build());
            TeamStats teamStats = leagueSeason.GetStatsFor(fixture.Team1);

            Assert.That(teamStats.Played, Is.EqualTo(1));
        }

        [Test]
        public void ShouldFinishWhenAllMatchesPlayed()
        {
            var leagueSeason = A.LeagueSeason.Build();
            FixtureSetTestHelper.PlayAllFixtures(leagueSeason);

            Assert.That(leagueSeason.IsFinished, Is.True);
        }

        [Test]
        public void ShouldNotFinishUntilAllMatchesPlayed()
        {
            var leagueSeason = A.LeagueSeason.Build();

            var fixtures = new List<Fixture>();
            leagueSeason.ScheduleFixtures(f => fixtures.Add(f));

            var allFixturesExceptLast = fixtures.Except(fixtures.Last().AsEnumerable()).ToList();
            allFixturesExceptLast.ForEach(f => leagueSeason.ApplyResult(A.MatchResult.ForFixture(f).Build()));

            Assert.That(leagueSeason.IsFinished, Is.False);
        }

        [Test]
        public void ShouldIncludePromotedAndRelegatedTeamsForNewSeason()
        {
            var leagueSeason = A.LeagueSeason.Build();
            leagueSeason.PromoteTeam(leagueSeason.Teams.ElementAt(0));
            leagueSeason.RelegateTeam(leagueSeason.Teams.ElementAt(1));
            leagueSeason.RelegateTeam(leagueSeason.Teams.ElementAt(4));
            leagueSeason.PromoteTeam(leagueSeason.Teams.ElementAt(5));

            var promotedFromLower = new Team[] { A.Team.Build(), A.Team.Build() };
            var relegatedFromHigher = new Team[] { A.Team.Build(), A.Team.Build() };
            var newSeason = leagueSeason.AdvanceSeason(relegatedFromHigher, promotedFromLower);

            var expected = Enumerable
                .Union(leagueSeason.Teams.Skip(2).Take(2), leagueSeason.Teams.Skip(6))
                .Union(promotedFromLower)
                .Union(relegatedFromHigher)
                .ToArray();

            Assert.That(newSeason.Teams, Is.EquivalentTo(expected));
        }
    }
}
