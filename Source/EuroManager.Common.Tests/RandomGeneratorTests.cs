using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EuroManager.Common.Tests
{
    [TestFixture]
    public class RandomGeneratorTests : UnitTestFixture
    {
        private const double Delta = 1e-5;

        [Test]
        public void ShouldGiveMiddleValue()
        {
            var generator = new RandomGenerator(() => 0.5);
            double value = generator.Value(3, 7);

            Assert.That(value, Is.InRange(5.0 - Delta, 5.0 + Delta));
        }

        [Test]
        public void ShouldDecidePositively()
        {
            var generator = new RandomGenerator(() => 0.2);
            bool decision = generator.Decide(0.3);

            Assert.That(decision, Is.True);
        }

        [Test]
        public void ShouldDecideNegatively()
        {
            var generator = new RandomGenerator(() => 0.9);
            bool decision = generator.Decide(0.3);

            Assert.That(decision, Is.False);
        }

        [Test]
        public void ShouldSortRandomly()
        {
            int[] items = { 1, 2, 3 };
            var randomPreset = new RandomPreset(0.3, 0.1, 0.8);
            
            var generator = new RandomGenerator(() => randomPreset.Next());
            int[] sortedItems = generator.Sort(items).ToArray();

            int[] expected = { 2, 1, 3 };
            CollectionAssert.AreEqual(expected, sortedItems);
        }

        [Test]
        public void ShouldChooseItemRandomly()
        {
            int[] items = { 1, 2, 3 };
            
            var generator = new RandomGenerator(() => 0.4);
            int chosen = generator.Choose(items, 0.5, 1.0, 0.3);

            Assert.That(chosen, Is.EqualTo(2));
        }

        [Test]
        public void ShouldChooseLastItem()
        {
            int[] items = { 1, 2, 3 };

            var generator = new RandomGenerator(() => 0.99);
            int chosen = generator.Choose(items, 0.5, 1.0, 0.5);

            Assert.That(chosen, Is.EqualTo(3));
        }

        private class RandomPreset
        {
            private double[] values;
            private int currentIndex = -1;

            public RandomPreset(params double[] values)
            {
                this.values = values;
            }

            public double Next()
            {
                currentIndex++;
                return values[currentIndex];
            }
        }
    }
}
