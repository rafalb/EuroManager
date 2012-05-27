using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class TournamentSeason : IEntity, IFixtureSet
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        protected TournamentSeason(Tournament tournament, DateTime startDate, DateTime endDate, IEnumerable<Team> teams = null)
        {
            World = tournament.World;
            Tournament = tournament;
            StartDate = startDate;
            EndDate = endDate;
            NextSchedulingDate = World.Date;

            Teams = teams == null ? new List<Team>() : teams.ToList();
            PromotedTeams = new List<Team>();
            RelegatedTeams = new List<Team>();
            PromotionPlayOffTeams = new List<Team>();
            RelegationPlayOffTeams = new List<Team>();
            
            Level = Tournament.Level;
            IsActive = true;
            Phase = TournamentPhase.NotStarted;
        }

        protected TournamentSeason()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? WorldId { get; private set; }

        public virtual World World { get; private set; }

        public int? TournamentId { get; private set; }

        public virtual Tournament Tournament { get; private set; }

        public bool IsActive { get; protected set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public DateTime? NextSchedulingDate { get; protected set; }

        public virtual List<Team> Teams { get; private set; }

        public virtual List<Team> PromotedTeams { get; private set; }

        public virtual List<Team> RelegatedTeams { get; private set; }

        public virtual List<Team> PromotionPlayOffTeams { get; private set; }

        public virtual List<Team> RelegationPlayOffTeams { get; private set; }

        public int PhaseId { get; private set; }

        public TournamentPhase Phase
        {
            get { return (TournamentPhase)PhaseId; }
            protected set { PhaseId = (int)value; }
        }

        public bool IsFinished
        {
            get { return Phase == TournamentPhase.Finished; }
        }

        public int Level { get; private set; }

        public abstract void ScheduleFixtures(Action<Fixture> addFixture);

        public abstract void ApplyResult(MatchResult result);

        public virtual TournamentSeason AdvanceSeason(IEnumerable<Team> relegatedFromHigher, IEnumerable<Team> promotedFromLower)
        {
            IsActive = false;
            return null;
        }

        public void PromoteTeam(Team team)
        {
            PromotedTeams.Add(team);
        }

        public void RelegateTeam(Team team)
        {
            RelegatedTeams.Add(team);
        }

        protected void AddTeams(IEnumerable<Team> teams)
        {
            Teams.AddRange(teams);
        }
    }
}
