namespace Alpha.BetLists.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string ShortDate = "MMddyyyy";
        private const string LongDateTimeWithSlash = "MM/dd/yyyy hh:mm:ss tt";

        public static bool IsBefore(this DateTime dateTime, DateTime otherDateTime)
            => dateTime < otherDateTime;

        public static bool IsBeforeOrEqual(this DateTime dateTime, DateTime otherDateTime)
            => dateTime <= otherDateTime;

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToShortDate(this DateTime dateTime)
            => dateTime.ToString(ShortDate);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime ToStartOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);

        public static DateTime ToEndOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;

        public static DateTime FirstDayOfYear(this DateTime dateTime)
            => new DateTime(dateTime.Year, 1, 1);

        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            const int numberOfWeek = 7;
            int diff = (numberOfWeek + (date.DayOfWeek - startOfWeek)) % numberOfWeek;

            return date.AddDays(-1 * diff).Date;
        }
        
        public static string ToLongDateTimeWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateTimeWithSlash);
    }
}