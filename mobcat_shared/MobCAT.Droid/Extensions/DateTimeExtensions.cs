using System;
namespace Microsoft.MobCAT.Droid.Extensions
{
    public static class DateTimeExtensions
    {
        static readonly DateTime _reference = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a DateTime to a long date
        /// </summary>
        /// <returns>The converted long date.</returns>
        /// <param name="dateTime">Date time.</param>
        public static long ToLongDate(this DateTime dateTime)
        {
            var ts = dateTime.AddDays(1) - _reference;

            var date = (long)ts.TotalMilliseconds;

            if (date < 0)
                throw new ArgumentException("Date must be greater than Jan 1, 1970");

            return date;
        }
    }
}
