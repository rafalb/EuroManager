using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class ShootEvent : IMatchEvent
    {
        public ShootEvent(int minute, int extended, Player shooter, Player opponent)
        {
            Minute = minute;
            Extended = extended;
            Shooter = shooter;
            Opponent = opponent;
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public Player Shooter { get; private set; }

        public Player Opponent { get; private set; }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
