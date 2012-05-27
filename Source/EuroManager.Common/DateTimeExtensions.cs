using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.Common
{
    public static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime date, DateTime other)
        {
            return date.Date == other.Date;
        }

        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1).Date;
            }

            return date.Date;
        }
    }
}
