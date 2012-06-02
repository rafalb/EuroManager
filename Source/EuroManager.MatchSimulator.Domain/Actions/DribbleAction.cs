using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class DribbleAction : IMatchAction
    {
        private PlayerRatingAdjuster ratingAdjuster = new PlayerRatingAdjuster();

        public bool CanContinue { get; private set; }

        public Player Dribbler { get; private set; }

        public Player Opponent { get; private set; }

        public void Perform(Match match)
        {
            Dribbler = match.CurrentPlayer;
            Opponent = Dribbler.Team.Opponent.PickPlayerToConfrontDribbler(Dribbler);

            bool isSuccessful = Dribbler.TryDribble(Opponent);
            match.OnDribble(Opponent, isSuccessful);
            ratingAdjuster.OnDribble(Dribbler, Opponent, isSuccessful);

            CanContinue = isSuccessful;
        }
    }
}
