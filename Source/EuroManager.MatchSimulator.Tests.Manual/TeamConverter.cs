using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain;
using EuroManager.WorldEditor.Loader;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public class TeamConverter
    {
        private IEnumerable<Club> clubs;

        public TeamConverter(World world)
        {
            clubs = Enumerable.Union(
                        world.Leagues.SelectMany(l => l.Divisions).SelectMany(d => d.Clubs),
                        world.RestOfWorld.Clubs)
                    .ToArray();
        }

        public Team CreateTeam(string name)
        {
            Club club = clubs.First(c => c.Name == name);
            var team = new Team(club.Name, club.Strategy);

            foreach (var player in club.Players)
            {
                team.AddSquadPlayer(0, player.Name, Position.FromCode(player.Position), player.Defending, player.Attacking, player.Form);
            }

            return team;
        }
    }
}
