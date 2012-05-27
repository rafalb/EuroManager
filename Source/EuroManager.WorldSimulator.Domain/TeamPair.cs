using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class TeamPair
    {
        public TeamPair(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }

        public Team Team1 { get; private set; }

        public Team Team2 { get; private set; }
    }
}
