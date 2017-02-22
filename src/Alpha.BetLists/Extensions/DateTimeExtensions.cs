namespace Alpha.BetLists.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DateTimeExtensions
    {
        public static bool IsBefore(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime < otherDateTime;
        }
    }
}