using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class NullAction : IMatchAction
    {
        public bool CanContinue
        {
            get { return false; }
        }

        public void Perform(Match match)
        {
        }
    }
}
