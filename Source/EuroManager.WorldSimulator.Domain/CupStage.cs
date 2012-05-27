using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class CupStage : IEntity
    {
        protected CupStage(int teamCount, int roundCount)
        {
            TeamCount = teamCount;
            RoundCount = roundCount;
        }

        protected CupStage()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int TeamCount { get; private set; }

        public int RoundCount { get; private set; }

        public abstract CupStageSeason CreateStageSeason();
    }
}
