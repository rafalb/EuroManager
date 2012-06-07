using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class PlayerBuilder
    {
        private IRandomGenerator random = RandomGenerator.Current;

        public Player Build()
        {
            return new Player(A.World.Build(), "Test " + (int)random.Value(100, 1000), 70, 75, 80);
        }

        public IEnumerable<Player> Repeat(int count)
        {
            return Enumerable.Range(0, count).Select(x => Build());
        }
    }
}
