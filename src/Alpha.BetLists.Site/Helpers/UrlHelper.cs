namespace Alpha.BetLists.Site.Helpers
{
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
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
                mapReportsPath = mapReportsPath ?? MapReportsDomain;

                return mapReportsPath;
            }

            internal set
            {
                mapReportsPath = value;
            }
        }

        internal static string MapReportsDomain(string appPath)
        {
            var cookieless = HttpContext.Current.Session.IsCookieless ? $"/(S({HttpContext.Current.Session.SessionID}))" : string.Empty;

            return IsStandaloneReport ? $"{ReportDomainUrl}{cookieless}{appPath?.Replace("~", string.Empty)}" : MicroSites.ReportsUrl(appPath);
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

        internal static string ReportDomain => ConfigurationManager.AppSettings["Report:Domain"] ?? string.Empty;

        internal static string ReportDomainUrl
        {
            get
            {
                var mainDomain = HttpContext.Current.Request.Headers["Host"];
                if (HttpContext.Current.Session.IsCookieless)
                {
                    var subDomain = mainDomain.Split('.').ElementAt(0);
                    mainDomain = mainDomain.Replace($"{subDomain}.", string.Empty);
                }

                return ReportDomain.IndexOf("{0}") != -1 ?
                    string.Format(ReportDomain, mainDomain) : ReportDomain;
            }
        }

        internal static string MainDomain => UserSession.Current.GetProperty<string>("MainDomain");

        internal static bool IsStandaloneReport
            => !string.IsNullOrWhiteSpace(MainDomain) && !string.IsNullOrWhiteSpace(ReportDomain);
    }
}