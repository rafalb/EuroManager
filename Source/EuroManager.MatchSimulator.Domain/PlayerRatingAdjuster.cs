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
                AdjustExponentiallyRight(dribbler, 0.015, 0.015);
                AdjustExponentially(opponent, -0.020, 0.010);
            }
            else
            {
                AdjustLinearly(dribbler, -0.008, 0.002);
                AdjustLinearly(opponent, 0.012, 0.003);
            }
        }

        public void OnPass(Player passingPlayer, Player receiver, Player opponent, bool isSuccessful)
        {
            if (isSuccessful)
            {
                AdjustExponentiallyRight(passingPlayer, 0.007, 0.023);
                AdjustExponentiallyRight(receiver, 0.003, 0.012);
                AdjustLinearly(opponent, -0.008, 0.004);
            }
            else
            {
                AdjustLinearly(passingPlayer, -0.006, 0.002);
                AdjustExponentially(receiver, -0.003, 0.002);
                AdjustLinearly(opponent, 0.008, 0.004);
            }
        }

        private void AdjustLinearly(Player player, double value, double spread)
        {
            double adjustment = random.Value(value - spread, value + spread);
            player.AdjustRating(adjustment);
        }

        private void AdjustExponentially(Player player, double value, double spread)
        {
            double factor = random.Value(-1, 1);
            double adjustment = value + (Math.Sign(factor) * factor * factor) * spread;
            player.AdjustRating(adjustment);
        }

        private void AdjustExponentiallyLeft(Player player, double value, double leftSpread)
        {
            double factor = random.Value(0, 1);
            double adjustment = value - (factor * factor) * leftSpread;
            player.AdjustRating(adjustment);
        }

        private void AdjustExponentiallyRight(Player player, double value, double rightSpread)
        {
            double factor = random.Value(0, 1);
            double adjustment = value + (factor * factor) * rightSpread;
            player.AdjustRating(adjustment);
        }
    }
}
