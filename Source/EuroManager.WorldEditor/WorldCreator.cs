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

            var worldCreationService = new WorldCreationService();

            Console.WriteLine("Creating new world...");
            int worldId = worldCreationService.CreateWorld(world.Name, world.Year);

            foreach (var player in world.Players)
            {
                Console.WriteLine("Creating player {0}...", player.Name);
                player.NewId = worldCreationService.CreatePlayer(worldId, player.Name, player.Defending, player.Attacking, player.Form);
            }

            foreach (var club in world.Clubs)
            {
                Console.WriteLine("Creating club {0}...", club.Name);
                var clubPlayers = world.Players.Where(p => p.ClubId == club.Id);
                var squad = clubPlayers.Select(p => new Tuple<PositionCode, int>(p.Position, p.NewId)).ToArray();
                club.NewId = worldCreationService.CreateTeam(worldId, club.Name, club.Id, club.Strategy, squad);
            }

            foreach (var league in world.Leagues)
            {
                Console.WriteLine("Creating league {0}...", league.Name);
                league.NewId = worldCreationService.CreateLeague(worldId, league.Name, seasonStart, seasonEnd);

                var leagueDivisions = world.Divisions.Where(d => d.LeagueId == league.Id);

                foreach (var division in leagueDivisions)
                {
                    Console.WriteLine("Creating division {0}...", division.Name);
                    var divisionClubs = world.Clubs.Where(c => c.DivisionId == division.Id);
                    var teamIds = divisionClubs.Select(c => c.NewId).ToArray();
                    worldCreationService.CreateDivision(league.NewId, division.Name, division.Level,
                        seasonStart, new DateTime(world.Year + 1, 05, 16), DayOfWeek.Saturday, 1, teamIds);
                }
            }

            Console.WriteLine("Creating European Cups...");
            var championsLeagueClubs = world.Clubs.Where(c => world.EuropeanCupsClubs.Any(cr => cr.ClubId == c.Id && cr.Level == 1));
            var europaLeagueClubs = world.Clubs.Where(c => world.EuropeanCupsClubs.Any(cr => cr.ClubId == c.Id && cr.Level == 2));

            worldCreationService.CreateEuroLeague(worldId,
                championsLeagueClubs.Select(c => c.NewId).ToArray(),
                europaLeagueClubs.Select(c => c.NewId).ToArray());

            Console.WriteLine("World created successfully.");

            return worldId;
        }
    }
}
