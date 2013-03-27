using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.DataAccess
{
    public class WorldContext : DbContext
    {
        private WorldModelMapper modelMapper = new WorldModelMapper();

        public WorldContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<World> Worlds { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<NationalLeague> NationalLeagues { get; set; }

        public DbSet<CompetitionSeason> CompetitionSeasons { get; set; }

        public DbSet<NationalLeagueSeason> NationalLeagueSeasons { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Cup> Cups { get; set; }

        public DbSet<League> Leagues { get; set; }

        public DbSet<TournamentSeason> TournamentSeasons { get; set; }

        public DbSet<CupSeason> CupSeasons { get; set; }

        public DbSet<LeagueSeason> LeagueSeasons { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<SquadMember> SquadMembers { get; set; }

        public DbSet<Fixture> Fixtures { get; set; }

        public DbSet<MatchResult> Results { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<PlayerTournamentStats> PlayerTournamentStats { get; set; }

        public World GetDefaultWorld(bool readOnly = false)
        {
            return Worlds.ReadOnly(readOnly).FirstOrDefault(w => w.IsDefault);
        }

        public IEnumerable<CompetitionSeason> GetActiveCompetitionSeasonsForAdvancingDate(int worldId)
        {
            Competitions.Where(c => c.WorldId == worldId).ToArray();
            Tournaments.Where(t => t.WorldId == worldId).ToArray();
            CupSeasons.Where(c => c.WorldId == worldId && c.IsActive).ToArray();
            LeagueSeasons.Where(l => l.WorldId == worldId && l.IsActive).ToArray();
            
            var nationalLeagueSeasons = from s in NationalLeagueSeasons
                                            .Include(s => s.PlayOffs)
                                        where s.WorldId == worldId && s.IsActive
                                        select s;

            return nationalLeagueSeasons.ToArray();
        }

        public IEnumerable<CompetitionSeason> GetActiveCompetitionSeasonsForAdvancingSeason(int worldId)
        {
            Competitions.Where(c => c.WorldId == worldId).ToArray();
            Tournaments.Where(t => t.WorldId == worldId).ToArray();
            Cups.Include(c => c.Stages).Where(c => c.WorldId == worldId).ToArray();
            Teams.Where(t => t.WorldId == worldId).ToArray();

            (from c in CupSeasons
                 .Include(c => c.PromotedTeams)
                 .Include(c => c.RelegatedTeams)
                 .Include(c => c.Stages)
             where c.WorldId == worldId && c.IsActive
             select c).ToArray();

            (from d in LeagueSeasons
                 .Include(d => d.PromotedTeams)
                 .Include(d => d.RelegatedTeams)
                 .Include(d => d.TeamStats)
             where d.WorldId == worldId && d.IsActive
             select d).ToArray();

            var nationalLeagueSeasons = from s in NationalLeagueSeasons
                                            .Include(s => s.PlayOffs)
                                        where s.WorldId == worldId && s.IsActive
                                        select s;

            return nationalLeagueSeasons.ToArray();
        }

        public IEnumerable<TournamentSeason> GetActiveTournamentSeasonsForFixtureScheduling(int worldId, DateTime date)
        {
            var tournaments = TournamentSeasons
                .Include(s => s.Teams)
                .Where(t => t.WorldId == worldId && t.IsActive &&
                    t.NextSchedulingDate != null && t.NextSchedulingDate <= date)
                .ToArray();

            return tournaments;
        }

        public LeagueSeason GetActiveLeagueSeasonWithStandings(int leagueId, bool readOnly = false)
        {
            return LeagueSeasons.ReadOnly(readOnly)
                .Include(l => l.TeamStats)
                .Where(l => l.TournamentId == leagueId && l.IsActive)
                .FirstOrDefault();
        }

        public CupSeason GetActiveCupSeasonWithStandings(int cupId, bool readOnly = false)
        {
            return CupSeasons.ReadOnly(readOnly)
                .Include(c => c.Stages)
                .Where(c => c.TournamentId == cupId && c.IsActive)
                .FirstOrDefault();
        }

        public bool AreAnyFixturesLeftToPlay(int worldId, DateTime date)
        {
            return Fixtures
                .Where(f => f.WorldId == worldId && f.ResultId == null && f.Date == date)
                .Any();
        }

        public Fixture GetNextFixtureToPlay(int tournamentId, DateTime date)
        {
            TournamentSeason season = TournamentSeasons.FirstOrDefault(s => s.TournamentId == tournamentId && s.IsActive);
            Fixture fixture = null;

            if (season != null)
            {
                fixture = Fixtures.FirstOrDefault(f => f.TournamentSeasonId == season.Id && f.ResultId == null && f.Date == date);

                if (fixture != null)
                {
                    Entry(season).Collection(s => s.Teams).Load();
                    Entry(fixture).Reference(f => f.Team1).Load();
                    Entry(fixture).Reference(f => f.Team2).Load();

                    (from s in SquadMembers
                         .Include(s => s.Player)
                     where s.TeamId == fixture.Team1Id || s.TeamId == fixture.Team2Id
                     select s).ToArray();
                }
            }

            return fixture;
        }

        public IEnumerable<MatchResult> GetMatchResults(int tournamentId, DateTime date, bool readOnly = false)
        {
            TournamentSeason season = TournamentSeasons.First(s => s.TournamentId == tournamentId && s.IsActive);
            var results = Results.ReadOnly(readOnly)
                .Where(r => r.TournamentSeasonId == season.Id && r.Date == date)
                .ToArray();

            var resultIds = results.Select(r => r.Id).ToArray();
            Goals.ReadOnly(readOnly).Where(g => resultIds.Contains(g.ResultId)).ToArray();

            return results;
        }

        public MatchResult GetLastMatchResult(int tournamentId, DateTime date, bool readOnly = false)
        {
            TournamentSeason season = TournamentSeasons.First(s => s.TournamentId == tournamentId && s.IsActive);
            var result = Results.ReadOnly(readOnly)
                .Where(r => r.TournamentSeasonId == season.Id && r.Date == date)
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            return result;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelMapper.MapModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
