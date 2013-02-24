using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class Cup : Tournament
    {
        public Cup(Competition competition, string name, int level, DayOfWeek dayOfWeek, int frequency, IEnumerable<CupStage> stages)
            : base(competition, name, level)
        {
            DayOfWeek = dayOfWeek;
            Frequency = frequency;

            Stages = stages.ToList();

            for (int i = 0; i < Stages.Count; i++)
            {
                Stages[i].StageNumber = i + 1;
            }
        }

        protected Cup()
        {
        }

        public DayOfWeek DayOfWeek { get; private set; }

        public int Frequency { get; private set; }

        public virtual List<CupStage> Stages { get; private set; }

        public IEnumerable<CupStage> StagesOrdered
        {
            get { return Stages.OrderBy(s => s.StageNumber); }
        }
    }
}
