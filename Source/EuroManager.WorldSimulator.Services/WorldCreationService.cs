using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;
using EuroManager.WorldSimulator.DataAccess;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.Services
{
    public class WorldCreationService : ApplicationService
    {
        public int CreateWorld(string name, int startYear)
        {
            var world = new World(name, DateTime.Now, startYear);

            Context.Worlds.Add(world);
            Context.SaveChanges();

            return world.Id;
        }

        public int CreatePlayer(int worldId, string name, int defensiveSkills, int offensiveSkills, int form)
        {
            var world = Context.Worlds.Find(worldId);
            var player = new Player(world, name, defensiveSkills, offensiveSkills, form);

            Context.Players.Add(player);
            Context.SaveChanges();

            return player.Id;
        }

        public int CreateTeam(int worldId, string name, TeamStrategy strategy, IEnumerable<Tuple<PositionCode, int>> squad)
        {
            var world = Context.Worlds.Find(worldId);
            var team = new Team(world, name, strategy,
                squad.Select(s => new SquadMember(s.Item1, s.Item2)).ToArray());

            Context.Teams.Add(team);
            Context.SaveChanges();

            return team.Id;
        }

        public int CreateLeague(int worldId, string name, DateTime startDate, DateTime endDate)
        {
            var world = Context.Worlds.Find(worldId);
            var league = new NationalLeague(world, name);
            var leagueSeason = new NationalLeagueSeason(league, startDate, endDate);
            league.PlayOffs = new LeaguePlayOffs(league, name + " Play-offs", DayOfWeek.Wednesday);

            Context.NationalLeagues.Add(league);
            Context.NationalLeagueSeasons.Add(leagueSeason);
            Context.SaveChanges();

            return league.Id;
        }

        public int CreateDivision(int leagueId, string name, int level,
            DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek, int frequency,
            IEnumerable<int> teamIds)
        {
            NationalLeague league = Context.NationalLeagues.Find(leagueId);
            NationalLeagueSeason leagueSeason = Context.NationalLeagueSeasons.First(s => s.CompetitionId == leagueId && s.IsActive);

            var teams = teamIds.Select(id => Context.Teams.Find(id)).ToArray();
            var division = new League(league, name, level, dayOfWeek, frequency);
            var divisionSeason = new LeagueSeason(division, startDate, endDate, teams);

            leagueSeason.AddDivisionSeason(divisionSeason);
            Context.SaveChanges();

            return division.Id;
        }

        public int CreateEuroLeague(int worldId, IEnumerable<int> division1ClubIds, IEnumerable<int> division2ClubIds, IEnumerable<int> division3ClubIds)
        {
            var world = Context.Worlds.Find(worldId);
            var division1Clubs = division1ClubIds.Select(id => Context.Teams.Find(id)).ToArray();
            var division2Clubs = division2ClubIds.Select(id => Context.Teams.Find(id)).ToArray();
            var division3Clubs = division3ClubIds.Select(id => Context.Teams.Find(id)).ToArray();

            var euroLeagueBuilder = new EuroLeagueBuilder(world);
            var euroLeauge = euroLeagueBuilder.CreateEuroLeague(division1Clubs, division2Clubs, division3Clubs);

            Context.NationalLeagues.Add(euroLeauge.League);
            Context.NationalLeagueSeasons.Add(euroLeauge);
            Context.SaveChanges();

            return euroLeauge.Id;
        }
    }
}
