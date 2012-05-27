using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class GroupStage : CupStage
    {
        public GroupStage(int groupCount, int groupTeamCount, int groupPromotedCount, bool isNeutralGround, bool hasReturnRound)
            : base(groupTeamCount * groupCount, CalculateTotalRoundCount(groupTeamCount, hasReturnRound))
        {
            GroupCount = groupCount;
            GroupTeamCount = groupTeamCount;
            GroupPromotedCount = groupPromotedCount;
            IsNeutralGround = isNeutralGround;
            HasReturnRound = hasReturnRound;
        }

        protected GroupStage()
        {
        }

        public int GroupCount { get; private set; }

        public int GroupTeamCount { get; private set; }

        public int GroupPromotedCount { get; private set; }

        public bool IsNeutralGround { get; private set; }

        public bool HasReturnRound { get; private set; }

        public override CupStageSeason CreateStageSeason()
        {
            return new GroupStageSeason(this);
        }

        private static int CalculateTotalRoundCount(int groupTeamCount, bool hasReturnRound)
        {
            var scheduler = new Scheduler();
            int roundCount = scheduler.CalculateRoundCount(groupTeamCount);

            return hasReturnRound ? 2 * roundCount : roundCount;
        }
    }
}
