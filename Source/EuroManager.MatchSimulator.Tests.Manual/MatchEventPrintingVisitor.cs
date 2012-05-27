using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain.Events;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public class MatchEventPrintingVisitor : IMatchEventVisitor
    {
        public void Accept(GoalEvent goalEvent)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("{0} scores a GOAL!!!", goalEvent.Scorer);
            Console.ForegroundColor = originalColor;
        }

        public void Accept(PassEvent passEvent)
        {
            Console.Write("{0} passes to {1} (marked by {2})", passEvent.PassingPlayer, passEvent.Receiver, passEvent.Opponent);
        }

        public void Accept(DribbleEvent dribbleEvent)
        {
            Console.Write("{0} dribbles past {1}", dribbleEvent.Dribbler, dribbleEvent.Opponent);
        }

        public void Accept(ShootEvent shootEvent)
        {
            Console.Write("{0} shoots (confronted by {1})", shootEvent.Shooter, shootEvent.Opponent);
        }

        public void Accept(PenaltyKickEvent penaltyEvent)
        {
            Console.Write("Penalty {0} for team {1}", penaltyEvent.IsScored ? "scored" : "failed", penaltyEvent.IsForFirstTeam ? 1 : 2);
        }
    }
}
