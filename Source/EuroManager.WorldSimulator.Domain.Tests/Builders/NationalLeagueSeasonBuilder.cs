using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class NationalLeagueSeasonBuilder
    {
        private int divisionCount = 3;

        public NationalLeagueSeasonBuilder WithDivisions(int divisionCount)
        {
            this.divisionCount = divisionCount;
            return this;
        }

        public NationalLeagueSeason Build()
        {
            var season = new NationalLeagueSeason(A.NationalLeague.WithDivisions(divisionCount).Build(), A.Date, A.Date.AddYears(1));

            foreach (var division in season.League.Divisions)
            {
                season.AddDivisionSeason(A.LeagueSeason.ForLeague((League)division).Build());
            }

            return season;
        }
    }
}
