using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class GroupStatsTests : UnitTestFixture
    {
        [Test]
        public void ShouldCalculateRoundsPerTeam()
        {
            var groupStats = new GroupStats(1, A.Team.Repeat(5), isNeutralGround: true, hasReturnRound: false);

            Assert.That(groupStats.RoundsPerTeam, Is.EqualTo(4));
        }

        [Test]
        public void ShouldCalculateRoundsPerTeamWhenHasReturnRound()
        {
            var groupStats = new GroupStats(3, A.Team.Repeat(4), isNeutralGround: false, hasReturnRound: true);

            Assert.That(groupStats.RoundsPerTeam, Is.EqualTo(6));
        }

        [Test]
        public void ShouldCreateFixtures()
        {
            var groupStats = new GroupStats(5, A.Team.Repeat(9), isNeutralGround: false, hasReturnRound: true);
            var dates = Enumerable.Repeat(A.Date, 16).ToArray();
            var fixtures = groupStats.CreateFixtures(A.CupSeason.Build(), dates);

            Assert.That(fixtures, Is.Not.Empty);
        }

        [Test]
        public void ShouldApplyMatchResult()
        {
            var groupStats = new GroupStats(1, A.Team.Repeat(4), isNeutralGround: false, hasReturnRound: false);
            var result = A.MatchResult.ForTeams(groupStats.Teams.ElementAt(0), groupStats.Teams.ElementAt(2)).WithScore(0, 1).Build();

            bool hasAppliedResult = groupStats.TryApplyResult(result);

            Assert.That(hasAppliedResult, Is.True);
        }

        [Test]
        public void ShouldNotApplyMatchResultMeantForOtherGroup()
        {
            var groupStats = new GroupStats(1, A.Team.Repeat(4), isNeutralGround: false, hasReturnRound: false);
            var result = A.MatchResult.ForTeams(A.Team.Build(), groupStats.Teams.ElementAt(2)).WithScore(0, 1).Build();

            bool hasAppliedResult = groupStats.TryApplyResult(result);

            Assert.That(hasAppliedResult, Is.False);
        }

        [Test]
        public void ShouldUpdateTeamStatsAfterMatch()
        {
            var groupStats = new GroupStats(1, A.Team.Repeat(4), isNeutralGround: false, hasReturnRound: false);
            var result = A.MatchResult.ForTeams(groupStats.Teams.ElementAt(0), groupStats.Teams.ElementAt(2)).WithScore(0, 1).Build();

            groupStats.TryApplyResult(result);

            Assert.That(groupStats.TeamStats[2].Played, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCompleteWhenAllMatchesPlayed()
        {
            var groupStats = new GroupStats(1, A.Team.Repeat(4), isNeutralGround: false, hasReturnRound: false);

            var dates = Enumerable.Repeat(A.Date, 3).ToArray();
            var fixtures = groupStats.CreateFixtures(A.CupSeason.Build(), dates);
            PlayAllFixtures(groupStats, fixtures);

            Assert.That(groupStats.IsCompleted, Is.True);
        }

        private void PlayAllFixtures(GroupStats groupStats, IEnumerable<Fixture> fixtures)
        {
            foreach (var fixture in fixtures)
            {
                groupStats.TryApplyResult(A.MatchResult.ForFixture(fixture).WithScore(1, 0).Build());
            }
        }
    }
}
