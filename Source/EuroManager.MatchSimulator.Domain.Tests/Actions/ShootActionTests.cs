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
    public class WhenShootActionSucceeded : UnitTestFixture
    {
        private ShootAction action;
        private Match match;
        private double previousShooterRating;
        private double previousAssistantRating;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);
            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Scored);
            MatchRandomizer.Current = randomizerStub.Object;

            match = A.Match.Build();
            match.InitiateAttack(match.Team2.Squad.ElementAt(5));

            var pass = new PassAction();
            pass.Perform(match);

            previousShooterRating = match.CurrentPlayer.Rating;
            previousAssistantRating = match.PreviousPlayer.Rating;

            action = new ShootAction();
            action.Perform(match);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldNotContinue()
        {
            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldIncreaseScore()
        {
            Assert.That(match.Score2, Is.EqualTo(1));
        }

        [Test]
        public void ShouldIncreaseShooterRating()
        {
            Assert.That(action.Shooter.Rating, Is.GreaterThan(previousShooterRating));
        }

        [Test]
        public void ShouldIncreaseAssistantRating()
        {
            Assert.That(action.Assistant.Rating, Is.GreaterThan(previousAssistantRating));
        }

        [Test]
        public void ShouldDecreaseOpponentRating()
        {
            Assert.That(action.Opponent.Rating, Is.LessThan(Player.InitialRating));
        }

        [Test]
        public void ShouldDecreaseGoalkeeperRating()
        {
            Assert.That(action.Goalkeeper.Rating, Is.LessThan(Player.InitialRating));
        }
    }

    [TestFixture]
    public class WhenShootActionFailed : UnitTestFixture
    {
        private ShootAction action;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);
            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Missed);
            MatchRandomizer.Current = randomizerStub.Object;

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            action = new ShootAction();
            action.Perform(match);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldNotContinueOnFail()
        {
            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldDecreaseShooterRating()
        {
            Assert.That(action.Shooter.Rating, Is.LessThan(Player.InitialRating));
        }
    }
}
