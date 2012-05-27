using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public interface IFixtureSet
    {
        void ScheduleFixtures(Action<Fixture> addFixture);

        void ApplyResult(MatchResult result);
    }
}
