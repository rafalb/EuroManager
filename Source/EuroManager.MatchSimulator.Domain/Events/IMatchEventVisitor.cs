using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public interface IMatchEventVisitor
    {
        void Accept(GoalEvent goalEvent);

        void Accept(PassEvent passEvent);

        void Accept(DribbleEvent dribbleEvent);

        void Accept(ShootEvent shootEvent);

        void Accept(PenaltyKickEvent penaltyEvent);
    }
}
