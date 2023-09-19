namespace Alpha.BetLists.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;

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

        public static string[] SplitByCommaWithNoneEmptyEntries(this string value)
            => value.Split(
                new[]
                {
                    ','
                },
                StringSplitOptions.RemoveEmptyEntries);

        public static IEnumerable<int> ToListInt(this string[] value)
            => value.Select(s => Converter.ToInt(s));

        public static IDictionary<int, bool> ToIntDictionary(this string delimitedString)
        {
            IDictionary<int, bool> results = new Dictionary<int, bool>();

            if (!string.IsNullOrWhiteSpace(delimitedString))
            {
                delimitedString
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(text => {
                        if (int.TryParse(text, out var number))
                        {
                            results[number] = true;
                        }
                    });
            }

            return results;
        }

        public static string RemoveDuplicate(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var hashSet = new HashSet<int>();
            var valueArray = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in valueArray)
            {
                if (int.TryParse(item, out var intValue))
                {
                    hashSet.Add(intValue);
                }
            }

            return string.Join(",", hashSet);
        }
    }
}