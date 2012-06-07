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
        private TeamStats stats;
        private Team team1;
        private Team team2;
        private Player player;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            player = new Player(A.World.Build(), "Test", 75, 75, 75);
            team1 = A.Team.WithPlayers(player).Build();
            team2 = A.Team.Build();
            stats = new TeamStats(team1);
        }

        [Test]
        public void ShouldCalculateGoalDifference()
        {
            stats.ApplyResult(ResultForScore(2, 5));

            Assert.That(stats.GoalDifference, Is.EqualTo(-3));
        }

        [Test]
        public void ShouldCountPlayedMatches()
        {
            stats.ApplyResult(ResultForScore(1, 0));
            stats.ApplyResult(ResultForScore(2, 2));

            Assert.That(stats.Played, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountWins()
        {
            stats.ApplyResult(ResultForScore(1, 0));
            stats.ApplyResult(ResultForScore(0, 1));
            stats.ApplyResult(ResultForScore(2, 1));

            Assert.That(stats.Wins, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountDraws()
        {
            stats.ApplyResult(ResultForScore(1, 1));
            stats.ApplyResult(ResultForScore(0, 1));
            stats.ApplyResult(ResultForScore(2, 1));

            Assert.That(stats.Draws, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCountLosses()
        {
            stats.ApplyResult(ResultForScore(1, 1));
            stats.ApplyResult(ResultForScore(0, 1));
            stats.ApplyResult(ResultForScore(2, 4));

            Assert.That(stats.Losses, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCalculatePoints()
        {
            stats.ApplyResult(ResultForScore(1, 0));
            stats.ApplyResult(ResultForScore(0, 1));
            stats.ApplyResult(ResultForScore(2, 4));
            stats.ApplyResult(ResultForScore(3, 1));
            stats.ApplyResult(ResultForScore(1, 1));
            stats.ApplyResult(ResultForScore(2, 0));
            stats.ApplyResult(ResultForScore(1, 2));
            stats.ApplyResult(ResultForScore(2, 2));

            Assert.That(stats.Points, Is.EqualTo(11));
        }

        [Test]
        public void ShouldConsiderHigherPointsAsHigherPosition()
        {
            var stats1 = new TeamStats(team1);
            var stats2 = new TeamStats(team2);
            
            stats1.ApplyResult(A.MatchResult.ForTeams(team1, A.Team.Build()).WithScore(1, 0).Build());
            stats2.ApplyResult(A.MatchResult.ForTeams(team2, A.Team.Build()).WithScore(0, 0).Build());

            Assert.That(stats1, Is.GreaterThan(stats2));
        }

        [Test]
        public void ShouldConsiderHigherGoalDifferenceAsHigherPositionWhenPointsAreEqual()
        {
            var stats1 = new TeamStats(team1);
            var stats2 = new TeamStats(team2);
            
            stats1.ApplyResult(A.MatchResult.ForTeams(team1, A.Team.Build()).WithScore(1, 0).Build());
            stats2.ApplyResult(A.MatchResult.ForTeams(team2, A.Team.Build()).WithScore(2, 0).Build());

            Assert.That(stats1, Is.LessThan(stats2));
        }

        [Test]
        public void ShouldConsiderHigherGoalsScoredAsHigherPositionWhenPointsAndGoalDifferenceAreEqual()
        {
            var stats1 = new TeamStats(team1);
            var stats2 = new TeamStats(team2);

            stats1.ApplyResult(A.MatchResult.ForTeams(team1, A.Team.Build()).WithScore(1, 2).Build());
            stats2.ApplyResult(A.MatchResult.ForTeams(team2, A.Team.Build()).WithScore(2, 3).Build());

            Assert.That(stats1, Is.LessThan(stats2));
        }

        private MatchResult ResultForScore(int score1, int score2)
        {
            return A.MatchResult.ForTeams(team1, team2).WithScore(score1, score2).Build();
        }
    }
}
