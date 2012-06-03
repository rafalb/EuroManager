using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Domain;
using EuroManager.Common.Tests;
using Moq;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests
{
    [TestFixture]
    public class SimulatorTests : UnitTestFixture
    {
        private MockRepository mocks;
        private IMatchRandomizer randomizer;

        [SetUp]
        public override void SetUp()
        {
            randomizer = new MatchRandomizer(new StaticRandomGenerator());
            mocks = new MockRepository(MockBehavior.Default);

            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            MatchRandomizer.ResetCurrent();

            base.TearDown();
        }

        [Test]
        public void ShouldGenerateMatchResult()
        {
            var match = A.Match.Build();
            var simulator = new Simulator(randomizer);
            var result = simulator.Play(match);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ShouldFinishWithInactiveMatch()
        {
            var match = A.Match.Build();
            var simulator = new Simulator(randomizer);
            simulator.Play(match);

            Assert.That(match.IsActive, Is.False);
        }

        [Test]
        public void ShouldFinishWithConclusiveMatch()
        {
            var match = A.Match.Build();
            var simulator = new Simulator(randomizer);
            simulator.Play(match);

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldPlayExtraTimeWhenInconclusive()
        {
            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Missed);
            MatchRandomizer.Current = randomizerStub.Object;

            var match = A.Match.WithExtraTimeRequired().Build();
            var simulator = new Simulator(randomizerStub.Object);
            simulator.Play(match);

            Assert.That(match.Length, Is.EqualTo(120));
        }

        [Test]
        public void ShouldPlayPenaltyShootoutWhenInconclusive()
        {
            var randomizerStub = mocks.Create<MatchRandomizer>(MockBehavior.Loose, new StaticRandomGenerator());
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Missed);
            randomizerStub.Setup(r => r.IsFirstTeamStartingPenaltyShootout()).Returns(true);
            
            bool toggle = false;
            randomizerStub.Setup(r => r.TryPenaltyKick()).Returns(() => { toggle = !toggle; return toggle; });

            MatchRandomizer.Current = randomizerStub.Object;

            var match = A.Match.WithExtraTimeRequired().Build();
            var simulator = new Simulator(randomizerStub.Object);
            simulator.Play(match);

            Assert.That(match.PenaltyScore1, Is.GreaterThan(0));
        }
    }
}
