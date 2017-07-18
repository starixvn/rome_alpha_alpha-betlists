namespace Alpha.BetLists.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapReportsPath;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> addQueryStringParamsFunc;

        public static Func<string, IDictionary<string, object>, string> AddQueryStringParamsFunc
        {
            get
            {
                return addQueryStringParamsFunc ?? AddQueryStringParams;
            }

            internal set
            {
                addQueryStringParamsFunc = value;
            }
        }

        public static Func<string, string> MapReportsPath
        {
            get
            {
                return mapReportsPath ?? MicroSites.ReportsUrl;
            }

            internal set
            {
                mapReportsPath = value;
            }
        }

        internal static string AddQueryStringParams(string path, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return path;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{path}?{queryString}";
        }
    }
}