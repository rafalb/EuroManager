using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    public static class FixtureSetTestHelper
    {
        public static void PlayAllFixtures(IFixtureSet fixtureSet)
        {
            var fixtures = new List<Fixture>();
            fixtureSet.ScheduleFixtures(fixtures.Add);
            fixtures.ForEach(f => fixtureSet.ApplyResult(A.MatchResult.ForFixture(f).Build()));
        }
    }
}
