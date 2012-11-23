using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class LeagueBuilder
    {
        private NationalLeague nationalLeague;
        private int level = 1;
        private bool hasReturnRound = true;

        public LeagueBuilder ForNationalLeague(NationalLeague nationalLeague)
        {
            this.nationalLeague = nationalLeague;
            return this;
        }

        public LeagueBuilder AtLevel(int level)
        {
            this.level = level;
            return this;
        }

        public LeagueBuilder WithReturnRound(bool hasReturnRound)
        {
            this.hasReturnRound = hasReturnRound;
            return this;
        }

        public League Build()
        {
            if (nationalLeague == null)
            {
                nationalLeague = A.NationalLeague.Build();
            }

            return new League(nationalLeague, "Test", level, DayOfWeek.Saturday, 1, hasReturnRound);
        }

        public IEnumerable<League> Repeat(int count)
        {
            return Enumerable.Range(1, count).Select(i => AtLevel(i).Build()).ToArray();
        }
    }
}
