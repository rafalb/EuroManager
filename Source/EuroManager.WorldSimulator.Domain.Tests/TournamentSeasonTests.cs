using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.TournamentSeasonTests
{
    [TestFixture]
    public class WhenTournamentSeasonCreated : UnitTestFixture
    {
        private TournamentSeason season;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = A.CupSeason.Build();
        }

        [Test]
        public void ShouldCreateTournamentStatsForEachTeam()
        {
            var teamsWithStats = season.TeamTournamentStats.Select(s => s.Team).ToArray();
            Assert.That(teamsWithStats, Is.EquivalentTo(season.Teams));
        }
    }

    [TestFixture]
    public class WhenTournamentSeasonMatchPlayed : UnitTestFixture
    {
        private TournamentSeason season;
        private Team team;
        private TeamTournamentStats teamStats;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = A.LeagueSeason.Build();

            team = season.Teams.ElementAt(3);
            teamStats = season.TeamTournamentStats.First(s => s.Team == team);
            season.ApplyResult(A.MatchResult.ForTeams(team, season.Teams.ElementAt(2)).Build());
        }

        [Test]
        public void ShouldApplyResultToTeamTournamentStats()
        {
            Assert.That(teamStats.Played, Is.EqualTo(1));
        }
    }
}
