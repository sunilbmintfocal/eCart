using System;
using System.Globalization;
using System.Linq;

namespace MintCart.Common
{
    public class DateTimeConversionHelper
    {
        public static DateTime GetCurrentUTCTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime ConvertUTCToLocalTime(DateTime? datetime)
        {
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == double.Parse("180")).FirstOrDefault(); // Saudi Offset minutes UTC+3. 3 hours * 60 minutes/hour
            var localtime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo);
            return localtime;
        }

        public static DateTime ConvertLocalToUTCTime(DateTime? datetime)
        {
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(datetime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        public static DateTime GetStartDateTime(DateTime? datetime)
        {
            //get Local Time
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == double.Parse("180")).FirstOrDefault(); // Saudi Offset minutes UTC+3. 3 hours * 60 minutes/hour
            var localtime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;

            //convert the local date back to utc date
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(startDateTime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        public static DateTime GetEndDateTime(DateTime? datetime)
        {
            //get Local Time
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == double.Parse("180")).FirstOrDefault(); // Saudi Offset minutes UTC+3. 3 hours * 60 minutes/hour
            var localtime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;
            DateTime endDateTime = startDateTime.AddDays(1).AddSeconds(-1);

            //convert the local date back to utc date
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(endDateTime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        public static DateTime RoundDownToNearestHour(DateTime? datetime)
        {
            return new DateTime(datetime.Value.Year, datetime.Value.Month, datetime.Value.Day, datetime.Value.Hour, 0, 0, 0, datetime.Value.Kind);
        }

        public static TimeSpan GetTimeIntervalBetweenTwoDates(DateTime date1, DateTime date2)
        {
            return date1 - date2;
        }

        public static DateTime AddHoursToUTCTime(int hours)
        {
            return DateTime.UtcNow.AddHours(hours);
        }
    }
}
