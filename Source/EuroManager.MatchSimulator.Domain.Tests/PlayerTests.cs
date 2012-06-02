using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
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
    }
}
