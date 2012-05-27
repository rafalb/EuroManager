using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.CupSeasonTests
{
    [TestFixture]
    public class WhenCupSeasonCreated : UnitTestFixture
    {
        private CupSeason cupSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            cupSeason = A.CupSeason.Build();
        }

        [Test]
        public void ShouldAssignStages()
        {
            Assert.That(cupSeason.Stages, Has.All.Matches<CupStageSeason>(s => s.CupSeason == cupSeason));
        }

        [Test]
        public void ShouldScheduleRoundDatesInAdvance()
        {
            var groupStage = cupSeason.Stages.OfType<GroupStageSeason>().First();
            Assert.That(groupStage.RoundDates.Count, Is.EqualTo(groupStage.RoundCount));
        }

        [Test]
        public void ShouldActivateFirstStage()
        {
            Assert.That(cupSeason.CurrentStage, Is.EqualTo(cupSeason.Stages.First()));
        }

        [Test]
        public void ShouldScheduleCurrentStageFixtures()
        {    
            var fixtures = new List<Fixture>();
            cupSeason.ScheduleFixtures(f => fixtures.Add(f));

            Assert.That(fixtures, Is.Not.Empty);
        }
    }

    [TestFixture]
    public class WhenAllStageFixturesPlayed : UnitTestFixture
    {
        private CupSeason cupSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            cupSeason = A.CupSeason.Build();
            FixtureSetTestHelper.PlayAllFixtures(cupSeason);
        }

        [Test]
        public void ShouldAdvanceStage()
        {
            Assert.That(cupSeason.CurrentStage, Is.EqualTo(cupSeason.Stages.ElementAt(1)));
        }

        [Test]
        public void ShouldRegisterForSchedulingNextStage()
        {
            Assert.That(cupSeason.NextSchedulingDate, Is.LessThanOrEqualTo(cupSeason.World.Date));
        }
    }

    [TestFixture]
    public class WhenAllStagesFinished : UnitTestFixture
    {
        private CupSeason cupSeason;
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            cupSeason = A.CupSeason.Build();
            CupSeasonTestHelper.PlayAllStages(cupSeason);
        }

        [Test]
        public void ShouldFinishSeason()
        {
            Assert.That(cupSeason.IsFinished, Is.True);
        }

        [Test]
        public void ShouldPromoteWinners()
        {
            var lastStage = cupSeason.Stages.Last();
            Assert.That(cupSeason.PromotedTeams, Is.EquivalentTo(lastStage.PromotedTeams));
        }

        [Test]
        public void ShouldPromoteRunnersUpToPlayOffs()
        {
            var lastStage = cupSeason.Stages.Last();
            Assert.That(cupSeason.PromotionPlayOffTeams, Is.EquivalentTo(lastStage.Teams.Except(lastStage.PromotedTeams)));
        }
    }

    [TestFixture]
    public class WhenAdvancingCupSeason : UnitTestFixture
    {
        private CupSeason cupSeason;
        private CupSeason newSeason;
        private IEnumerable<Team> promotedFromLower;
        private IEnumerable<Team> relegatedFromHigher;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            cupSeason = A.CupSeason.Build();
            
            cupSeason.PromoteTeam(cupSeason.Teams.ElementAt(0));
            cupSeason.RelegateTeam(cupSeason.Teams.ElementAt(1));
            cupSeason.RelegateTeam(cupSeason.Teams.ElementAt(4));
            cupSeason.PromoteTeam(cupSeason.Teams.ElementAt(5));

            promotedFromLower = new Team[] { A.Team.Build(), A.Team.Build() };
            relegatedFromHigher = new Team[] { A.Team.Build(), A.Team.Build() };

            newSeason = (CupSeason)cupSeason.AdvanceSeason(relegatedFromHigher, promotedFromLower);
        }

        [Test]
        public void ShouldDeactivateCurrentSeason()
        {
            Assert.That(cupSeason.IsActive, Is.False);
        }

        [Test]
        public void ShouldAdvanceStartDate()
        {
            Assert.That(newSeason.StartDate.Year, Is.EqualTo(cupSeason.StartDate.Year + 1));
        }

        [Test]
        public void ShouldCreateEquivalentStageSeasons()
        {
            Assert.That(newSeason.Stages.Select(s => s.GetType()), Is.EqualTo(cupSeason.Stages.Select(s => s.GetType())));
        }

        [Test]
        public void ShouldIncludePromotedAndRelegatedTeams()
        {
            var expected = Enumerable
                .Union(cupSeason.Teams.Skip(2).Take(2), cupSeason.Teams.Skip(6))
                .Union(promotedFromLower)
                .Union(relegatedFromHigher)
                .ToArray();

            Assert.That(newSeason.Teams, Is.EquivalentTo(expected));
        }
    }

    public static class CupSeasonTestHelper
    {
        public static void PlayAllStages(CupSeason season)
        {
            for (int i = 0; i < season.Stages.Count; i++)
            {
                FixtureSetTestHelper.PlayAllFixtures(season);
            }
        }
    }
}
