using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class League : Tournament
    {
        public League(NationalLeague league, string name, int level, DayOfWeek dayOfWeek, int frequency, bool hasReturnRound = true)
            : base(league, name, level)
        {
            DayOfWeek = dayOfWeek;
            Frequency = frequency;
            HasReturnRound = hasReturnRound;

            AutomaticPromotionCount = 1;
            ConditionalPromotionCount = 1;
            AutomaticRelegationCount = 1;
            ConditionalRelegationCount = 1;
        }

        protected League()
        {
        }

        public int DayOfWeekId { get; private set; }

        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)DayOfWeekId; }
            private set { DayOfWeekId = (int)value; }
        }

        public int Frequency { get; private set; }

        public bool HasReturnRound { get; private set; }

        public int AutomaticPromotionCount { get; set; }

        public int ConditionalPromotionCount { get; set; }

        public int AutomaticRelegationCount { get; set; }

        public int ConditionalRelegationCount { get; set; }
    }
}
