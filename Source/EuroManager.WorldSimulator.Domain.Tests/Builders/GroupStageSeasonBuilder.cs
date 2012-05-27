using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class GroupStageSeasonBuilder
    {
        private GroupStage groupStage;
        private int groupCount;
        private int groupSize;
        private bool hasReturnRound = false;
        private bool isActivated = false;
        private bool isScheduled = false;

        public GroupStageSeasonBuilder WithGroups(int groupCount, int groupSize, bool hasReturnRound)
        {
            this.groupCount = groupCount;
            this.groupSize = groupSize;
            this.hasReturnRound = hasReturnRound;
            return this;
        }

        public GroupStageSeasonBuilder Scheduled()
        {
            isScheduled = true;
            return this;
        }

        public GroupStageSeasonBuilder Activated()
        {
            isActivated = true;
            return this;
        }

        public GroupStageSeason Build()
        {
            if (groupStage == null)
            {
                groupStage = new GroupStage(groupCount, groupSize, 2, isNeutralGround: false, hasReturnRound: hasReturnRound);
            }

            var season = (GroupStageSeason)groupStage.CreateStageSeason();
            season.CupSeason = A.CupSeason.Build();

            if (isScheduled)
            {
                var roundDates = Enumerable.Range(0, groupStage.RoundCount).Select(i => new DateTime(2012, 10, 15).AddDays(7 * i)).ToArray();
                season.ScheduleRoundDates(roundDates);
            }

            if (isActivated)
            {
                var teams = Enumerable.Repeat(0, groupStage.TeamCount).Select(x => A.Team.Build()).ToArray();
                season.Activate(teams);
            }

            return season;
        }
    }
}
