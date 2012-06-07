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
                .WithTeam1PlayersStats(new PlayerMatchStats(player, 7)).Build());
            stats.ApplyResult(A.MatchResult.ForTeams(A.Team.Build(), team)
                .WithTeam2PlayersStats(new PlayerMatchStats(player, 4)).Build());
        }

        [Test]
        public void ShouldCountMatchesPlayed()
        {
            Assert.That(stats.Played, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCalculateAverageRating()
        {
            Assert.That(stats.AverageRating, Is.EqualTo(5.5));
        }
    }
}
