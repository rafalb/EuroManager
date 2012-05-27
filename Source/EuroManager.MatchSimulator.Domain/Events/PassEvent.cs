using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class PassEvent : IMatchEvent
    {
        public PassEvent(int minute, int extended, Player passingPlayer, Player receiver, Player opponent)
        {
            Minute = minute;
            Extended = extended;
            PassingPlayer = passingPlayer;
            Receiver = receiver;
            Opponent = opponent;
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public Player PassingPlayer { get; private set; }

        public Player Receiver { get; private set; }

        public Player Opponent { get; private set; }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
