using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace EuroManager.Common
{
    public class RandomGenerator : IRandomGenerator
    {
        private static Random random = new Random();

        private Func<double> nextDouble;

        static RandomGenerator()
        {
            ResetCurrent();
        }

        public RandomGenerator(Func<double> nextDouble)
        {
            Contract.Requires(nextDouble != null);

            this.nextDouble = nextDouble;
        }

        public static IRandomGenerator Current { get; set; }

        public static void ResetCurrent()
        {
            Current = new RandomGenerator(() => random.NextDouble());
        }

        public double Value(double min, double max)
        {
            return min + nextDouble() * (max - min);
        }

        public bool Decide(double chance)
        {
            return nextDouble() < chance;
        }

        public IEnumerable<T> Sort<T>(IEnumerable<T> items)
        {
            return items.OrderBy(i => nextDouble()).ToArray();
        }

        public T Choose<T>(IEnumerable<T> items, params double[] chances)
        {
            Contract.Requires(items != null && items.Any());
            Contract.Requires(chances != null && chances.Count() == items.Count());

            double total = chances.Sum();
            double choice = nextDouble() * total;
            double current = 0;
            int index = 0;

            foreach (T item in items)
            {
                if (chances[index] > 0)
                {
                    current += chances[index];

                    if (choice < current)
                    {
                        return item;
                    }
                }

                index++;
            }

            return items.Last();
        }
    }
}
