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
    public class PassActionTests : UnitTestFixture
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
            randomizerMock.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            var action = new PassAction();
            action.Perform(match);

            Assert.That(action.CanContinue, Is.False);
        }

        [Test]
        public void ShouldContinueOnSuccess()
        {
            randomizerMock.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            Match match = A.Match.Build();
            match.InitiateAttack(match.Team1.Squad.ElementAt(5));

            var action = new PassAction();
            action.Perform(match);

            Assert.That(action.CanContinue, Is.True);
        }

        [Test]
        public void ShouldSwitchCurrentPlayerOnSuccess()
        {
            randomizerMock.Setup(r => r.TryPass(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            Match match = A.Match.Build();
            var player = match.Team1.Squad.ElementAt(5);
            match.InitiateAttack(player);

            var action = new PassAction();
            action.Perform(match);

            Assert.That(match.CurrentPlayer, Is.Not.EqualTo(player));
        }
    }
}
