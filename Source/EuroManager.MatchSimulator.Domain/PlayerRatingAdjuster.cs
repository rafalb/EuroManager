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
                dribbler.AdjustRating(Adjustment(0.020, 0.005));
                opponent.AdjustRating(-Adjustment(0.020, 0.005));
            }
            else
            {
                dribbler.AdjustRating(-Adjustment(0.008, 0.002));
                opponent.AdjustRating(Adjustment(0.012, 0.004));
            }
        }

        public void OnPass(Player passingPlayer, Player receiver, Player opponent, bool isSuccessful)
        {
            if (isSuccessful)
            {
                passingPlayer.AdjustRating(Adjustment(0.010, 0.003));
                receiver.AdjustRating(Adjustment(0.005, 0.002));
                opponent.AdjustRating(-Adjustment(0.010, 0.005));
            }
            else
            {
                passingPlayer.AdjustRating(-Adjustment(0.006, 0.002));
                receiver.AdjustRating(-Adjustment(0.003, 0.002));
                opponent.AdjustRating(Adjustment(0.008, 0.004));
            }
        }

        private double Adjustment(double value, double spread)
        {
            return random.Value(value - spread, value + spread);
        }
    }
}
