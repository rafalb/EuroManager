using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests.WorldTests
{
    [TestFixture]
    public class WhenCreatingFirstDefaultWorld : UnitTestFixture
    {
        private World world;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            world = new World("Test", A.Date, 2013);
            world.SetAsDefault(null);
        }

        [Test]
        public void ShouldBeginOnDayBeforeSeasonStart()
        {
            Assert.That(world.Date, Is.EqualTo(world.SeasonStartDate.AddDays(-1)));
        }

        [Test]
        public void ShouldSetAsDefaultWorld()
        {
            Assert.That(world.IsDefault, Is.True);
        }

        [Test]
        public void ShouldNotAdvanceDateWithFixturesLeft()
        {
            Assert.That(() => world.AdvanceDate(true), Throws.InvalidOperationException);
        }

        [Test]
        public void ShouldCalculateNextSeasonStartDate()
        {
            Assert.That(world.NextSeasonStartDate, Is.EqualTo(world.SeasonEndDate.AddDays(1)));
        }
    }

    [TestFixture]
    public class WhenSwitchingDefaultWorld : UnitTestFixture
    {
        private World world1;
        private World world2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            world1 = new World("Test 1", A.Date, 2013);
            world2 = new World("Test 2", A.Date, 2015);

            world1.SetAsDefault(null);
            world2.SetAsDefault(currentDefaultWorld: world1);
        }

        [Test]
        public void ShouldSetNewWorldAsDefault()
        {
            Assert.That(world2.IsDefault, Is.True);
        }

        [Test]
        public void ShouldSetPreviousWorldAsNotDefault()
        {
            Assert.That(world1.IsDefault, Is.False);
        }
    }

    [TestFixture]
    public class WhenAdvancingDate : UnitTestFixture
    {
        private World world;
        private DateTime originalDate;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            world = new World("Test", A.Date, 2013);
            originalDate = world.Date;

            world.AdvanceDate(false);
            world.AdvanceDate(false);
            world.AdvanceDate(false);
        }

        [Test]
        public void ShouldUpdateWorldDate()
        {
            Assert.That(world.Date, Is.EqualTo(originalDate.AddDays(3)));
        }
    }

    [TestFixture]
    public class WhenSeasonEnded : UnitTestFixture
    {
        private World world;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            world = new World("Test", A.Date, 2012);
            WorldTestHelper.AdvanceDate(world, new DateTime(2013, 07, 31));
        }

        [Test]
        public void ShouldNotAdvanceDate()
        {
            Assert.That(() => world.AdvanceDate(false), Throws.InvalidOperationException);
        }

        [Test]
        public void ShouldNotAdvanceSeasonWithFixturesLeft()
        {
            Assert.That(() => world.AdvanceSeason(true), Throws.InvalidOperationException);
        }

        [Test]
        public void ShouldAdvanceSeasonWithNoFixturesLeft()
        {
            world.AdvanceSeason(false);
            Assert.That(world.SeasonNumber, Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class WhenAdvancingSeason : UnitTestFixture
    {
        private World world;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            world = new World("Test", A.Date, 2013);
            WorldTestHelper.AdvanceDate(world, new DateTime(2014, 07, 31));
            world.AdvanceSeason(false);
        }

        [Test]
        public void ShouldUpdateSeasonStartDate()
        {
            Assert.That(world.SeasonStartDate.Year, Is.EqualTo(2014));
        }

        [Test]
        public void ShouldUpdateSeasonEndDate()
        {
            Assert.That(world.SeasonEndDate.Year, Is.EqualTo(2015));
        }

        [Test]
        public void ShouldAdvanceDate()
        {
            world.AdvanceDate(false);
            Assert.That(world.Date, Is.EqualTo(world.SeasonStartDate));
        }
    }

    public static class WorldTestHelper
    {
        public static void AdvanceDate(World world, DateTime date)
        {
            while (world.Date < date)
            {
                world.AdvanceDate(false);
            }
        }
    }
}
