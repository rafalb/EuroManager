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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);
            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Scored);
            MatchRandomizer.Current = randomizerStub.Object;

            match = A.Match.Build();
            match.InitiateAttack(match.Team2.Squad.ElementAt(5));

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
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Saved);
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
    }
}
