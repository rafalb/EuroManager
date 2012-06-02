using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain.Tests.Builders;

namespace EuroManager.MatchSimulator.Domain.Tests
{
    public static class A
    {
        public static PlayerBuilder Player
        {
            get { return new PlayerBuilder(); }
        }

        public static TeamBuilder Team
        {
            get { return new TeamBuilder(); }
        }

        public static MatchBuilder Match
        {
            get { return new MatchBuilder(); }
        }
    }
}
