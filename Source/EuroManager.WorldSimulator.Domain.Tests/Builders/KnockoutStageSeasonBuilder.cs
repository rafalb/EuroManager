using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class KnockoutStageSeasonBuilder
    {
        private int pairCount;
        private bool isScheduled = false;
        private bool isActivated = false;

        public KnockoutStageSeasonBuilder WithPairCount(int pairCount)
        {
            this.pairCount = pairCount;
            return this;
        }

        public KnockoutStageSeasonBuilder Scheduled()
        {
            this.isScheduled = true;
            return this;
        }

        public KnockoutStageSeasonBuilder Activated()
        {
            this.isActivated = true;
            return this;
        }

        public KnockoutStageSeason Build()
        {
            var stage = new KnockoutStage(pairCount);
            var season = new KnockoutStageSeason(stage);
            season.CupSeason = A.CupSeason.Build();

            if (isScheduled)
            {
                season.ScheduleRoundDates(new DateTime[] { A.Date });
            }

            if (isActivated)
            {
                season.Activate(A.Team.Repeat(pairCount * 2));
            }

            return season;
        }
    }
}
