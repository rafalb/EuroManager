using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class MatchResult : IEntity
    {
        public MatchResult(Fixture fixture, Team winner, int score1, int score2, int penaltyScore1, int penaltyScore2, IEnumerable<Goal> goals)
        {
            TournamentSeasonId = fixture.TournamentSeasonId;
            Date = fixture.Date;
            Team1 = fixture.Team1;
            Team2 = fixture.Team2;
            Team1Name = fixture.Team1Name;
            Team2Name = fixture.Team2Name;
            Winner = winner;
            Score1 = score1;
            Score2 = score2;
            PenaltyScore1 = penaltyScore1;
            PenaltyScore2 = penaltyScore2;
            Goals = goals.ToList();
        }

        protected MatchResult()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int TournamentSeasonId { get; private set; }

        public DateTime Date { get; private set; }

        public int? Team1Id { get; private set; }

        public virtual Team Team1 { get; private set; }

        [StringLength(100)]
        public string Team1Name { get; private set; }

        public int? Team2Id { get; private set; }

        public virtual Team Team2 { get; private set; }

        [StringLength(100)]
        public string Team2Name { get; private set; }

        public int? WinnerId { get; private set; }

        public virtual Team Winner { get; private set; }

        public int Score1 { get; private set; }

        public int Score2 { get; private set; }

        public int PenaltyScore1 { get; private set; }

        public int PenaltyScore2 { get; private set; }

        public virtual List<Goal> Goals { get; private set; }

        public IEnumerable<Goal> Goals1
        {
            get { return Goals.Where(g => g.IsForFirstTeam).ToArray(); }
        }

        public IEnumerable<Goal> Goals2
        {
            get { return Goals.Where(g => g.IsForSecondTeam).ToArray(); }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1} {2}:{3}", Team1.Name, Team2.Name, Score1, Score2);
        }
    }
}
