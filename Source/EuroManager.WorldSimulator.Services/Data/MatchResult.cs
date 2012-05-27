using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class MatchResult
    {
        public DateTime Date { get; set; }

        public string Team1Name { get; set; }

        public string Team2Name { get; set; }

        public int Score1 { get; set; }

        public int Score2 { get; set; }

        public int PenaltyScore1 { get; set; }

        public int PenaltyScore2 { get; set; }

        public Goal[] Goals1 { get; set; }

        public Goal[] Goals2 { get; set; }
    }
}
