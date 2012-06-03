using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Actions
{
    public class ShootAction : IMatchAction
    {
        public bool CanContinue
        {
            get { return false; }
        }

        public void Perform(Match match)
        {
            Player shooter = match.CurrentPlayer;
            Player opponent = shooter.Team.Opponent.PickPlayerToConfrontShooter(shooter);

            ShotResult result = shooter.TryShoot(opponent, shooter.Team.Opponent.Goalkeeper);
            match.OnShoot(opponent, isSuccessful: result == ShotResult.Scored);
        }
    }
}
