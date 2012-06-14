using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.TeamTournamentStatsTests
{
    [TestFixture]
    public class WhenTeamTournamentStatsCreated : UnitTestFixture
    {
        private Team team;
        private TeamTournamentStats stats;
        private Player player;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            team = A.Team.WithPlayers(A.Player.Repeat(20).ToArray()).Build();
            stats = new TeamTournamentStats(A.LeagueSeason.Build(), team);

            player = team.Players.ElementAt(6);
            stats.ApplyResult(A.MatchResult
                .ForTeams(team, A.Team.Build())
                .WithTeam1PlayersStats(new PlayerMatchStats(player, rating: 4, goals: 0, assists: 0))
                .Build());
        }

        [Test]
        public void ShouldCreateStatsForTeamPlayers()
        {
            var playersWithStats = stats.PlayerStats.Select(s => s.Player).ToArray();
            Assert.That(playersWithStats, Is.EquivalentTo(team.Players));
        }

        [Test]
        public void ShouldApplyResultToPlayerStats()
        {
            PlayerTournamentStats playerStats = stats.PlayerStats.First(s => s.Player == player);
            Assert.That(playerStats.Played, Is.EqualTo(1));
        }
    }
}
