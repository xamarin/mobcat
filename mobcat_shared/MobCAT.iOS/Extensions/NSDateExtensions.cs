using System;
using Foundation;

namespace Microsoft.MobCAT.iOS.Extensions
{
    public static class NSDateExtensions
    {
        static DateTime _reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a NSDate to a DateTime
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="date">Date.</param>
        public static DateTime ToDateTime(this NSDate date)
        {
            var utcDateTime = _reference.AddSeconds(date.SecondsSinceReferenceDate);
            var dateTime = utcDateTime.ToLocalTime();
            return dateTime;
        }

        /// <summary>
        /// Converts a DateTime to a NSDate
        /// </summary>
        /// <returns>The NSDate.</returns>
        /// <param name="datetime">Datetime.</param>
        public static NSDate ToNSDate(this DateTime datetime)
        {
            var utcDateTime = datetime.ToUniversalTime();
            var date = NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - _reference).TotalSeconds);
            return date;
        }
    }
}
