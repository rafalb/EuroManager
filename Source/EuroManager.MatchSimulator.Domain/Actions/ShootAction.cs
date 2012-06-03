using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class ShootAction : IMatchAction
    {
        private PlayerRatingAdjuster ratingAdjuster = new PlayerRatingAdjuster();

        public Player Shooter { get; private set; }

        public Player Assistant { get; private set; }

        public Player Opponent { get; private set; }

        public Player Goalkeeper { get; private set; }

        public bool CanContinue
        {
            get { return false; }
        }

        public void Perform(Match match)
        {
            Shooter = match.CurrentPlayer;
            Assistant = match.PreviousPlayer;
            Opponent = Shooter.Team.Opponent.PickPlayerToConfrontShooter(Shooter);
            Goalkeeper = Shooter.Team.Opponent.Goalkeeper;

            ShotResult result = Shooter.TryShoot(Opponent, Goalkeeper);
            match.OnShoot(Opponent, result);
            ratingAdjuster.OnShoot(Shooter, Assistant, Opponent, Goalkeeper, result);
        }
    }
}
