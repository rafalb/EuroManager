using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class TieKnockoutStageSeason : CupStageSeason
    {
        private IRandomGenerator randomGenerator = RandomGenerator.Current;

        public TieKnockoutStageSeason(TieKnockoutStage stage)
            : base(stage)
        {
            Ties = new List<Tie>();
        }

        protected TieKnockoutStageSeason()
        {
        }

        public DateTime? FirstLegDate { get; private set; }

        public DateTime? SecondLegDate { get; private set; }

        public virtual List<Tie> Ties { get; private set; }

        public override void ScheduleRoundDates(IEnumerable<DateTime> roundDates)
        {
            FirstLegDate = roundDates.ElementAt(0);
            SecondLegDate = roundDates.ElementAt(1);
        }

        public override void Activate(IEnumerable<Team> teams)
        {
            Teams.AddRange(teams);
            var ties = CreateTies(FirstLegDate.Value, SecondLegDate.Value, teams);
            Ties.AddRange(ties);
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (Phase == TournamentPhase.NotStarted)
            {
                foreach (var tie in Ties)
                {
                    var fixtures = tie.CreateFixtures(CupSeason);
                    addFixture(fixtures[0]);
                    addFixture(fixtures[1]);
                }

                Phase = TournamentPhase.InProgress;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            var tie = Ties.First(t => t.IsMatchingResult(result));
            tie.AddResult(result);

            if (tie.HasWinner)
            {
                PromoteTeam(tie.Winner);

                if (Ties.All(t => t.HasWinner))
                {
                    Phase = TournamentPhase.Finished;
                }
            }
        }

        private List<Tie> CreateTies(DateTime firstLegDate, DateTime secondLegDate, IEnumerable<Team> teams)
        {
            teams = randomGenerator.Sort(teams);

            var teams1 = teams.Take(TeamCount / 2).ToArray();
            var teams2 = teams.Skip(TeamCount / 2).Take(TeamCount / 2).ToArray();

            var ties = Enumerable.Zip(teams1, teams2, (t1, t2) =>
                new Tie(firstLegDate, secondLegDate, t1, t2)).ToList();
            return ties;
        }
    }
}
