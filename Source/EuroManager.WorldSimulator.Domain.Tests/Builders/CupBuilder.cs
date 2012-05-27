using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class CupBuilder
    {
        private Competition competition;
        private List<CupStage> stages = new List<CupStage>();

        public CupBuilder InCompetition(Competition competition)
        {
            this.competition = competition;
            return this;
        }

        public CupBuilder WithSampleStages()
        {
            stages.Add(new TieKnockoutStage(10));
            stages.Add(new GroupStage(2, 5, 2, isNeutralGround: true, hasReturnRound: false));
            stages.Add(new KnockoutStage(2));
            stages.Add(new KnockoutStage(1));
            return this;
        }

        public CupBuilder WithStage(CupStage stage)
        {
            stages.Add(stage);
            return this;
        }

        public Cup Build()
        {
            if (competition == null)
            {
                competition = A.NationalLeague.InWorld(A.World.Build()).Build();
            }

            return new Cup(competition, "Test", 1, DayOfWeek.Saturday, 2, stages);
        }
    }
}
