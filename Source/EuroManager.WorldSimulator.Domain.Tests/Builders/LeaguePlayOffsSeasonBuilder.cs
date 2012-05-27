using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class LeaguePlayOffsSeasonBuilder
    {
        private DayOfWeek dayOfWeek = DayOfWeek.Saturday;
        private DateTime startDate;
        private DateTime endDate;
        private int pairCount = 4;

        public LeaguePlayOffsSeasonBuilder On(DayOfWeek dayOfWeek)
        {
            this.dayOfWeek = dayOfWeek;
            return this;
        }

        public LeaguePlayOffsSeasonBuilder Starting(DateTime startDate)
        {
            this.startDate = startDate;
            return this;
        }

        public LeaguePlayOffsSeasonBuilder Ending(DateTime endDate)
        {
            this.endDate = endDate;
            return this;
        }

        public LeaguePlayOffsSeasonBuilder WithPairs(int pairCount)
        {
            this.pairCount = pairCount;
            return this;
        }

        public LeaguePlayOffsSeason Build()
        {
            var playOffs = new LeaguePlayOffs(A.NationalLeague.Build(), "Test", dayOfWeek);
            return new LeaguePlayOffsSeason(playOffs, startDate, endDate, CreateTeamPairs(pairCount));
        }

        private IEnumerable<TeamPair> CreateTeamPairs(int count)
        {
            return Enumerable.Zip(A.Team.Repeat(count), A.Team.Repeat(count),
                (t1, t2) => new TeamPair(t1, t2)).ToArray();
        }
    }
}
