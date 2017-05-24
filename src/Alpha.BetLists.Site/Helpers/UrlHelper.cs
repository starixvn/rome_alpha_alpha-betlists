namespace Alpha.BetLists.Site.Helpers
{
    using Configurations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapExMainPath;

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

        public static Func<string, string> MapExMainPath
        {
            get
            {
                return mapExMainPath ?? MicroSites.OldMainUrl;
            }

            internal set
            {
                mapExMainPath = value;
            }
        }

        public static Func<string, string> MapReportsPath
        {
            get
            {
                return mapReportsPath ?? GetReportsPath;
            }

            internal set
            {
                mapReportsPath = value;
            }
        }

        internal static string AddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return url;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{url}?{queryString}";
        }

        internal static string GetReportsPath(string virtualPath = "~/Outstanding/Index") => AppPathHelper.GetAppPath(AppSettings.ReportsAppPath, virtualPath);
    }
}