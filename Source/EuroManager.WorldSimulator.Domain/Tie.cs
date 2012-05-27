using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class Tie : IEntity
    {
        public Tie(DateTime firstLegDate, DateTime secondLegDate, Team team1, Team team2)
        {
            FirstLegDate = firstLegDate;
            SecondLegDate = secondLegDate;
            Team1 = team1;
            Team2 = team2;
        }

        protected Tie()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public DateTime FirstLegDate { get; private set; }

        public DateTime SecondLegDate { get; private set; }

        public int? Team1Id { get; private set; }

        public virtual Team Team1 { get; private set; }

        public int? Team2Id { get; private set; }

        public virtual Team Team2 { get; private set; }

        public int? FirstLegResultId { get; private set; }

        public virtual MatchResult FirstLegResult { get; private set; }

        public int? SecondLegResultId { get; private set; }

        public virtual MatchResult SecondLegResult { get; private set; }

        public int? WinnerId { get; private set; }

        public virtual Team Winner { get; private set; }

        public int? LoserId { get; private set; }

        public virtual Team Loser { get; private set; }

        public bool HasWinner
        {
            get { return Winner != null; }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Team1.Name, Team2.Name);
        }

        public Fixture[] CreateFixtures(TournamentSeason tournamentSeason)
        {
            var firstLeg = new Fixture(tournamentSeason, FirstLegDate, Team1, Team2, false, false);
            var secondLeg = new Fixture(SecondLegDate, firstLeg);

            return new Fixture[] { firstLeg, secondLeg };
        }

        public bool IsMatchingResult(MatchResult result)
        {
            return
                (FirstLegResult == null && result.Date == FirstLegDate &&
                    result.Team1 == Team1 && result.Team2 == Team2) ||
                (FirstLegResult != null && SecondLegResult == null && result.Date == SecondLegDate &&
                    result.Team1 == Team2 && result.Team2 == Team1);
        }

        public void AddResult(MatchResult result)
        {
            if (IsMatchingResult(result))
            {
                if (result.Date == FirstLegDate)
                {
                    FirstLegResult = result;
                }
                else
                {
                    SecondLegResult = result;
                    Winner = result.Winner;
                    Loser = Winner == Team1 ? Team2 : Team1;
                }
            }
        }
    }
}
