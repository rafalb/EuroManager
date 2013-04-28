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
        private IEnumerable<Player> players = Enumerable.Empty<Player>();

        public TeamBuilder InWorld(World world)
        {
            this.world = world;
            return this;
        }

        public TeamBuilder WithPlayers(params Player[] players)
        {
            this.players = players;
            return this;
        }

        public Team Build()
        {
            SquadMember[] squad = players.Select(p => new SquadMember(PositionCode.CB, p)).ToArray();
            return new Team(world, "Test", "Test", TeamStrategy.Center, squad);
        }

        public IEnumerable<Team> Repeat(int count)
        {
            return Enumerable.Repeat(0, count).Select(x => Build()).ToArray();
        }
    }
}
