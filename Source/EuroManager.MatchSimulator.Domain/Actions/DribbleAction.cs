using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class DribbleAction : IMatchAction
    {
        public bool CanContinue { get; private set; }

        public void Perform(Match match)
        {
            Player dribbler = match.CurrentPlayer;
            Player opponent = dribbler.Team.Opponent.PickPlayerToConfrontDribbler(dribbler);

            bool isSuccessful = dribbler.TryDribble(opponent);
            match.OnDribble(opponent, isSuccessful);

            CanContinue = isSuccessful;
        }
    }
}
