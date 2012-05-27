using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public interface IMatchAction
    {
        bool CanContinue { get; }

        void Perform(Match match);
    }
}
