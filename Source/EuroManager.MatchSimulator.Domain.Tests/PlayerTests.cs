using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Tests;
using Moq;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests.PlayerTests
{
    [TestFixture]
    public class WhenPlayerCreated : UnitTestFixture
    {
        private Player player;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            player = A.Player.Build();
        }

        [Test]
        public void ShouldReceiveInitialRating()
        {
            Assert.That(player.Rating, Is.EqualTo(Player.InitialRating));
        }

        [Test]
        public void ShouldAllowToIncreaseRating()
        {
            double originalRating = player.Rating;
            player.AdjustRating(0.05);

            Assert.That(player.Rating, Is.EqualTo(originalRating + 0.05));
        }

        [Test]
        public void ShouldAllowToDecreaseRating()
        {
            double originalRating = player.Rating;
            player.AdjustRating(-0.1);

            Assert.That(player.Rating, Is.EqualTo(originalRating - 0.1));
        }

        [Test]
        public void ShouldNotIncreaseRatingBeyondMax()
        {
            player.AdjustRating(1.0);

            Assert.That(player.Rating, Is.EqualTo(1.0));
        }

        [Test]
        public void ShouldNotDecreaseRatingBelowMin()
        {
            player.AdjustRating(-1.0);

            Assert.That(player.Rating, Is.EqualTo(0.0));
        }

        [Test]
        public void ShouldRoundFinalRating()
        {
            player.AdjustRating(-0.03);
            player.AdjustRating(0.22);

            Assert.That(player.FinalRating, Is.EqualTo(8));
        }

        [Test]
        public void ShouldNotGiveFinalRatingBelowMin()
        {
            player.AdjustRating(-0.7);

            Assert.That(player.FinalRating, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class WhenPlayerScoresGoals : UnitTestFixture
    {
        private Player player;
        private Player assistant;
        private Player opponent;
        private Player goalkeeper;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);
            var randomizerStub = mocks.Create<IMatchRandomizer>();
            randomizerStub.Setup(r => r.TryShoot(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(ShotResult.Scored);
            MatchRandomizer.Current = randomizerStub.Object;

            player = A.Player.Build();
            assistant = A.Player.Build();
            opponent = A.Player.Build();
            goalkeeper = A.Player.Build();
            
            player.TryShoot(assistant, opponent, goalkeeper);
            player.TryShoot(assistant, opponent, goalkeeper);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldCountGoals()
        {
            Assert.That(player.Goals, Is.EqualTo(2));
        }
        
        [Test]
        public void ShouldCountShotsOnTarget()
        {
            Assert.That(player.ShotsOnTarget, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountAssists()
        {
            Assert.That(assistant.Assists, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCountShotsAllowed()
        {
            Assert.That(opponent.ShotsAllowed, Is.EqualTo(2));
        }

        [Test]
        public void ShouldCalculateGoalkeeperNotSavedShots()
        {
            Assert.That(goalkeeper.ShotsNotSaved, Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class WhenPlayerStopsDribbler : UnitTestFixture
    {
        private Player player;
        private Player dribbler;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var mocks = new MockRepository(MockBehavior.Loose);
            var randomizerStub = mocks.Create<IMatchRandomizer>();
            randomizerStub.Setup(r => r.TryDribble(It.IsAny<double>(), It.IsAny<double>())).Returns(false);
            MatchRandomizer.Current = randomizerStub.Object;

            player = A.Player.Build();
            dribbler = A.Player.Build();

            dribbler.TryDribble(player);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MatchRandomizer.ResetCurrent();
        }

        [Test]
        public void ShouldCountTacklesCompleted()
        {
            Assert.That(player.TacklesCompleted, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCountDribblesFailed()
        {
            Assert.That(dribbler.DribblesFailed, Is.EqualTo(1));
        }
    }
}
