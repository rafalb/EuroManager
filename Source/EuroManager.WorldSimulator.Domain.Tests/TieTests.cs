using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.TieTests
{
    [TestFixture]
    public class WhenTieCreatedAndScheduled : UnitTestFixture
    {
        private Tie tie;
        private Fixture[] fixtures;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            tie = new Tie(A.Date, A.Date.AddDays(7), A.Team.Build(), A.Team.Build());
            fixtures = tie.CreateFixtures(A.CupSeason.Build());
        }

        [Test]
        public void ShouldCreateFixtureWithFirstLegDate()
        {
            Assert.That(fixtures[0].Date, Is.EqualTo(tie.FirstLegDate));
        }

        [Test]
        public void ShouldCreateFixtureWithSecondLegDate()
        {
            Assert.That(fixtures[1].Date, Is.EqualTo(tie.SecondLegDate));
        }

        [Test]
        public void ShouldCreateFirstLegFixtureWithProperTeams()
        {
            Assert.That(fixtures[0].Team1, Is.EqualTo(tie.Team1));
        }

        [Test]
        public void ShouldCreateSecondLegFixtureWithProperTeams()
        {
            Assert.That(fixtures[1].Team1, Is.EqualTo(tie.Team2));
        }

        [Test]
        public void ShouldMatchFirstLegResult()
        {
            MatchResult firstLegResult = A.MatchResult.ForFixture(fixtures[0]).Build();
            Assert.That(tie.IsMatchingResult(firstLegResult), Is.True);
        }

        [Test]
        public void ShouldMatchSecondLegResult()
        {
            tie.AddResult(A.MatchResult.ForFixture(fixtures[0]).WithScore(1, 2).Build());
            MatchResult secondLegResult = A.MatchResult.ForFixture(fixtures[1]).Build();

            Assert.That(tie.IsMatchingResult(secondLegResult), Is.True);
        }
    }

    [TestFixture]
    public class WhenTieMatchesPlayed : UnitTestFixture
    {
        private Tie tie;
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            tie = new Tie(A.Date, A.Date.AddDays(7), A.Team.Build(), A.Team.Build());

            var fixtures = tie.CreateFixtures(A.CupSeason.Build());
            tie.AddResult(A.MatchResult.ForFixture(fixtures[0]).WithScore(1, 2).Build());
            tie.AddResult(A.MatchResult.ForFixture(fixtures[1]).WithScore(0, 1).Build());
        }

        [Test]
        public void ShouldSelectWinner()
        {
            Assert.That(tie.Winner, Is.EqualTo(tie.Team2));
        }

        [Test]
        public void ShouldSelectLoser()
        {
            Assert.That(tie.Loser, Is.EqualTo(tie.Team1));
        }
    }
}
