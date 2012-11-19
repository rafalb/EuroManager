using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldEditor
{
    public class WorldCreator
    {
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EuroLeague", Justification = "Special name")]
        public int CreateWorld(Loader.World world)
        {
            DateTime seasonStart = new DateTime(world.Year, 08, 01);
            DateTime seasonEnd = new DateTime(world.Year + 1, 05, 31);

            var leagues = world.Leagues;
            var divisions = leagues.SelectMany(l => l.Divisions).ToArray();
            var clubs = divisions.SelectMany(d => d.Clubs).Union(world.RestOfWorld.Clubs).ToArray();
            var players = clubs.SelectMany(t => t.Players).ToArray();

            var worldCreationService = new WorldCreationService();

            Console.WriteLine("Creating new world...");
            int worldId = worldCreationService.CreateWorld(world.Name, world.Year);

            foreach (var player in players)
            {
                Console.WriteLine("Creating player {0}...", player.Name);
                player.NewId = worldCreationService.CreatePlayer(worldId, player.Name, player.Defending, player.Attacking, player.Form);
            }

            foreach (var club in clubs)
            {
                Console.WriteLine("Creating club {0}...", club.Name);
                var squad = club.Players.Select(p => new Tuple<PositionCode, int>(p.Position, p.NewId)).ToArray();
                club.NewId = worldCreationService.CreateTeam(worldId, club.Name, club.Strategy, squad);
            }

            foreach (var league in leagues)
            {
                Console.WriteLine("Creating league {0}...", league.Name);
                league.NewId = worldCreationService.CreateLeague(worldId, league.Name, seasonStart, seasonEnd);

                foreach (var division in league.Divisions)
                {
                    Console.WriteLine("Creating division {0}...", division.Name);
                    var teamIds = division.Clubs.Select(c => c.NewId).ToArray();
                    worldCreationService.CreateDivision(league.NewId, division.Name, division.Level,
                        seasonStart, new DateTime(world.Year + 1, 05, 16), DayOfWeek.Saturday, 1, teamIds);
                }
            }

            Console.WriteLine("Creating European Cups...");
            worldCreationService.CreateEuroLeague(worldId,
                world.EuropeanCups.ChampionsLeague.Clubs.Select(c => clubs.First(c2 => c2.Id == c.Id).NewId).ToArray(),
                world.EuropeanCups.EuropaLeague.Clubs.Select(c => clubs.First(c2 => c2.Id == c.Id).NewId).ToArray());

            Console.WriteLine("World created successfully.");

            return worldId;
        }
    }
}
