using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Events
{
    public interface IMatchEvent
    {
        int Minute { get; }

        int Extended { get; }

        void Visit(IMatchEventVisitor visitor);
    }
}
