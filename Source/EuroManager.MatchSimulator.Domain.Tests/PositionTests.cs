using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests
{
    [TestFixture]
    public class PositionTests : UnitTestFixture
    {
        [Test]
        public void ShouldCreatePositionFromCode()
        {
            Position position = Position.FromCode(PositionCode.RCM);

            Assert.That(position, Is.EqualTo(Position.RightCenterMidfielder));
        }

        [Test]
        public void ShouldCalculateDistanceForward()
        {
            Position position1 = Position.FromCode(PositionCode.RB);
            Position position2 = Position.FromCode(PositionCode.LF);

            Assert.That(position1.DistanceForward(position2), Is.EqualTo(4));
        }

        [Test]
        public void ShouldCalculateDistanceBackward()
        {
            Position position1 = Position.FromCode(PositionCode.ST);
            Position position2 = Position.FromCode(PositionCode.LCM);

            Assert.That(position1.DistanceForward(position2), Is.EqualTo(-2));
        }

        [Test]
        public void ShouldCalculateDistanceSideways()
        {
            Position position1 = Position.FromCode(PositionCode.RCB);
            Position position2 = Position.FromCode(PositionCode.LF);

            Assert.That(position1.DistanceSideways(position2), Is.EqualTo(2));
        }

        [Test]
        public void ShouldCalculateOppositeLocation()
        {
            Position position = Position.FromCode(PositionCode.RF).Opposite();

            Assert.That(position.Location, Is.EqualTo(Location.Back));
        }

        [Test]
        public void ShouldCalculateOppositeSide()
        {
            Position position = Position.FromCode(PositionCode.RF).Opposite();

            Assert.That(position.Side, Is.EqualTo(Side.LeftCenter));
        }
    }
}
