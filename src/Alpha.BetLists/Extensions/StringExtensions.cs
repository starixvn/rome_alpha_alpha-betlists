namespace Alpha.BetLists.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static IEnumerable<int> ToListInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Enumerable.Empty<int>();
            }

            var result = 0;

            var ids = value
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(str => int.TryParse(str, out result))
                .Select(str => Convert.ToInt32(str));

            return ids;
        }
    }
}