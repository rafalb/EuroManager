using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class KnockoutStageSeason : CupStageSeason
    {
        private IRandomGenerator randomGenerator = RandomGenerator.Current;

        public KnockoutStageSeason(KnockoutStage stage)
            : base(stage)
        {
        }

        protected KnockoutStageSeason()
        {
        }

        public DateTime? Date { get; private set; }

        public int PlayedCount { get; private set; }

        public override void ScheduleRoundDates(IEnumerable<DateTime> roundDates)
        {
            Date = roundDates.Single();
        }

        public override void Activate(IEnumerable<Team> teams)
        {
            Teams.AddRange(teams);
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (Phase == TournamentPhase.NotStarted)
            {
                var teams = randomGenerator.Sort(Teams);

                var teams1 = teams.Take(TeamCount / 2).ToArray();
                var teams2 = teams.Skip(TeamCount / 2).Take(TeamCount / 2).ToArray();

                var matches = Enumerable.Zip(teams1, teams2, (t1, t2) =>
                    new Fixture(CupSeason, Date.Value, t1, t2, true, true)).ToList();
                matches.ForEach(m => addFixture(m));

                Phase = TournamentPhase.InProgress;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            PromoteTeam(result.Winner);
            PlayedCount += 1;

            if (PlayedCount == TeamCount / 2)
            {
                Phase = TournamentPhase.Finished;
            }
        }
    }
}
