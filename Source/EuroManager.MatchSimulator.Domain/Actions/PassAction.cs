using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class PassAction : IMatchAction
    {
        private PlayerRatingAdjuster ratingAdjuster = new PlayerRatingAdjuster();

        public bool CanContinue { get; private set; }

        public Player PassingPlayer { get; private set; }

        public Player Receiver { get; private set; }

        public Player Opponent { get; private set; }

        public void Perform(Match match)
        {
            PassingPlayer = match.CurrentPlayer;
            Receiver = PassingPlayer.Team.PickPlayerToReceivePass(PassingPlayer);
            Opponent = PassingPlayer.Team.Opponent.PickPlayerToMarkPassReceiver(Receiver);

            bool isSuccessful = PassingPlayer.TryPass(Receiver, Opponent);
            match.OnPass(Receiver, Opponent, isSuccessful);
            ratingAdjuster.OnPass(PassingPlayer, Receiver, Opponent, isSuccessful);

            CanContinue = isSuccessful;
        }
    }
}
