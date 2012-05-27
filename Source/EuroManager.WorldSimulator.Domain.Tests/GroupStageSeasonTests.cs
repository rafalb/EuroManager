using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class WhenGroupStageScheduled : UnitTestFixture
    {
        private GroupStageSeason groupStageSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            groupStageSeason = A.GroupStageSeason.WithGroups(2, 4, hasReturnRound: true).Scheduled().Build();
        }
        
        [Test]
        public void ShouldScheduleRoundDates()
        {
            Assert.That(groupStageSeason.RoundDates.Count, Is.EqualTo(groupStageSeason.RoundCount));
        }
    }

    [TestFixture]
    public class WhenGroupStageActivated : UnitTestFixture
    {
        private GroupStageSeason groupStageSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            groupStageSeason = A.GroupStageSeason.WithGroups(4, 5, hasReturnRound: true).Scheduled().Activated().Build();
        }
        
        [Test]
        public void ShouldCreateGroups()
        {
            Assert.That(groupStageSeason.Groups.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldAssignAllTeamsToGroups()
        {
            Assert.That(groupStageSeason.Groups.SelectMany(g => g.Teams), Is.EquivalentTo(groupStageSeason.Teams));
        }

        [Test]
        public void ShouldAssignGroupNumbers()
        {
            Assert.That(groupStageSeason.Groups.Select(g => g.Number), Is.EquivalentTo(Enumerable.Range(1, 4)));
        }
    }

    [TestFixture]
    public class WhenGroupStageFixturesScheduled : UnitTestFixture
    {
        private GroupStageSeason groupStageSeason;
        private List<Fixture> fixtures;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            groupStageSeason = A.GroupStageSeason.WithGroups(6, 4, hasReturnRound: true).Scheduled().Activated().Build();

            fixtures = new List<Fixture>();
            groupStageSeason.ScheduleFixtures(f => fixtures.Add(f));
        }

        [Test]
        public void ShouldScheduleFixturesForAllMatches()
        {
            Assert.That(fixtures.Count, Is.EqualTo(72));
        }

        [Test]
        public void ShouldApplyResultToGroup()
        {
            var fixture = fixtures.ElementAt(9);
            groupStageSeason.ApplyResult(A.MatchResult.ForFixture(fixture).Build());

            var group = groupStageSeason.Groups.First(g => g.Teams.Contains(fixture.Team1));
            var teamStats = group.TeamStats.First(s => s.Team == fixture.Team1);

            Assert.That(teamStats.Played, Is.EqualTo(1));
        }
    }    

    [TestFixture]
    public class WhenAllGroupsFinished : UnitTestFixture
    {
        private GroupStageSeason groupStageSeason;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            groupStageSeason = A.GroupStageSeason.WithGroups(4, 4, hasReturnRound: true).Scheduled().Activated().Build();
            FixtureSetTestHelper.PlayAllFixtures(groupStageSeason);
        }

        [Test]
        public void ShouldFinishStage()
        {
            Assert.That(groupStageSeason.IsFinished, Is.True);
        }

        [Test]
        public void ShouldPromoteTeams()
        {
            Assert.That(groupStageSeason.PromotedTeams.Count, Is.EqualTo(4 * groupStageSeason.GroupPromotedCount));
        }
    }
}
