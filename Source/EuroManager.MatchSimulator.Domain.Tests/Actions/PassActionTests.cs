using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using EuroManager.MatchSimulator.Domain.Actions;
using Moq;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests.Actions
{
    [TestFixture]
    public class WhenPassActionSucceeded : UnitTestFixture
    {
        private PassAction action;
        private Match match;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);

            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            MatchRandomizer.Current = randomizerStub.Object;

            match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            action = new PassAction();
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
        public void ShouldSwitchCurrentPlayerToPassReceiver()
        {
            Assert.That(match.CurrentPlayer, Is.EqualTo(action.Receiver));
        }

        [Test]
        public void ShouldIncreasePassingPlayerRating()
        {
            Assert.That(action.PassingPlayer.Rating, Is.GreaterThan(Player.InitialRating));
        }

        [Test]
        public void ShouldIncreasePassReceiverRating()
        {
            Assert.That(action.Receiver.Rating, Is.GreaterThan(Player.InitialRating));
        }

        [Test]
        public void ShouldDecreaseOpponentRating()
        {
            Assert.That(action.Opponent.Rating, Is.LessThan(Player.InitialRating));
        }
    }

    [TestFixture]
    public class WhenPassActionFailed : UnitTestFixture
    {
        private PassAction action;
        private Match match;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);

            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);
            MatchRandomizer.Current = randomizerStub.Object;

            match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            action = new PassAction();
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
        public void ShouldDecreasePassingPlayerRating()
        {
            Assert.That(action.PassingPlayer.Rating, Is.LessThan(Player.InitialRating));
        }

        [Test]
        public void ShouldDecreasePassReceiverRating()
        {
            Assert.That(action.Receiver.Rating, Is.LessThan(Player.InitialRating));
        }

        [Test]
        public void ShouldIncreaseOpponentRating()
        {
            Assert.That(action.Opponent.Rating, Is.GreaterThan(Player.InitialRating));
        }
    }
}
