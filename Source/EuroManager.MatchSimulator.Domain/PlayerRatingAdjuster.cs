using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.MatchSimulator.Domain
{
    public class PlayerRatingAdjuster
    {
        private IRandomGenerator random = RandomGenerator.Current;

        public void OnDribble(Player dribbler, Player opponent, bool isSuccessful)
        {
            if (isSuccessful)
            {
                AdjustExponentiallyRight(dribbler, 0.010, 0.010);
                AdjustLinearly(opponent, -0.015, 0.005);
            }
            else
            {
                AdjustLinearly(dribbler, -0.015, 0.005);
                AdjustExponentially(opponent, 0.020, 0.010);
            }
        }

        public void OnPass(Player passingPlayer, Player receiver, Player opponent, bool isSuccessful)
        {
            if (isSuccessful)
            {
                AdjustExponentiallyRight(passingPlayer, 0.000, 0.020);
                AdjustExponentiallyRight(receiver, 0.000, 0.005);
                AdjustExponentiallyLeft(opponent, 0.000, 0.010);
            }
            else
            {
                AdjustLinearly(passingPlayer, -0.010, 0.005);
                AdjustExponentiallyLeft(receiver, 0.000, 0.005);
                AdjustExponentially(opponent, 0.020, 0.010);
            }
        }

        public void OnShoot(Player shooter, Player assistant, Player opponent, Player goalkeeper, ShotResult result)
        {
            if (result == ShotResult.Scored)
            {
                AdjustLinearly(shooter, 0.060, 0.020);
                AdjustExponentiallyRight(assistant, 0.030, 0.050);
                AdjustExponentiallyLeft(opponent, -0.030, 0.050);
                AdjustExponentiallyLeft(goalkeeper, -0.020, 0.060);

                foreach (var player in shooter.Team.Squad)
                {
                    AdjustLinearly(player, 0.025, 0.005);
                }

                foreach (var player in opponent.Team.Squad)
                {
                    AdjustLinearly(player, -0.025, 0.005);
                }
            }
            else if (result == ShotResult.Blocked)
            {
                AdjustLinearly(shooter, -0.010, 0.005);
                AdjustExponentiallyRight(opponent, 0.030, 0.030);
            }
            else if (result == ShotResult.Missed)
            {
                AdjustExponentiallyLeft(shooter, 0.010, 0.050);
                AdjustExponentially(opponent, 0.010, 0.020);
            }
            else if (result == ShotResult.Saved)
            {
                AdjustExponentially(shooter, 0.000, 0.030);
                AdjustExponentially(opponent, 0.000, 0.030);
                AdjustExponentiallyRight(goalkeeper, 0.040, 0.060);
            }
            else
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Shot result {0} not supported", result));
            }
        }

        private void AdjustLinearly(Player player, double value, double spread)
        {
            if (player != null)
            {
                double adjustment = MathHelper.LinearAdjustment(value, spread, random.Value(0, 1));
                player.AdjustRating(adjustment);
            }
        }

        private void AdjustExponentially(Player player, double value, double spread)
        {
            if (player != null)
            {
                double adjustment = MathHelper.ExponentialAdjustment(value, spread, random.Value(0, 1));
                player.AdjustRating(adjustment);
            }
        }

        private void AdjustExponentiallyLeft(Player player, double value, double spread)
        {
            if (player != null)
            {
                double adjustment = MathHelper.LeftExponentialAdjustment(value, spread, random.Value(0, 1));
                player.AdjustRating(adjustment);
            }
        }

        private void AdjustExponentiallyRight(Player player, double value, double spread)
        {
            if (player != null)
            {
                double adjustment = MathHelper.RightExponentialAdjustment(value, spread, random.Value(0, 1));
                player.AdjustRating(adjustment);
            }
        }
    }
}
