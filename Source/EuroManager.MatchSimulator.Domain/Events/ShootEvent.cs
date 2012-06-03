using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public class ShootEvent : IMatchEvent
    {
        public ShootEvent(int minute, int extended, Player shooter, Player opponent, ShotResult result)
        {
            Minute = minute;
            Extended = extended;
            Shooter = shooter;
            Opponent = opponent;
            Result = result;
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public Player Shooter { get; private set; }

        public Player Opponent { get; private set; }

        public ShotResult Result { get; private set; }

        public void Visit(IMatchEventVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
