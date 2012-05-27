using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EuroManager.Common.Tests
{
    [TestFixture]
    public class UnitTestFixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            RandomGenerator.Current = new StaticRandomGenerator();
        }

        [TearDown]
        public virtual void TearDown()
        {
            RandomGenerator.ResetCurrent();
        }
    }
}
