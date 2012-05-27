using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class LeaguePlayOffsSeason : TournamentSeason
    {
        public LeaguePlayOffsSeason(LeaguePlayOffs playOffs, DateTime startDate, DateTime endDate, IEnumerable<TeamPair> teamPairs)
            : base(playOffs, startDate, endDate)
        {
            DateTime firstLegDate = startDate.Next(playOffs.DayOfWeek);
            DateTime secondLegDate = firstLegDate.AddDays(7);
            Ties = teamPairs.Select(p => new Tie(firstLegDate, secondLegDate, p.Team1, p.Team2)).ToList();

            var teams = Enumerable.Union(teamPairs.Select(p => p.Team1).ToArray(), teamPairs.Select(p => p.Team2).ToArray()).ToList();
            AddTeams(teams);
        }

        protected LeaguePlayOffsSeason()
        {
        }

        public virtual List<Tie> Ties { get; private set; }

        private LeaguePlayOffs PlayOffs
        {
            get { return (LeaguePlayOffs)Tournament; }
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (Phase == TournamentPhase.NotStarted)
            {
                foreach (var tie in Ties)
                {
                    var fixtures = tie.CreateFixtures(this);
                    addFixture(fixtures[0]);
                    addFixture(fixtures[1]);
                }

                Phase = TournamentPhase.InProgress;
                NextSchedulingDate = null;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            var tie = Ties.First(t => t.IsMatchingResult(result));
            tie.AddResult(result);

            if (Ties.All(t => t.HasWinner))
            {
                Phase = TournamentPhase.Finished;
            }
        }
    }
}
