namespace Alpha.BetLists.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";

        public static bool IsBefore(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime < otherDateTime;
        }

        public static bool IsBeforeOrEqual(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime <= otherDateTime;
        }

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime ToStartOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);

        public static DateTime ToEndOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;
    }
}