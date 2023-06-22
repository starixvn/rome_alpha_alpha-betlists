namespace Alpha.BetLists.Extensions
{
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static string JoinWithComma<T>(this IEnumerable<T> source, string separator = ",")
            => string.Join(separator, source);

        public static string JoinWithSlash<T>(this IEnumerable<T> source) => string.Join("/", source);
    }
}