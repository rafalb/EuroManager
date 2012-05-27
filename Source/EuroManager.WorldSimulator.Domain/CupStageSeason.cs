using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class CupStageSeason : IEntity, IFixtureSet
    {
        protected CupStageSeason(CupStage stage)
        {
            Stage = stage;
            TeamCount = stage.TeamCount;
            RoundCount = stage.RoundCount;

            Phase = TournamentPhase.NotStarted;
            Teams = new List<Team>();
            PromotedTeams = new List<Team>();
        }

        protected CupStageSeason()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int StageId { get; private set; }

        public virtual CupStage Stage { get; private set; }

        public int CupSeasonId { get; private set; }

        public virtual CupSeason CupSeason { get; set; }

        public int PhaseId { get; private set; }

        public TournamentPhase Phase
        {
            get { return (TournamentPhase)PhaseId; }
            protected set { PhaseId = (int)value; }
        }

        public int TeamCount { get; private set; }

        public int RoundCount { get; private set; }

        public virtual List<Team> Teams { get; private set; }

        public virtual List<Team> PromotedTeams { get; private set; }

        public bool IsFinished
        {
            get { return Phase == TournamentPhase.Finished; }
        }

        public abstract void ScheduleRoundDates(IEnumerable<DateTime> roundDates);

        public abstract void Activate(IEnumerable<Team> teams);

        public abstract void ScheduleFixtures(Action<Fixture> addFixture);

        public abstract void ApplyResult(MatchResult result);

        protected void PromoteTeam(Team team)
        {
            PromotedTeams.Add(team);
        }
    }
}
