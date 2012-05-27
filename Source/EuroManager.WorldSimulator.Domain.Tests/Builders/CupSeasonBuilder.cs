using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class CupSeasonBuilder
    {
        private Cup cup;
        private List<Team> teams = new List<Team>();

        public CupSeasonBuilder ForCup(Cup cup)
        {
            this.cup = cup;
            return this;
        }

        public CupSeasonBuilder WithTeam(Team team)
        {
            teams.Add(team);
            return this;
        }

        public CupSeason Build()
        {
            if (cup == null)
            {
                cup = A.Cup.WithSampleStages().Build();
            }

            if (!teams.Any())
            {
                teams.AddRange(Enumerable.Repeat(0, cup.Stages[0].TeamCount).Select(x => A.Team.InWorld(cup.World).Build()));
            }

            return new CupSeason(cup, new DateTime(2012, 08, 01), new DateTime(2013, 05, 31), teams);
        }
    }
}
