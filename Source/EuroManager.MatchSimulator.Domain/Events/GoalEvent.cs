using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class GoalEvent : IMatchEvent
    {
        public GoalEvent(bool isForFirstTeam, int minute, int extended, Player scorer, Player assistant)
        {
            IsForFirstTeam = isForFirstTeam;
            Minute = minute;
            Extended = extended;
            Scorer = scorer;
            Assistant = assistant;
        }

        public bool IsForFirstTeam { get; private set; }

        public bool IsForSecondTeam
        {
            get { return !IsForFirstTeam; }
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public Player Scorer { get; private set; }

        public Player Assistant { get; private set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Scorer.Name, Minute);
        }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
