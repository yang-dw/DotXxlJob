using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    internal static class DateTimeExtensions
    {
        private const long Era = 62135596800000L;
        private const long Millis = 60000;

        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTotalMilliseconds(this DateTime dt)
        {
            return dt.ToUniversalTime().Ticks / 10000 - Era;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetTotalMinutes(this DateTime dt)
        {
            var val = GetTotalMilliseconds(dt);
            return (int)(val / Millis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FromMinutes(this int value)
        {
            var ticks = (value * Millis + Era) * 10000;
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FromMilliseconds(this long value)
        {
            var ticks = (value + Era) * 10000;
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static DateTime FromMillis(long millis)
        {
            return Jan1st1970.AddMilliseconds(millis);
        }
    }
}
