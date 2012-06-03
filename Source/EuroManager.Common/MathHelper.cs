using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.Common
{
    public static class MathHelper
    {
        public static double LinearAdjustment(double value, double spread, double factor)
        {
            return value + 2 * (factor - 0.5) * spread;
        }

        public static double ExponentialAdjustment(double value, double spread, double factor)
        {
            factor = 2 * (factor - 0.5);
            return value + Math.Sign(factor) * factor * factor * spread;
        }

        public static double LeftExponentialAdjustment(double value, double spread, double factor)
        {
            return value - (1 - factor) * (1 - factor) * spread;
        }

        public static double RightExponentialAdjustment(double value, double spread, double factor)
        {
            return value + factor * factor * spread;
        }
    }
}
