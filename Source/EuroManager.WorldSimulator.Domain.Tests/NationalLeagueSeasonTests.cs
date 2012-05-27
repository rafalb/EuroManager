using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.NationalLeagueSeasonTests
{
    [TestFixture]
    public class WhenAllNationalLeagueDivisionsFinished : UnitTestFixture
    {
        private NationalLeagueSeason season;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = A.NationalLeagueSeason.WithDivisions(3).Build();
            NationalLeagueSeasonTestHelper.PlayAllDivisionFixtures(season);
            season.AdvanceDate();
        }

        [Test]
        public void ShouldCreatePlayOffsSeason()
        {
            Assert.That(season.PlayOffs, Is.Not.Null);
        }

        [Test]
        public void ShouldCreatePlayOffPairsBetweenDivisions()
        {
            Assert.That(season.PlayOffs.Ties, Has.Count.EqualTo(2));
        }

        [Test]
        public void ShouldAppointLowerDivisionTeamAsFirstLegHost()
        {
            Team team1 = season.Divisions[1].PromotionPlayOffTeams.First();
            Team team2 = season.Divisions[0].RelegationPlayOffTeams.First();

            Assert.That(season.PlayOffs.Ties, Has.Exactly(1).Matches<Tie>(t => t.Team1 == team1 && t.Team2 == team2));
        }
    }

    [TestFixture]
    public class WhenNationalLeaguePlayOffsFinished : UnitTestFixture
    {
        private NationalLeagueSeason season;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = A.NationalLeagueSeason.WithDivisions(3).Build();
            
            NationalLeagueSeasonTestHelper.PlayAllDivisionFixtures(season);
            season.AdvanceDate();

            NationalLeagueSeasonTestHelper.PlayPlayOffsFixtures(season.PlayOffs);
            season.AdvanceDate();
        }

        [Test]
        public void ShouldPromoteWinnersFromLowerDivisions()
        {
            Tie tie = season.PlayOffs.Ties.First(t => DivisionOf(t.Winner).Level > DivisionOf(t.Loser).Level);
            Assert.That(DivisionOf(tie.Winner).PromotedTeams, Has.Exactly(1).EqualTo(tie.Winner));
        }

        [Test]
        public void ShouldNotPromoteLosersFromLowerDivisions()
        {
            Tie tie = season.PlayOffs.Ties.First(t => DivisionOf(t.Winner).Level < DivisionOf(t.Loser).Level);
            Assert.That(DivisionOf(tie.Loser).PromotedTeams, Has.None.EqualTo(tie.Loser));
        }

        [Test]
        public void ShouldRelegateLosersFromHigherDivisions()
        {
            Tie tie = season.PlayOffs.Ties.First(t => DivisionOf(t.Winner).Level > DivisionOf(t.Loser).Level);
            Assert.That(DivisionOf(tie.Loser).RelegatedTeams, Has.Exactly(1).EqualTo(tie.Loser));
        }

        [Test]
        public void ShouldNotRelegateWinnersFromHigherDivisions()
        {
            Tie tie = season.PlayOffs.Ties.First(t => DivisionOf(t.Winner).Level < DivisionOf(t.Loser).Level);
            Assert.That(DivisionOf(tie.Winner).RelegatedTeams, Has.None.EqualTo(tie.Loser));
        }

        [Test]
        public void ShouldNotPromoteWinnersFromHigherDivisions()
        {
            Tie tie = season.PlayOffs.Ties.First(t => DivisionOf(t.Winner).Level < DivisionOf(t.Loser).Level);
            Assert.That(DivisionOf(tie.Winner).PromotedTeams, Has.None.EqualTo(tie.Loser));
        }

        private TournamentSeason DivisionOf(Team team)
        {
            return season.Divisions.Single(d => d.Teams.Contains(team));
        }
    }

    [TestFixture]
    public class WhenAdvancingNationalLeagueSeason : UnitTestFixture
    {
        private NationalLeagueSeason season;
        private NationalLeagueSeason newSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = A.NationalLeagueSeason.WithDivisions(3).Build();

            NationalLeagueSeasonTestHelper.PlayAllDivisionFixtures(season);
            season.AdvanceDate();
            NationalLeagueSeasonTestHelper.PlayPlayOffsFixtures(season.PlayOffs);
            season.AdvanceDate();

            newSeason = (NationalLeagueSeason)season.AdvanceSeason();
        }

        [Test]
        public void ShouldDeactivateCurrentSeason()
        {
            Assert.That(season.IsActive, Is.False);
        }

        [Test]
        public void ShouldActivateNewSeason()
        {
            Assert.That(newSeason.IsActive, Is.True);
        }

        [Test]
        public void ShouldIncreaseStartYear()
        {
            Assert.That(newSeason.StartDate.Year, Is.EqualTo(season.StartDate.Year + 1));
        }

        [Test]
        public void ShouldIncreaseEndYear()
        {
            Assert.That(newSeason.EndDate.Year, Is.EqualTo(season.EndDate.Year + 1));
        }

        [Test]
        public void ShouldDeactivatePlayOffs()
        {
            Assert.That(season.PlayOffs.IsActive, Is.False);
        }

        [Test]
        public void ShouldCreateRelevantDivisionSeasons()
        {
            Assert.That(newSeason.Divisions.Select(d => d.GetType()), Is.EquivalentTo(season.Divisions.Select(d => d.GetType())));
        }

        [Test]
        public void ShouldIncludePromotedTeamsInHigherDivision()
        {
            TournamentSeason division = season.Divisions.Last(d => d.PromotedTeams.Any());
            TournamentSeason upperDivision = newSeason.Divisions.Single(d => d.Level == division.Level - 1);

            Assert.That(division.PromotedTeams, Is.SubsetOf(upperDivision.Teams));
        }

        [Test]
        public void ShouldNotIncludePromotedTeamsInSameDivision()
        {
            TournamentSeason division = season.Divisions.Last(d => d.PromotedTeams.Any());
            TournamentSeason newDivision = newSeason.Divisions.Single(d => d.Level == division.Level);

            Assert.That(division.PromotedTeams, Has.None.Matches<Team>(newDivision.Teams.Contains));
        }

        [Test]
        public void ShouldIncludeRelegatedTeamsInLowerDivision()
        {
            TournamentSeason division = season.Divisions.First(d => d.RelegatedTeams.Any());
            TournamentSeason lowerDivision = newSeason.Divisions.Single(d => d.Level == division.Level + 1);

            Assert.That(division.RelegatedTeams, Is.SubsetOf(lowerDivision.Teams));
        }

        [Test]
        public void ShouldNotIncludeRelegatedTeamsInSameDivision()
        {
            TournamentSeason division = season.Divisions.First(d => d.RelegatedTeams.Any());
            TournamentSeason newDivision = newSeason.Divisions.Single(d => d.Level == division.Level);

            Assert.That(division.RelegatedTeams, Has.None.Matches<Team>(newDivision.Teams.Contains));
        }
    }

    public static class NationalLeagueSeasonTestHelper
    {
        public static void PlayAllDivisionFixtures(NationalLeagueSeason season)
        {
            foreach (var division in season.Divisions)
            {
                FixtureSetTestHelper.PlayAllFixtures(division);
            }
        }

        public static void PlayPlayOffsFixtures(LeaguePlayOffsSeason playOffs)
        {
            var fixtures = new List<Fixture>();
            playOffs.ScheduleFixtures(fixtures.Add);
            fixtures = fixtures.OrderBy(f => f.Date).ToList();

            playOffs.ApplyResult(A.MatchResult.ForFixture(fixtures[0]).Build());
            playOffs.ApplyResult(A.MatchResult.ForFixture(fixtures[1]).Build());
            playOffs.ApplyResult(A.MatchResult.ForFixture(fixtures[2]).WonBy(fixtures[2].Team2).Build());
            playOffs.ApplyResult(A.MatchResult.ForFixture(fixtures[3]).WonBy(fixtures[3].Team1).Build());
        }
    }
}
