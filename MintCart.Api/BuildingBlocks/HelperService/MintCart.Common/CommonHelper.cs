using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace MintCart.Common
{
    public static class CommonHelper
    {
        #region Strings
        public static string Truncate(string value, int length)
        {
            if (value != null && value.Length > length)
            {
                return value[..length];
            }

            return value ?? default!;
        }
        public static string ListToString(IEnumerable<string> input)
        {
            return string.Join(",", input);
        }

        public static SecureString ToSecureString(string passwordString)
        {
            var secureString = new SecureString();
            foreach (Char c in passwordString)
                secureString.AppendChar(c);

            return secureString;
        }
        #endregion

        #region Token
        /// <summary>
        /// Returns back the Current Context Token
        /// </summary>
        public static string GetCurrentContextToken(HttpContext _httpContext)
        {
            // Retrieve the JWT token from the current request headers
            string token = _httpContext.Request.Headers["Authorization"];

            // Check if the token is present and formatted correctly
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Remove "Bearer " prefix from the token
                token = token.Substring("Bearer ".Length);
                return token;
            }
            // If the token is not present or formatted correctly, return null or throw an exception
            return null;
        }
        #endregion

        public static string GenerateNewGuid(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out _))
            {
                return Guid.NewGuid().ToString();
            }
            return id;
        }
        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool CheckEmailRegex(string Email)
        {
            try
            {
                var isValid = new System.Net.Mail.MailAddress(Email);
                var emailMatch = Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (!emailMatch)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> GetFilterValues(string fieldName, string Operator, ReportFilters reportFilters)
        {
            List<string> stationIds = reportFilters.Filters.Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator && !string.IsNullOrEmpty(f.Value)).
                                        SelectMany(f => f.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            return stationIds;

        }

        #region DateTime
        public static DateTime? GetDateFilter(string fieldName, string Operator, ReportFilters reportFilters)
        {
            DateTime? startDate = reportFilters.Filters
                                   .Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator)
                                   .Select(f => DateTime.TryParse(f.Value, out var date) ? date : (DateTime?)null)
                                   .FirstOrDefault();

            startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            if (startDate.HasValue)
            {
                var ee = reportFilters.Filters.Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator).FirstOrDefault().Value;
                string Date = ee.Split("T")[0];
                string Time = ee.Split("T")[1];
                DateTime date = new DateTime(Convert.ToInt16(Date.Split("-")[0]), Convert.ToInt16(Date.Split("-")[1]), Convert.ToInt16(Date.Split("-")[2]), Convert.ToInt16(Time.Split(":")[0]), Convert.ToInt16(Time.Split(":")[1]), Convert.ToInt16(Time.Split(":")[2].Split(".")[0]), DateTimeKind.Utc);
                return date;
            }
            return startDate;
        }
        #endregion

        #region Shift

        public static CommonShiftModel GetShiftByTime(DateTime dateTime, List<CommonShiftModel> shifts)
        {
            var localDateTime = CommonHelper.ConvertUTCToLocalTime(dateTime);
            var time = localDateTime.TimeOfDay;

            var shift = shifts.FirstOrDefault(x =>
                       // Case 1: Shift is within the same day, time falls between FromTime and ToTime
                       (x.FromTime <= x.ToTime && time >= x.FromTime && time < x.ToTime)
                       // Case 2: Shift spans midnight, time falls either before midnight on the same day 
                       // or after midnight into the next day.
                       || (x.FromTime > x.ToTime && (time >= x.FromTime || time < x.ToTime)));

            return shift;
        }

        /// <summary>
        /// Get Previous Shift by passing the list of shifts for specific (station - tenant) and 
        /// start time of the current shift which we need to find the previous shift for
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        public static CommonShiftModel GetPreviousShift(TimeSpan fromTime, List<CommonShiftModel> shifts)
        {
            var previousShift = shifts.Where(x => x.ToTime < fromTime) // Get shifts that end before the current shift starts
                                      .OrderByDescending(x => x.ToTime) // Sort by ToTime in descending order to get the most recent one
                                      .FirstOrDefault(); // Get the most recent shift that ended before the current shift
            return previousShift;
        }

        #endregion

        #region Date

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

        public static DateTime GetShiftStartDateTime(DateTime operationDateTime, CommonShiftModel shift)
        {
            DateTime shiftStartDateTime;

            //get Local Time as operationDateTime will be in UTC
            var localtime = ConvertUTCToLocalTime(operationDateTime);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;

            //Add shift hours
            if (shift.FromTime < shift.ToTime || shift.FromTime < localtime.TimeOfDay)
            {
                shiftStartDateTime = startDateTime.Date.AddHours(shift.FromTime.Hours).AddMinutes(shift.FromTime.Minutes);
            }
            else
            {
                shiftStartDateTime = startDateTime.Date.AddDays(-1).AddHours(shift.FromTime.Hours).AddMinutes(shift.FromTime.Minutes);
            }
            //convert the local date back to utc date
            DateTime utcDateTime = ConvertLocalToUTCTime(shiftStartDateTime);

            //Converting back to UTC
            var result = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);

            return result;
        }

        public static DateTime GetShiftEndDateTime(DateTime operationDateTime, CommonShiftModel shift)
        {
            DateTime shiftEndDateTime;

            //get Local Time as operationDateTime will be in UTC
            var localtime = ConvertUTCToLocalTime(operationDateTime);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;


            //Convert shiftStart and shiftEnd from timestamp to datetime
            if (shift.FromTime < shift.ToTime || shift.FromTime > localtime.TimeOfDay)
            {
                shiftEndDateTime = startDateTime.Date.AddHours(shift.ToTime.Hours).AddMinutes(shift.ToTime.Minutes);
            }
            else
            {
                shiftEndDateTime = startDateTime.Date.AddDays(1).AddHours(shift.ToTime.Hours).AddMinutes(shift.ToTime.Minutes);
            }

            //convert the local date back to utc date
            DateTime utcDateTime = ConvertLocalToUTCTime(shiftEndDateTime);

            //Converting back to UTC
            var result = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);

            return result;
        }

        #endregion

        public static string ConvertDateTimeToString(DateTime? dateTime)
        {
            return dateTime?.ToString() ?? string.Empty;
        }

        public static double ConvertFloatToDouble(float? number, double defaultValue = double.NaN)
        {
            return number.HasValue ? (double)number.Value : defaultValue;
        }

    }
}
