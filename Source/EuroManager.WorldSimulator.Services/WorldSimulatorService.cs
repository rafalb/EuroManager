using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using AutoMapper;
using EuroManager.Common.Domain;
using EuroManager.WorldSimulator.DataAccess;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.Services
{
    public class WorldSimulatorService : ApplicationService
    {
        public int GetWorldCount()
        {
            return Context.Worlds.Count();
        }

        public DateTime GetCurrentDate()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            return world.Date;
        }

        public bool IsSeasonEnd()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            return world.IsSeasonEnd;
        }

        public DateTime GetNextSeasonStartDate()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            return world.NextSeasonStartDate;
        }

        public IEnumerable<Data.Tournament> GetTournaments()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            var tournaments = from s in Context.TournamentSeasons.ReadOnly(true)
                              join t in Context.Tournaments on s.TournamentId equals t.Id
                              where s.WorldId == world.Id && s.IsActive
                              orderby t.Id
                              select new Data.Tournament
                              {
                                  Id = s.Tournament.Id,
                                  Name = s.Tournament.Name,
                                  Level = s.Tournament.Level
                              };

            return tournaments.ToArray();
        }

        public Data.Team GetTeam(int teamId)
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            Team team = Context.Teams.ReadOnly(true).Single(t => t.Id == teamId && t.WorldId == world.Id);

            var mappedTeam = Mapper.Map<Data.Team>(team);
            return mappedTeam;
        }

        public IEnumerable<Data.Player> GetTeamPlayers(int teamId)
        {
            World world = Context.GetDefaultWorld(readOnly: true);

            var players = from p in Context.Players.ReadOnly(true)
                          join m in Context.SquadMembers on p.Id equals m.PlayerId
                          where m.TeamId == teamId
                          select new Data.Player
                          {
                              Id = p.Id,
                              Name = p.Name,
                              Position = m.Position,
                              DefensiveSkills = p.DefensiveSkills,
                              OffensiveSkills = p.OffensiveSkills,
                              Form = p.Form
                          };

            return players.ToArray();
        }

        public IEnumerable<Data.PlayerStats> GetCombinedPlayerStatsByTeam(int teamId)
        {
            World world = Context.GetDefaultWorld(readOnly: true);

            var stats = from s in Context.PlayerTournamentStats.ReadOnly(true)
                        join m in Context.SquadMembers on s.PlayerId equals m.PlayerId
                        join t in Context.TournamentSeasons on s.TournamentSeasonId equals t.Id
                        join tm in Context.Teams on m.TeamId equals tm.Id
                        where tm.WorldId == world.Id && tm.Id == teamId && t.IsActive
                        select new
                        {
                            PlayerId = s.PlayerId,
                            Stats = s,
                            Position = m.Position,
                            TeamId = tm.Id,
                            TeamName = tm.Name,
                            TeamShortName = tm.ShortName
                        };

            var combinedStats = from s in stats.ToArray()
                                group s by s.PlayerId into g
                                let cs = PlayerTournamentStats.Combine(g.Select(gr => gr.Stats).ToArray())
                                let fs = g.First()
                                select new Data.PlayerStats
                                {
                                    Id = fs.PlayerId ?? 0,
                                    Name = fs.Stats.PlayerName,
                                    TeamId = fs.TeamId,
                                    TeamName = fs.TeamName,
                                    TeamShortName = fs.TeamShortName,
                                    Position = fs.Position,
                                    Played = cs.Played,
                                    Goals = cs.Goals,
                                    Assists = cs.Assists,
                                    Rating = cs.AverageRating
                                };

            return combinedStats.ToArray();
        }

        public DateTime? GetNextFixtureDate()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            Fixture nextFixture = (from f in Context.Fixtures
                                   where f.Date > world.Date
                                   orderby f.Date
                                   select f).FirstOrDefault();

            return nextFixture == null ? (DateTime?)null : nextFixture.Date;
        }

        public IEnumerable<Data.Tournament> GetTournamentsWithFixturesForToday()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            var tournaments = from s in Context.TournamentSeasons.ReadOnly(true)
                              where s.WorldId == world.Id
                              where Context.Fixtures.Any(f => f.TournamentSeasonId == s.Id && f.Date == world.Date && f.Result == null)
                              select new Data.Tournament
                              {
                                  Id = s.Tournament.Id,
                                  Name = s.Tournament.Name,
                                  Level = s.Tournament.Level
                              };
            return tournaments.ToArray();
        }

        public DateTime? GetLastMatchResultDateForActiveTournaments()
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            var q = from r in Context.Results.ReadOnly(true)
                    join s in Context.TournamentSeasons on r.TournamentSeasonId equals s.Id
                    where s.WorldId == world.Id && s.IsActive
                    orderby r.Date descending
                    select r;

            MatchResult result = q.FirstOrDefault();

            return result == null ? (DateTime?)null : result.Date;
        }

        public IEnumerable<Data.Tournament> GetTournamentsWithResultsForDate(DateTime date)
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            var tournaments = from s in Context.TournamentSeasons.ReadOnly(true)
                              where s.WorldId == world.Id
                              where Context.Results.Any(r => r.TournamentSeasonId == s.Id && r.Date == date)
                              select new Data.Tournament
                              {
                                  Id = s.Tournament.Id,
                                  Name = s.Tournament.Name,
                                  Level = s.Tournament.Level
                              };
            return tournaments.ToArray();
        }

        public IEnumerable<Data.MatchResult> GetMatchResults(int tournamentId, DateTime date)
        {
            var results = Context.GetMatchResults(tournamentId, date, readOnly: true);

            var mappedResults = Mapper.Map<MatchResult[], Data.MatchResult[]>(results.ToArray());
            return mappedResults;
        }

        public IEnumerable<Data.MatchResult> GetRecentMatchResults(int tournamentId)
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            TournamentSeason season = Context.TournamentSeasons.ReadOnly(true)
                .First(s => s.TournamentId == tournamentId && s.IsActive);

            MatchResult lastResult = Context.Results.ReadOnly(true)
                .Where(r => r.TournamentSeasonId == season.Id)
                .OrderByDescending(r => r.Date)
                .FirstOrDefault();

            if (lastResult == null)
            {
                return Enumerable.Empty<Data.MatchResult>();
            }
            else
            {
                var results = Context.GetMatchResults(tournamentId, lastResult.Date, readOnly: true);

                var mappedResults = Mapper.Map<MatchResult[], Data.MatchResult[]>(results.ToArray());
                return mappedResults;
            }
        }

        public Data.MatchResultDetails GetMatchResultDetails(int resultId)
        {
            MatchResult result = Context.GetMatchResult(resultId, readOnly: true);

            var mappedResult = Mapper.Map<MatchResult, Data.MatchResultDetails>(result);
            return mappedResult;
        }

        public Data.MatchResult GetLastMatchResult(int tournamentId)
        {
            World world = Context.GetDefaultWorld(readOnly: true);
            var result = Context.GetLastMatchResult(tournamentId, world.Date, readOnly: true);

            var mappedResult = Mapper.Map<MatchResult, Data.MatchResult>(result);
            return mappedResult;
        }

        public IEnumerable<Data.TeamStats> GetLeagueStandings(int leagueId)
        {
            LeagueSeason leagueSeason = Context.GetActiveLeagueSeasonWithStandings(leagueId, readOnly: true);

            if (leagueSeason == null)
            {
                return Enumerable.Empty<Data.TeamStats>();
            }
            else
            {
                var standings = leagueSeason.Standings.ToArray();
                var mappedStandings = Mapper.Map<TeamStats[], Data.TeamStats[]>(standings);
                return mappedStandings;
            }
        }

        public IEnumerable<Data.TeamStats> GetCupGroupStageStandings(int cupId)
        {
            CupSeason cupSeason = Context.GetActiveCupSeasonWithStandings(cupId, readOnly: true);

            if (cupSeason == null || cupSeason.CurrentStage == null || !(cupSeason.CurrentStage is GroupStageSeason))
            {
                return Enumerable.Empty<Data.TeamStats>();
            }
            else
            {
                var groupStage = (GroupStageSeason)cupSeason.CurrentStage;

                var standings = groupStage.Groups.SelectMany(g => g.Standings).ToArray();
                var mappedStandings = Mapper.Map<TeamStats[], Data.TeamStats[]>(standings);
                return mappedStandings;
            }
        }

        public IEnumerable<Data.PlayerStats> GetTopRatedPlayersStats(int tournamentId, int count)
        {
            var q = from s in Context.PlayerTournamentStats.ReadOnly(true)
                    join m in Context.SquadMembers on s.PlayerId equals m.PlayerId
                    join t in Context.TournamentSeasons on s.TournamentSeasonId equals t.Id
                    join tm in Context.Teams on m.TeamId equals tm.Id
                    where t.TournamentId == tournamentId && t.IsActive
                    orderby s.AverageRating descending
                    select new Data.PlayerStats
                    {
                        Id = s.Id,
                        Name = s.PlayerName,
                        TeamId = tm.Id,
                        TeamName = tm.Name,
                        TeamShortName = tm.ShortName,
                        Position = m.Position,
                        Played = s.Played,
                        Goals = s.Goals,
                        Assists = s.Assists,
                        Rating = s.AverageRating
                    };

            return q.Take(count).ToArray();
        }

        public IEnumerable<Data.PlayerStats> GetTopGoalScorersStats(int tournamentId, int count)
        {
            var q = from s in Context.PlayerTournamentStats.ReadOnly(true)
                    join m in Context.SquadMembers on s.PlayerId equals m.PlayerId
                    join t in Context.TournamentSeasons on s.TournamentSeasonId equals t.Id
                    join tm in Context.Teams on m.TeamId equals tm.Id
                    where t.TournamentId == tournamentId && t.IsActive
                    orderby s.Goals descending
                    select new Data.PlayerStats
                    {
                        Id = s.Id,
                        Name = s.PlayerName,
                        TeamId = tm.Id,
                        TeamName = tm.Name,
                        TeamShortName = tm.ShortName,
                        Position = m.Position,
                        Played = s.Played,
                        Goals = s.Goals,
                        Assists = s.Assists,
                        Rating = s.AverageRating
                    };

            return q.Take(count).ToArray();
        }

        public IEnumerable<Data.PlayerStats> GetTopAssistantsStats(int tournamentId, int count)
        {
            var q = from s in Context.PlayerTournamentStats.ReadOnly(true)
                    join m in Context.SquadMembers on s.PlayerId equals m.PlayerId
                    join t in Context.TournamentSeasons on s.TournamentSeasonId equals t.Id
                    join tm in Context.Teams on m.TeamId equals tm.Id
                    where t.TournamentId == tournamentId && t.IsActive
                    orderby s.Assists descending
                    select new Data.PlayerStats
                    {
                        Id = s.Id,
                        Name = s.PlayerName,
                        TeamId = tm.Id,
                        TeamName = tm.Name,
                        TeamShortName = tm.ShortName,
                        Position = m.Position,
                        Played = s.Played,
                        Goals = s.Goals,
                        Assists = s.Assists,
                        Rating = s.AverageRating
                    };

            return q.Take(count).ToArray();
        }

        public void SwitchDefaultWorld(int worldId)
        {
            World currentDefaultWorld = Context.GetDefaultWorld();
            World newDefaultWorld = Context.Worlds.Find(worldId);

            newDefaultWorld.SetAsDefault(currentDefaultWorld);

            Context.SaveChanges();
        }

        public bool PlayNextTodayFixture(int tournamentId)
        {
            World world = Context.GetDefaultWorld();
            Fixture fixture = Context.GetNextFixtureToPlay(tournamentId, world.Date);

            if (fixture == null)
            {
                return false;
            }
            else
            {
                var matchSimulator = new MatchSimulatorFacade();
                fixture.Result = matchSimulator.Play(fixture);
                var season = Context.TournamentSeasons.Find(fixture.TournamentSeasonId);
                season.ApplyResult(fixture.Result);

                Context.SaveChanges();
                return true;
            }
        }

        public void AdvanceDate()
        {
            World world = Context.GetDefaultWorld();
            bool areAnyFixturesLeft = Context.AreAnyFixturesLeftToPlay(world.Id, world.Date);

            world.AdvanceDate(areAnyFixturesLeft);

            var competitionSeasons = Context.GetActiveCompetitionSeasonsForAdvancingDate(world.Id);

            foreach (var competitionSeason in competitionSeasons)
            {
                competitionSeason.AdvanceDate();
            }

            var tournamentSeasons = Context.GetActiveTournamentSeasonsForFixtureScheduling(world.Id, world.Date);

            foreach (var tournamentSeason in tournamentSeasons)
            {
                tournamentSeason.ScheduleFixtures(f => Context.Fixtures.Add(f));
            }

            Context.SaveChanges();
        }

        public void AdvanceSeason()
        {
            World world = Context.GetDefaultWorld();
            bool areAnyFixturesLeft = Context.AreAnyFixturesLeftToPlay(world.Id, world.Date);

            world.AdvanceSeason(areAnyFixturesLeft);

            var competitionSeasons = Context.GetActiveCompetitionSeasonsForAdvancingSeason(world.Id);

            foreach (var competitionSeason in competitionSeasons)
            {
                CompetitionSeason newSeason = competitionSeason.AdvanceSeason();
                Context.CompetitionSeasons.Add(newSeason);
            }

            Context.SaveChanges();
        }
    }
}
