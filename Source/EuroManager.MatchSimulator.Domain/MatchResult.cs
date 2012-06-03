using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain.Events;

namespace EuroManager.MatchSimulator.Domain
{
    public class MatchResult
    {
        public MatchResult(Team team1, Team team2, Team winner, int score1, int score2, int penaltyScore1, int penaltyScore2,
            IEnumerable<IMatchEvent> events)
        {
            Team1 = team1;
            Team2 = team2;
            Winner = winner;
            Score1 = score1;
            Score2 = score2;
            PenaltyScore1 = penaltyScore1;
            PenaltyScore2 = penaltyScore2;
            Events = events;
        }

        public Team Team1 { get; private set; }

        public Team Team2 { get; private set; }

        public Team Winner { get; private set; }

        public int Score1 { get; private set; }

        public int Score2 { get; private set; }

        public int PenaltyScore1 { get; set; }

        public int PenaltyScore2 { get; set; }

        public IEnumerable<IMatchEvent> Events { get; private set; }

        public IEnumerable<GoalEvent> Goals
        {
            get { return Events.OfType<GoalEvent>().ToArray(); }
        }
    }
}
