using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class DribbleEvent : IMatchEvent
    {
        public DribbleEvent(int minute, int extended, Player dribbler, Player opponent)
        {
            Minute = minute;
            Extended = extended;
            Dribbler = dribbler;
            Opponent = opponent;
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public Player Dribbler { get; private set; }

        public Player Opponent { get; private set; }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
