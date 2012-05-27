using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class GroupStageTests : UnitTestFixture
    {
        [Test]
        public void ShouldCalculateRoundCountWhenHasReturnRound()
        {
            GroupStage stage = new GroupStage(4, 4, 2, false, true);

            Assert.That(stage.RoundCount, Is.EqualTo(6));
        }

        [Test]
        public void ShouldCalculateRoundCountWhenNotHasReturnRound()
        {
            GroupStage stage = new GroupStage(4, 5, 2, false, false);

            Assert.That(stage.RoundCount, Is.EqualTo(5));
        }
    }
}
