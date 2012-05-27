using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class LeagueSeasonBuilder
    {
        private League league;

        public LeagueSeasonBuilder ForLeague(League league)
        {
            this.league = league;
            return this;
        }

        public LeagueSeason Build()
        {
            if (league == null)
            {
                league = A.League.Build();
            }

            var leagueSeason = new LeagueSeason(league, A.Date, A.Date.AddMonths(9), A.Team.Repeat(18));

            return leagueSeason;
        }

        public IEnumerable<LeagueSeason> Repeat(int count)
        {
            return Enumerable.Range(0, count).Select(x => Build()).ToArray();
        }
    }
}
