namespace Alpha.BetLists.Site.Helpers
{
    using Alpha.BetLists.Site.Constants;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Web.Helpers;

    public static class UrlHelper
    {
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

        public static Func<string, string> MapReportsPath { get; internal set; } = MapReportsDomain;

        internal static string MapReportsDomain(string appPath)
        {
            var cookieless = HttpContext.Current.Session.IsCookieless ?
                $"/(S({UserSession.Current.GetProperty<string>(SessionKey.ReportSessionID)}))" :
                string.Empty;

            return IsStandaloneReport ? $"{ReportDomain}{cookieless}{appPath?.Replace("~", string.Empty)}" : MicroSites.ReportsUrl(appPath);
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

        internal static string ReportDomain => UserSession.Current.GetProperty<string>(SessionKey.ReportDomainSessionKey) ?? string.Empty;

        internal static string MainDomain => UserSession.Current.GetProperty<string>("MainDomain");

        internal static bool IsStandaloneReport
            => !string.IsNullOrWhiteSpace(MainDomain) && !string.IsNullOrWhiteSpace(ReportDomain);
    }
}