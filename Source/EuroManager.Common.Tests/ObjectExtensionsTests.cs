using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EuroManager.Common.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests : UnitTestFixture
    {
        [Test]
        public void ShouldNotBePresentInEmptyList()
        {
            int[] items = { };

            Assert.That(5.In(items), Is.False);
        }

        [Test]
        public void ShouldDetectPresenceInList()
        {
            var obj = new object();
            object[] items = { new object(), obj, new object() };

            Assert.That(obj.In(items), Is.True);
        }

        [Test]
        public void ShouldDetectItemLackingInList()
        {
            var obj = new object();
            object[] items = { new object(), new object() };

            Assert.That(obj.In(items), Is.False);
        }

        [Test]
        public void ShouldCreateEnumerableFromItem()
        {
            var obj = new object();
            var items = obj.AsEnumerable();

            Assert.That(items.Single(), Is.EqualTo(obj));
        }
    }
}
