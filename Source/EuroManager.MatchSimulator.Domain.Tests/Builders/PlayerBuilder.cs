using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Tests.Builders
{
    public class PlayerBuilder
    {
        public Player Build()
        {
            return new Player(0, "Test", A.Team.Build(), Position.LeftMidfielder, 70, 70, 70);
        }
    }
}
