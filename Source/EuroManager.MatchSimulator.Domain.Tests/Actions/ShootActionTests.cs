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
    public class ShootActionTests : UnitTestFixture
    {
        private MockRepository mocks;
        private Mock<MatchRandomizer> randomizerMock;

        [SetUp]
        public override void SetUp()
        {
            mocks = new MockRepository(MockBehavior.Loose);
            randomizerMock = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());

            MatchRandomizer.Current = randomizerMock.Object;

            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            MatchRandomizer.ResetCurrent();

            base.TearDown();
        }

        [Test]
        public void ShouldNotContinueOnFail()
        {
            randomizerMock.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            var action = new ShootAction();
            action.Perform(match);

            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldNotContinueOnSuccess()
        {
            randomizerMock.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            var action = new ShootAction();
            action.Perform(match);

            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldScoreOnSuccess()
        {
            randomizerMock.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team2.Squad.ElementAt(5));

            var action = new ShootAction();
            action.Perform(match);

            Assert.That(match.Score2, Is.EqualTo(1));
        }
    }
}
