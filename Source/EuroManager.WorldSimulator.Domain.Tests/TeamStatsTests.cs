using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class TeamStatsTests : UnitTestFixture
    {
        [Test]
        public void ShouldCalculateGoalDifference()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(2, 5);

            Assert.That(stats.GoalDifference, Is.EqualTo(-3));
        }

        [Test]
        public void ShouldCountPlayedMatches()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(1, 0);
            stats.ApplyResult(2, 2);

            Assert.That(stats.Played, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountWins()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(1, 0);
            stats.ApplyResult(0, 1);
            stats.ApplyResult(2, 1);

            Assert.That(stats.Wins, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountDraws()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(1, 1);
            stats.ApplyResult(0, 1);
            stats.ApplyResult(2, 1);

            Assert.That(stats.Draws, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCountLosses()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(1, 1);
            stats.ApplyResult(0, 1);
            stats.ApplyResult(2, 4);

            Assert.That(stats.Losses, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCalculatePoints()
        {
            var stats = new TeamStats(A.Team.Build());
            stats.ApplyResult(1, 0);
            stats.ApplyResult(0, 1);
            stats.ApplyResult(2, 4);
            stats.ApplyResult(3, 1);
            stats.ApplyResult(1, 1);
            stats.ApplyResult(2, 0);
            stats.ApplyResult(1, 2);
            stats.ApplyResult(2, 2);

            Assert.That(stats.Points, Is.EqualTo(11));
        }

        [Test]
        public void ShouldConsiderHigherPointsAsHigherPosition()
        {
            var stats1 = new TeamStats(A.Team.Build());
            stats1.ApplyResult(1, 0);
            var stats2 = new TeamStats(A.Team.Build());
            stats2.ApplyResult(0, 0);

            Assert.That(stats1, Is.GreaterThan(stats2));
        }

        [Test]
        public void ShouldConsiderHigherGoalDifferenceAsHigherPositionWhenPointsAreEqual()
        {
            var stats1 = new TeamStats(A.Team.Build());
            stats1.ApplyResult(1, 0);
            var stats2 = new TeamStats(A.Team.Build());
            stats2.ApplyResult(2, 0);

            Assert.That(stats1, Is.LessThan(stats2));
        }

        [Test]
        public void ShouldConsiderHigherGoalsScoredAsHigherPositionWhenPointsAndGoalDifferenceAreEqual()
        {
            var stats1 = new TeamStats(A.Team.Build());
            stats1.ApplyResult(1, 2);
            var stats2 = new TeamStats(A.Team.Build());
            stats2.ApplyResult(2, 3);

            Assert.That(stats1, Is.LessThan(stats2));
        }
    }
}
