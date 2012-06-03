using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EuroManager.Common.Tests
{
    [TestFixture]
    public class MathHelperTests : UnitTestFixture
    {
        [Test]
        public void ShouldCalculatePositiveLinearAdjustment()
        {
            double adjustment = MathHelper.LinearAdjustment(1.5, 0.5, 0.3);
            Assert.That(adjustment, Is.EqualTo(1.3));
        }

        [Test]
        public void ShouldCalculateNegativeLinearAdjustment()
        {
            double adjustment = MathHelper.LinearAdjustment(-0.15, 0.05, 0.7);
            Assert.That(adjustment, Is.EqualTo(-0.13));
        }

        [Test]
        public void ShouldCalculatePositiveExponentialAdjustment()
        {
            double adjustment = MathHelper.ExponentialAdjustment(0.5, 0.2, 0.75);
            Assert.That(adjustment, Is.EqualTo(0.55));
        }

        [Test]
        public void ShouldCalculateNegativeExponentialAdjustment()
        {
            double adjustment = MathHelper.ExponentialAdjustment(-0.5, 0.2, 0.25);
            Assert.That(adjustment, Is.EqualTo(-0.55));
        }

        [Test]
        public void ShouldCalculateLeftExponentialAdjustment()
        {
            double adjustment = MathHelper.LeftExponentialAdjustment(0.5, 0.4, 0.5);
            Assert.That(adjustment, Is.EqualTo(0.4));
        }

        [Test]
        public void ShouldCalculateRightExponentialAdjustment()
        {
            double adjustment = MathHelper.RightExponentialAdjustment(-0.5, 0.4, 0.5);
            Assert.That(adjustment, Is.EqualTo(-0.4));
        }
    }
}
