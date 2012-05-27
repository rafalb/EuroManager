using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class PenaltyKickEvent : IMatchEvent
    {
        public PenaltyKickEvent(bool isForFirstTeam, int minute, int extended, bool isScored)
        {
            IsForFirstTeam = isForFirstTeam;
            Minute = minute;
            Extended = extended;
            IsScored = isScored;
        }

        public bool IsForFirstTeam { get; private set; }

        public bool IsForSecondTeam
        {
            get { return !IsForFirstTeam; }
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public bool IsScored { get; private set; }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
