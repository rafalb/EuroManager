using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.Common
{
    public static class ObjectExtensions
    {
        public static bool In(this object value, params object[] items)
        {
            return items.Contains(value);
        }

        public static IEnumerable<T> AsEnumerable<T>(this T value)
        {
            return new T[] { value };
        }
    }
}
