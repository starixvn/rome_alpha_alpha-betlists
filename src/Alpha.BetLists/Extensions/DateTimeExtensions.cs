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

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);
    }
}