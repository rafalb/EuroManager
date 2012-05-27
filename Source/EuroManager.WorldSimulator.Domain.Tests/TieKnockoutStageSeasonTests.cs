using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.TieKnockoutStageSeasonTests
{
    [TestFixture]
    public class WhenSchedulingTieKnockoutStageSeasonRoundDates : UnitTestFixture
    {
        private TieKnockoutStageSeason season;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            season = new TieKnockoutStageSeason(new TieKnockoutStage(4));
            season.ScheduleRoundDates(new DateTime[] { new DateTime(2012, 05, 22), new DateTime(2012, 06, 07) });
        }

        [Test]
        public void ShouldScheduleFirstLegDate()
        {
            Assert.That(season.FirstLegDate, Is.EqualTo(new DateTime(2012, 05, 22)));
        }

        [Test]
        public void ShouldScheduleSecondLegDate()
        {
            Assert.That(season.SecondLegDate, Is.EqualTo(new DateTime(2012, 06, 07)));
        }
    }

    [TestFixture]
    public class WhenActivatingTieKnockoutStageSeason : UnitTestFixture
    {
        private TieKnockoutStageSeason season;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = new TieKnockoutStageSeason(new TieKnockoutStage(4));
            season.CupSeason = A.CupSeason.Build();

            season.ScheduleRoundDates(new DateTime[] { new DateTime(2012, 05, 22), new DateTime(2012, 06, 07) });
            season.Activate(A.Team.Repeat(8));
        }

        [Test]
        public void ShouldCreateTies()
        {
            Assert.That(season.Ties, Has.Count.EqualTo(4));
        }

        [Test]
        public void ShouldIncludeAllTeamsInTies()
        {
            Assert.That(season.Teams, Has.All.Matches<Team>(t =>
                season.Ties.Count(tie => tie.Team1 == t || tie.Team2 == t) == 1));
        }

        [Test]
        public void ShouldScheduleTieMatchDates()
        {
            Assert.That(season.Ties, Has.All.Matches<Tie>(t =>
                t.FirstLegDate == new DateTime(2012, 05, 22) &&
                t.SecondLegDate == new DateTime(2012, 06, 07)));
        }

        [Test]
        public void ShouldScheduleFixtures()
        {
            var fixtures = new List<Fixture>();
            season.ScheduleFixtures(f => fixtures.Add(f));

            Assert.That(fixtures, Has.Count.EqualTo(8));
        }
    }

    [TestFixture]
    public class WhenScheduledTieKnockoutStageSeason : UnitTestFixture
    {
        private TieKnockoutStageSeason season;
        private List<Fixture> fixtures = new List<Fixture>();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            season = new TieKnockoutStageSeason(new TieKnockoutStage(4));
            season.CupSeason = A.CupSeason.Build();

            season.ScheduleRoundDates(new DateTime[] { new DateTime(2012, 05, 22), new DateTime(2012, 06, 07) });
            season.Activate(A.Team.Repeat(8));
            season.ScheduleFixtures(f => fixtures.Add(f));
        }

        [Test]
        public void ShouldApplyResult()
        {
            season.ApplyResult(A.MatchResult.ForFixture(fixtures[0]).Build());

            Assert.That(season.Ties.First().FirstLegResult, Is.Not.Null);
        }

        [Test]
        public void ShouldPromoteWinningTeam()
        {
            Tie tie = season.Ties[2];
            season.ApplyResult(A.MatchResult.ForFixture(fixtures.Single(f => f.Team1 == tie.Team1)).WithScore(0, 0).Build());
            season.ApplyResult(A.MatchResult.ForFixture(fixtures.Single(f => f.Team1 == tie.Team2)).WithScore(1, 0).Build());

            Assert.That(season.PromotedTeams, Has.Exactly(1).EqualTo(tie.Team2));
        }

        [Test]
        public void ShouldNotFinishUntilAllTiesComplete()
        {
            season.Ties.Skip(1).ToList().ForEach(t =>
                {
                    season.ApplyResult(A.MatchResult.ForFixture(fixtures.First(f => f.Team1 == t.Team1)).WithScore(0, 0).Build());
                    season.ApplyResult(A.MatchResult.ForFixture(fixtures.First(f => f.Team1 == t.Team2)).WithScore(1, 0).Build());
                });

            Assert.That(season.IsFinished, Is.False);
        }

        [Test]
        public void ShouldFinishAfterAllTiesComplete()
        {
            season.Ties.ForEach(t =>
            {
                season.ApplyResult(A.MatchResult.ForFixture(fixtures.First(f => f.Team1 == t.Team1)).WithScore(0, 0).Build());
                season.ApplyResult(A.MatchResult.ForFixture(fixtures.First(f => f.Team1 == t.Team2)).WithScore(1, 0).Build());
            });

            Assert.That(season.IsFinished, Is.True);
        }
    }
}
