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
            => IsStandaloneReport ? $"{ReportDomainUrl}{appPath?.Replace("~", string.Empty)}" : MicroSites.ReportsUrl(appPath);

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

        internal static string ReportDomainUrl => ReportDomain.IndexOf("{0}") != -1 ?
            string.Format(ReportDomain, HttpContext.Current.Request.Headers["Host"]) : ReportDomain;

        internal static string MainDomain => UserSession.Current.GetProperty<string>("MainDomain");

        internal static bool IsStandaloneReport
            => !string.IsNullOrWhiteSpace(MainDomain);
    }
}