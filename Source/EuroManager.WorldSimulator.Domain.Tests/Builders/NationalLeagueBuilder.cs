using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class NationalLeagueBuilder
    {
        private World world;
        private int divisionCount;

        public NationalLeagueBuilder InWorld(World world)
        {
            this.world = world;
            return this;
        }

        public NationalLeagueBuilder WithDivisions(int divisionCount)
        {
            this.divisionCount = divisionCount;
            return this;
        }

        public NationalLeague Build()
        {
            if (world == null)
            {
                world = A.World.Build();
            }

            var nationalLeague = new NationalLeague(world, "Test");

            if (divisionCount > 0)
            {
                var divisions = A.League.ForNationalLeague(nationalLeague).Repeat(divisionCount);

                foreach (var division in divisions)
                {
                    nationalLeague.AddDivision(division);
                }
            }

            nationalLeague.PlayOffs = new LeaguePlayOffs(nationalLeague, "Test", DayOfWeek.Wednesday);

            return nationalLeague;
        }
    }
}
