using System;
using System.Collections.Generic;
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
                AdjustExponentiallyRight(dribbler, 0.010, 0.015);
                AdjustExponentially(opponent, -0.020, 0.010);
            }
            else
            {
                AdjustLinearly(dribbler, -0.008, 0.002);
                AdjustExponentiallyRight(opponent, 0.015, 0.025);
            }
        }

        public void OnPass(Player passingPlayer, Player receiver, Player opponent, bool isSuccessful)
        {
            if (isSuccessful)
            {
                AdjustExponentiallyRight(passingPlayer, 0.003, 0.012);
                AdjustExponentiallyRight(receiver, 0.002, 0.008);
                AdjustLinearly(opponent, -0.008, 0.004);
            }
            else
            {
                AdjustLinearly(passingPlayer, -0.007, 0.003);
                AdjustExponentially(receiver, -0.003, 0.003);
                AdjustExponentiallyRight(opponent, 0.015, 0.025);
            }
        }

        public void OnShoot(Player shooter, Player assistant, Player opponent, Player goalkeeper, ShotResult result)
        {
            if (result == ShotResult.Scored)
            {
                AdjustLinearly(shooter, 0.080, 0.030);
                AdjustExponentiallyRight(assistant, 0.040, 0.080);
                AdjustExponentiallyLeft(opponent, -0.030, 0.070);
                AdjustExponentiallyLeft(goalkeeper, -0.040, 0.120);
            }
            else if (result == ShotResult.Blocked)
            {
                AdjustExponentiallyLeft(shooter, -0.020, 0.080);
                AdjustExponentiallyRight(opponent, 0.030, 0.040);
            }
            else if (result == ShotResult.Missed)
            {
                AdjustExponentiallyLeft(shooter, -0.030, 0.070);
                AdjustExponentiallyRight(opponent, 0.000, 0.050);
            }
            else if (result == ShotResult.Saved)
            {
                AdjustExponentiallyLeft(shooter, 0.020, 0.100);
                AdjustExponentiallyRight(opponent, 0.000, 0.050);
                AdjustExponentially(goalkeeper, 0.070, 0.060);
            }
            else
            {
                throw new InvalidOperationException(String.Format("Shot result {0} not supported", result));
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
