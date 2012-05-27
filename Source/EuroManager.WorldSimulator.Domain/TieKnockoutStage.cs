using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class TieKnockoutStage : CupStage
    {
        public TieKnockoutStage(int tieCount)
            : base(checked(2 * tieCount), 2)
        {
        }

        protected TieKnockoutStage()
        {
        }

        public override CupStageSeason CreateStageSeason()
        {
            return new TieKnockoutStageSeason(this);
        }
    }
}
