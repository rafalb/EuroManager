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
        }

        protected Cup()
        {
        }

        public int DayOfWeekId { get; private set; }

        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)DayOfWeekId; }
            private set { DayOfWeekId = (int)value; }
        }

        public int Frequency { get; private set; }

        public virtual List<CupStage> Stages { get; private set; }
    }
}
