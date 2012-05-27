using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class TeamBuilder
    {
        private World world;

        public TeamBuilder InWorld(World world)
        {
            this.world = world;
            return this;
        }

        public Team Build()
        {
            return new Team(world, "Test", TeamStrategy.Center, Enumerable.Empty<SquadMember>());
        }

        public IEnumerable<Team> Repeat(int count)
        {
            return Enumerable.Repeat(0, count).Select(x => Build()).ToArray();
        }
    }
}
