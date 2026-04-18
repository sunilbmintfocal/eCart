using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace MintCart.Common
{
    /// <summary>
    /// Provides common helper methods for the application.
    /// </summary>
    public static class CommonHelper
    {
        #region Strings
        /// <summary>
        /// Truncates a string to a specified length.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="length">The maximum length of the truncated string.</param>
        /// <returns>The truncated string, or the original string if it is shorter than the specified length.</returns>
        public static string Truncate(string value, int length)
        {
            if (value != null && value.Length > length)
            {
                return value[..length];
            }

            return value ?? default!;
        }

        /// <summary>
        /// Converts a list of strings into a single comma-separated string.
        /// </summary>
        /// <param name="input">The list of strings to convert.</param>
        /// <returns>A comma-separated string.</returns>
        public static string ListToString(IEnumerable<string> input)
        {
            return string.Join(",", input);
        }

        /// <summary>
        /// Converts a string into a SecureString.
        /// </summary>
        /// <param name="passwordString">The string to convert.</param>
        /// <returns>A SecureString containing the password.</returns>
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
        /// Returns back the Current Context Token from the HttpContext.
        /// </summary>
        /// <param name="_httpContext">The current HTTP context.</param>
        /// <returns>The JWT token without the Bearer prefix, or null if not found.</returns>
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

        /// <summary>
        /// Generates a new GUID if the provided ID is null or empty, otherwise confirms the ID is a valid GUID.
        /// </summary>
        /// <param name="id">The existing ID to check.</param>
        /// <returns>A new GUID string or the original valid GUID string.</returns>
        public static string GenerateNewGuid(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out _))
            {
                return Guid.NewGuid().ToString();
            }
            return id;
        }

        /// <summary>
        /// Generates a new unique identifier (GUID).
        /// </summary>
        /// <returns>A new GUID as a string.</returns>
        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Validates an email address using a regular expression.
        /// </summary>
        /// <param name="Email">The email address to validate.</param>
        /// <returns>True if the email address is valid, otherwise false.</returns>
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

        /// <summary>
        /// Extracts filter values for a specific field and operator from the report filters.
        /// </summary>
        /// <param name="fieldName">The name of the field to filter on.</param>
        /// <param name="Operator">The operator used for the filter.</param>
        /// <param name="reportFilters">The report filters containing the criteria.</param>
        /// <returns>A list of filter values.</returns>
        public static List<string> GetFilterValues(string fieldName, string Operator, ReportFilters reportFilters)
        {
            List<string> stationIds = reportFilters.Filters.Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator && !string.IsNullOrEmpty(f.Value)).
                                        SelectMany(f => f.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            return stationIds;

        }

        #region DateTime
        /// <summary>
        /// Retrieves a Date filter value for a specific field and operator.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="Operator">The operator.</param>
        /// <param name="reportFilters">The report filters.</param>
        /// <returns>The filtered date, or null if not found.</returns>
        public static DateTime? GetDateFilter(string fieldName, string Operator, ReportFilters reportFilters)
        {
            DateTime? startDate = reportFilters.Filters
                                   .Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator)
                                   .Select(f => DateTime.TryParse(f.Value, out var date) ? date : (DateTime?)null)
                                   .FirstOrDefault();

            if (startDate.HasValue)
            {
                startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                var filterValue = reportFilters.Filters.Where(f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase) && f.Operator == Operator).FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(filterValue) && filterValue.Contains("T"))
                {
                    string DatePart = filterValue.Split("T")[0];
                    string TimePart = filterValue.Split("T")[1];
                    string[] dateSegments = DatePart.Split("-");
                    string[] timeSegments = TimePart.Split(":");
                    
                    DateTime date = new DateTime(
                        Convert.ToInt16(dateSegments[0]), 
                        Convert.ToInt16(dateSegments[1]), 
                        Convert.ToInt16(dateSegments[2]), 
                        Convert.ToInt16(timeSegments[0]), 
                        Convert.ToInt16(timeSegments[1]), 
                        Convert.ToInt16(timeSegments[2].Split(".")[0]), 
                        DateTimeKind.Utc);
                    return date;
                }
            }
            return startDate;
        }

        /// <summary>
        /// Converts a DateTime value to a local time equivalent (centered around UTC+3 for specific business logic).
        /// </summary>
        /// <param name="datetime">The UTC DateTime to convert.</param>
        /// <returns>The converted local time.</returns>
        public static DateTime ConvertUTCToLocalTime(DateTime? datetime)
        {
            if (datetime == null) return DateTime.MinValue;
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == 180).FirstOrDefault(); 
            return TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo ?? TimeZoneInfo.Local);
        }

        /// <summary>
        /// Converts a local time back to UTC based on the +3 hours offset.
        /// </summary>
        /// <param name="datetime">The local DateTime to convert.</param>
        /// <returns>The UTC equivalent.</returns>
        public static DateTime ConvertLocalToUTCTime(DateTime? datetime)
        {
            if (datetime == null) return DateTime.MinValue;
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(datetime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }
        #endregion

        #region Shift
        /// <summary>
        /// Identifies the appropriate shift for a given date and time.
        /// </summary>
        /// <param name="dateTime">The date and time to check.</param>
        /// <param name="shifts">The list of available shifts.</param>
        /// <returns>The matching CommonShiftModel.</returns>
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
        /// <param name="fromTime">The start time of the current shift.</param>
        /// <param name="shifts">The list of available shifts.</param>
        /// <returns>The previous shift model.</returns>
        public static CommonShiftModel GetPreviousShift(TimeSpan fromTime, List<CommonShiftModel> shifts)
        {
            var previousShift = shifts.Where(x => x.ToTime < fromTime) // Get shifts that end before the current shift starts
                                      .OrderByDescending(x => x.ToTime) // Sort by ToTime in descending order to get the most recent one
                                      .FirstOrDefault(); // Get the most recent shift that ended before the current shift
            return previousShift;
        }
        #endregion

        #region Date
        /// <summary>
        /// Gets the start date time (00:00:00) for a given date, adjusted for the +3 hours offset.
        /// </summary>
        /// <param name="datetime">The date to adjust.</param>
        /// <returns>The start date time in UTC.</returns>
        public static DateTime GetStartDateTime(DateTime? datetime)
        {
            //get Local Time
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == 180).FirstOrDefault(); 
            var localtime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo ?? TimeZoneInfo.Local);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;

            //convert the local date back to utc date
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(startDateTime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets the end date time (23:59:59) for a given date, adjusted for the +3 hours offset.
        /// </summary>
        /// <param name="datetime">The date to adjust.</param>
        /// <returns>The end date time in UTC.</returns>
        public static DateTime GetEndDateTime(DateTime? datetime)
        {
            //get Local Time
            var cstZone = TimeZoneInfo.GetSystemTimeZones();
            var timezoneinfo = cstZone.Where(x => x.BaseUtcOffset.TotalMinutes == 180).FirstOrDefault(); 
            var localtime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(datetime), timezoneinfo ?? TimeZoneInfo.Local);

            //get DateTime with time as 00:00:00.
            DateTime startDateTime = localtime.Date;
            DateTime endDateTime = startDateTime.AddDays(1).AddSeconds(-1);

            //convert the local date back to utc date
            TimeSpan localOffset = TimeSpan.FromMinutes(180);
            DateTime utcDateTime = Convert.ToDateTime(endDateTime) - localOffset;

            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets the actual start date time for a specific shift based on an operation time.
        /// </summary>
        /// <param name="operationDateTime">The time of the operation.</param>
        /// <param name="shift">The shift details.</param>
        /// <returns>The shift start date time in UTC.</returns>
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

        /// <summary>
        /// Gets the actual end date time for a specific shift based on an operation time.
        /// </summary>
        /// <param name="operationDateTime">The time of the operation.</param>
        /// <param name="shift">The shift details.</param>
        /// <returns>The shift end date time in UTC.</returns>
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

        #region Formatting
        /// <summary>
        /// Formats a decimal amount as currency in INR format.
        /// </summary>
        /// <param name="amount">The amount to format.</param>
        /// <returns>A formatted currency string.</returns>
        public static string FormatCurrency(decimal amount)
        {
            // Indian number format with ₹ symbol
            return $"₹{amount:##,##,##0.00}";
        }

        /// <summary>
        /// Formats a DateTime as a user-friendly activity time string.
        /// </summary>
        /// <param name="date">The date and time of the activity.</param>
        /// <returns>A string like "Today, 10:00 AM" or "Yesterday, 05:00 PM".</returns>
        public static string FormatActivityTime(DateTime date)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            if (date.Date == today)
                return $"Today, {date:hh:mm tt}";

            if (date.Date == yesterday)
                return $"Yesterday, {date:hh:mm tt}";

            return date.ToString("dd MMM yyyy, hh:mm tt");
        }

        /// <summary>
        /// Resolves an icon name based on the activity type.
        /// </summary>
        /// <param name="activityType">The type of activity.</param>
        /// <returns>The name of the icon.</returns>
        public static string ResolveIcon(string? activityType) => activityType switch
        {
            "Sale" => "shopping_bag",
            "Complaint" => "assignment_late",
            "Purchase" => "local_shipping",
            _ => "receipt_long"
        };

        /// <summary>
        /// Resolves a status variant color name based on the status text.
        /// </summary>
        /// <param name="status">The status text.</param>
        /// <returns>The status variant name (e.g., primary, error, secondary).</returns>
        public static string ResolveStatusVariant(string? status) => status?.ToLower() switch
        {
            "completed" => "primary",
            "pending" => "error",
            "in transit" => "secondary",
            "cancelled" => "error",
            _ => "secondary"
        };
        #endregion

        /// <summary>
        /// Converts a DateTime? to its string representation.
        /// </summary>
        /// <param name="dateTime">The date time to convert.</param>
        /// <returns>The string representation of the date time.</returns>
        public static string ConvertDateTimeToString(DateTime? dateTime)
        {
            return dateTime?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Converts a nullable float to a double.
        /// </summary>
        /// <param name="number">The float to convert.</param>
        /// <param name="defaultValue">The default value if the number is null.</param>
        /// <returns>The converted double.</returns>
        public static double ConvertFloatToDouble(float? number, double defaultValue = double.NaN)
        {
            return number.HasValue ? (double)number.Value : defaultValue;
        }
    }
}
