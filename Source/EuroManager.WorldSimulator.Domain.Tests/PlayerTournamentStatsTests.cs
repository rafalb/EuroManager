using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class PlayerTournamentStatsTests : UnitTestFixture
    {
        private PlayerTournamentStats stats;
        private Player player;
        private Team team;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            TournamentSeason season = A.LeagueSeason.Build();

            player = new Player(season.World, "Test", 75, 75, 75);
            team = A.Team.WithPlayers(player).Build();

            stats = new PlayerTournamentStats(season, player);

            stats.ApplyResult(A.MatchResult.ForTeams(team, A.Team.Build())
                .WithTeam1PlayersStats(new PlayerMatchStats(player, rating: 7, goals: 2, assists: 1)).Build());
            stats.ApplyResult(A.MatchResult.ForTeams(A.Team.Build(), team)
                .WithTeam2PlayersStats(new PlayerMatchStats(player, rating: 4, goals: 1, assists: 3)).Build());
        }

        [Test]
        public void ShouldCountMatchesPlayed()
        {
            Assert.That(stats.Played, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountGoalsScored()
        {
            Assert.That(stats.Goals, Is.EqualTo(3));
        }

        [Test]
        public void ShouldCountAssists()
        {
            Assert.That(stats.Assists, Is.EqualTo(4));
        }

        [Test]
        public void ShouldCalculateAverageRating()
        {
            Assert.That(stats.AverageRating, Is.EqualTo(5.5));
        }
    }

    [TestFixture]
    public class PlayerTournamentStatsCombiningTests : UnitTestFixture
    {
        private PlayerTournamentStats stats1;
        private PlayerTournamentStats stats2;
        private PlayerTournamentStats combinedStats;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            TournamentSeason tournament1 = A.LeagueSeason.Build();
            TournamentSeason tournament2 = A.LeagueSeason.Build();

            var player = new Player(tournament1.World, "Test", 75, 75, 75);
            var team = A.Team.WithPlayers(player).Build();

            var stats1 = new PlayerTournamentStats(tournament1, player);
            var stats2 = new PlayerTournamentStats(tournament2, player);

            stats1.ApplyResult(A.MatchResult.ForTeams(team, A.Team.Build())
                .WithTeam1PlayersStats(new PlayerMatchStats(player, rating: 7, goals: 2, assists: 1)).Build());
            stats1.ApplyResult(A.MatchResult.ForTeams(A.Team.Build(), team)
                .WithTeam2PlayersStats(new PlayerMatchStats(player, rating: 4, goals: 1, assists: 3)).Build());

            stats2.ApplyResult(A.MatchResult.ForTeams(team, A.Team.Build())
                .WithTeam1PlayersStats(new PlayerMatchStats(player, rating: 7, goals: 1, assists: 2)).Build());

            combinedStats = PlayerTournamentStats.Combine(stats1, stats2);
        }

        [Test]
        public void ShouldCombineMatchesPlayed()
        {
            Assert.That(combinedStats.Played, Is.EqualTo(3));
        }

        [Test]
        public void ShouldCombineGoalCount()
        {
            Assert.That(combinedStats.Goals, Is.EqualTo(4));
        }

        [Test]
        public void ShouldCombineAssistCount()
        {
            Assert.That(combinedStats.Assists, Is.EqualTo(6));
        }

        [Test]
        public void ShouldCombineAverageRating()
        {
            Assert.That(combinedStats.AverageRating, Is.EqualTo(6.0));
        }
    }
}
