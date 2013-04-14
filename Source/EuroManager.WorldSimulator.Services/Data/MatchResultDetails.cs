using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class MatchResultDetails
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Team1Id { get; set; }

        public int Team2Id { get; set; }

        public string Team1Name { get; set; }

        public string Team2Name { get; set; }

        public int Score1 { get; set; }

        public int Score2 { get; set; }

        public int PenaltyScore1 { get; set; }

        public int PenaltyScore2 { get; set; }

        public Goal[] Goals1 { get; set; }

        public Goal[] Goals2 { get; set; }

        public PlayerMatchStats[] PlayersStats1 { get; set; }

        public PlayerMatchStats[] PlayersStats2 { get; set; }
    }
}
