using System;
using System.Collections.Generic;

namespace EuroManager.Common
{
    public interface IRandomGenerator
    {
        double Value(double min, double max);

        bool Decide(double chance);

        IEnumerable<T> Sort<T>(IEnumerable<T> items);

        T Choose<T>(IEnumerable<T> items, params double[] chances);
    }
}
