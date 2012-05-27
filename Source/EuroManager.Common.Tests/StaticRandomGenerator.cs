using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.Common.Tests
{
    public class StaticRandomGenerator : IRandomGenerator
    {
        private Random random = new Random(100);
        private IRandomGenerator generator;

        public StaticRandomGenerator()
        {
            generator = new RandomGenerator(() => random.NextDouble());
        }

        public double Value(double min, double max)
        {
            return generator.Value(min, max);
        }

        public bool Decide(double chance)
        {
            return generator.Decide(chance);
        }

        public IEnumerable<T> Sort<T>(IEnumerable<T> items)
        {
            return generator.Sort(items);
        }

        public T Choose<T>(IEnumerable<T> items, params double[] chances)
        {
            return generator.Choose(items, chances);
        }
    }
}
