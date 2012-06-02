using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Tests;
using EuroManager.MatchSimulator.Domain.Actions;
using Moq;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests.Actions.DribbleActionTests
{
    [TestFixture]
    public class WhenDribbleActionSucceeded : UnitTestFixture
    {
        private DribbleAction action;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);

            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryDribble(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            MatchRandomizer.Current = randomizerStub.Object;
            
            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(3));

            action = new DribbleAction();
            action.Perform(match);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldContinueAttack()
        {
            Assert.That(action.CanContinue, Is.True);
        }

        [Test]
        public void ShouldIncreaseDribblerRating()
        {
            Assert.That(action.Dribbler.Rating, Is.GreaterThan(Player.InitialRating));
        }

        [Test]
        public void ShouldDecreaseOpponentRating()
        {
            Assert.That(action.Opponent.Rating, Is.LessThan(Player.InitialRating));
        }
    }

    [TestFixture]
    public class WhenDribbleActoinFailed : UnitTestFixture
    {
        private DribbleAction action;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);

            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryDribble(It.IsAny<double>(), It.IsAny<double>())).Returns(false);
            MatchRandomizer.Current = randomizerStub.Object;

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(3));

            action = new DribbleAction();
            action.Perform(match);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldNotContinueAttack()
        {
            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldDecreaseDribblerRating()
        {
            Assert.That(action.Dribbler.Rating, Is.LessThan(Player.InitialRating));
        }

        [Test]
        public void ShouldIncreaseOpponentRating()
        {
            Assert.That(action.Opponent.Rating, Is.GreaterThan(Player.InitialRating));
        }
    }
}
