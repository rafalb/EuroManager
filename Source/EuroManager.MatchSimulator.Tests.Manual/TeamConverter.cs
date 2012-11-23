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
        private World world;

        public TeamConverter(World world)
        {
            this.world = world;
        }

        public Team CreateTeam(string name)
        {
            Club club = world.Clubs.First(c => c.Name == name);
            var team = new Team(club.Name, club.Strategy);

            var clubPlayers = world.Players.Where(p => p.ClubId == club.Id);

            foreach (var player in clubPlayers)
            {
                team.AddSquadPlayer(0, player.Name, Position.FromCode(player.Position), player.Defending, player.Attacking, player.Form);
            }

            return team;
        }
    }
}
