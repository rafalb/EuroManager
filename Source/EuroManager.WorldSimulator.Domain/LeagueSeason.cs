using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class LeagueSeason : TournamentSeason
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        public LeagueSeason(League league, DateTime startDate, DateTime endDate, IEnumerable<Team> teams)
            : base(league, startDate, endDate, teams)
        {
            DayOfWeek = League.DayOfWeek;
            Frequency = League.Frequency;
            HasReturnRound = League.HasReturnRound;

            TeamStats = teams.Select(t => new TeamStats(t)).ToList();
        }

        protected LeagueSeason()
        {
        }

        public virtual List<TeamStats> TeamStats { get; private set; }

        public int DayOfWeekId { get; private set; }

        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)DayOfWeekId; }
            private set { DayOfWeekId = (int)value; }
        }

        public int Frequency { get; private set; }

        public bool HasReturnRound { get; private set; }

        public IEnumerable<TeamStats> Standings
        {
            get { return TeamStats.OrderByDescending(s => s).ToArray(); }
        }

        private League League
        {
            get { return (League)Tournament; }
        }

        private IEnumerable<Team> AutomaticPromotionZone
        {
            get { return Standings.Take(League.AutomaticPromotionCount).Select(s => s.Team).ToArray(); }
        }

        private IEnumerable<Team> PlayOffPromotionZone
        {
            get { return Standings.Skip(League.AutomaticPromotionCount).Take(League.ConditionalPromotionCount).Select(s => s.Team).ToArray(); }
        }

        private IEnumerable<Team> AutomaticRelegationZone
        {
            get
            {
                return Standings.Skip(Standings.Count() - League.AutomaticRelegationCount)
                    .Take(League.AutomaticRelegationCount).Select(s => s.Team).ToArray();
            }
        }

        private IEnumerable<Team> PlayOffRelegationZone
        {
            get
            {
                return Standings.Skip(Standings.Count() - League.AutomaticRelegationCount - League.ConditionalRelegationCount)
                    .Take(League.ConditionalRelegationCount).Select(s => s.Team).ToArray();
            }
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (Phase == TournamentPhase.NotStarted)
            {
                var scheduler = new Scheduler();
                var fixtures = scheduler.ScheduleLeagueFixtures(this, Teams, StartDate, EndDate, DayOfWeek, Frequency, HasReturnRound);

                foreach (var fixture in fixtures)
                {
                    addFixture(fixture);
                }

                Phase = TournamentPhase.InProgress;
                NextSchedulingDate = null;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            base.ApplyResult(result);

            GetStatsFor(result.Team1).ApplyResult(result);
            GetStatsFor(result.Team2).ApplyResult(result);

            if (Phase == TournamentPhase.InProgress)
            {
                int roundsToPlay = (TeamStats.Count - 1) * (HasReturnRound ? 2 : 1);

                if (TeamStats.All(s => s.Played == roundsToPlay))
                {
                    Phase = TournamentPhase.Finished;

                    PromotedTeams.AddRange(AutomaticPromotionZone);
                    RelegatedTeams.AddRange(AutomaticRelegationZone);
                    PromotionPlayOffTeams.AddRange(PlayOffPromotionZone);
                    RelegationPlayOffTeams.AddRange(PlayOffRelegationZone);
                }
            }
        }

        public override TournamentSeason AdvanceSeason(IEnumerable<Team> relegatedFromHigher, IEnumerable<Team> promotedFromLower)
        {
            IEnumerable<Team> updatedTeams = Teams;

            if (relegatedFromHigher.Any())
            {
                updatedTeams = updatedTeams.Except(PromotedTeams).Union(relegatedFromHigher);
            }

            if (promotedFromLower.Any())
            {
                updatedTeams = updatedTeams.Except(RelegatedTeams).Union(promotedFromLower);
            }

            IsActive = false;

            var nextSeason = new LeagueSeason(League, StartDate.AddYears(1), EndDate.AddYears(1), updatedTeams.ToArray());
            return nextSeason;
        }

        public TeamStats GetStatsFor(Team team)
        {
            return TeamStats.First(s => s.Team == team);
        }
    }
}
