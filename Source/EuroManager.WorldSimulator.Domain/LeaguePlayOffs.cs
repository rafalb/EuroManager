using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class LeaguePlayOffs : Tournament
    {
        public LeaguePlayOffs(NationalLeague league, string name, DayOfWeek dayOfWeek)
            : base(league, name, 0)
        {
            DayOfWeek = dayOfWeek;
        }

        protected LeaguePlayOffs()
        {
        }

        public DayOfWeek DayOfWeek { get; private set; }
    }
}
