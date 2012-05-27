using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class KnockoutStage : CupStage
    {
        public KnockoutStage(int matchCount)
            : base(checked(2 * matchCount), 1)
        {
        }

        protected KnockoutStage()
        {
        }

        public override CupStageSeason CreateStageSeason()
        {
            return new KnockoutStageSeason(this);
        }
    }
}
