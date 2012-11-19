using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using NUnit.Framework;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [TestFixture]
    public class EuroLeagueBuilderTests : UnitTestFixture
    {
        [Test]
        public void ShouldCreateEuroLeague()
        {
            var world = A.World.Build();
            var euroLeagueBuilder = new EuroLeagueBuilder(world);

            var teams1 = Enumerable.Repeat(0, 8).Select(x => A.Team.Build()).ToArray();
            var teams2 = Enumerable.Repeat(0, 8).Select(x => A.Team.Build()).ToArray();

            var euroLeagueSeason = euroLeagueBuilder.CreateEuroLeague(teams1, teams2);

            Assert.That(euroLeagueSeason, Is.Not.Null);
        }
    }
}
