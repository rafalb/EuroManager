using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class PassAction : IMatchAction
    {
        public bool CanContinue { get; private set; }

        public void Perform(Match match)
        {
            Player passingPlayer = match.CurrentPlayer;
            Player receiver = passingPlayer.Team.PickPlayerToReceivePass(passingPlayer);
            Player opponent = passingPlayer.Team.Opponent.PickPlayerToMarkPassReceiver(receiver);

            bool isSuccessful = passingPlayer.TryPass(receiver, opponent);
            match.OnPass(receiver, opponent, isSuccessful);

            CanContinue = isSuccessful;
        }
    }
}
