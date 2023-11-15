namespace Alpha.BetLists.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Extensions;
    using Fanex.Common;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;
        private static IEnumerable<string> monitoringBetsBettypes;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string SiteLabel => GetConfigValue("SiteLabel");

        public static string SiteLabel789y => GetConfigValue("SiteLabel789y");

        public static string StoredProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string MainAppPath => GetConfigValue("MainAppPath") ?? "site-main";

        public static string ResultAppPath => GetConfigValue("ResultSiteVirtualPath") ?? "site-results";

        public static string TotalBetsForecastAppPath => GetConfigValue("ExTotalBetsForecastAppPath") ?? "ex-totalbetsforecast";

        public static string DefaultIpAddress => GetConfigValue("DefaultIpAddress");

        public static string LocalIpAddress => GetConfigValue("LocalIpAddress");

        public static bool EnableBetsListMonitoring => Converter.ToBool(GetConfigValue("EnableBetsListMonitoring"));

        public static bool LocalTest => GetConfigValue("LocalTest") == "1";

        public static IEnumerable<int> UsersAlwaysAllowAccessBetsListMonitoring
        {
            get
            {
                var config = GetConfigValue("UsersAlwaysAllowAccessBetsListMonitoring");

                return config == null ? Enumerable.Empty<int>() : config.SplitByCommaWithNoneEmptyEntries().ToListInt();
            }
        }

        public static int RunningMatchCacheTime => Converter.ToInt(GetConfigValue("RunningMatchCacheTime"));

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static IEnumerable<int> ProductOfferProductDetail
        {
            get
            {
                var productIds = GetConfigValue("ProductOfferProductDetail");
                if (!string.IsNullOrEmpty(productIds))
                {
                    return productIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => int.Parse(i));
                }

                return Enumerable.Empty<int>();
            }
        }

        public static IEnumerable<string> OfferSabaPromotionSites
            => GetConfigValue("EnableProduct-37:Sites")?.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

        public static IEnumerable<string> OfferSabaPromotionHeadlineSites
            => GetConfigValue("EnableProduct-37:HLSites")?.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

        public static byte SettingStatus
        {
            get
            {
                byte.TryParse(ConfigurationManager.AppSettings["SettingStatus"], out byte settingStatus);
                return settingStatus;
            }
        }

        public static string BetlistRouting
        {
            get
            {
                var context = HttpContext.Current;
                var cookieless = context.Session.IsCookieless ? $"/(S({context.Session.SessionID}))" : string.Empty;
                return IsStandaloneReport ? $"site-betlists{cookieless}/" : string.Empty;
            }
        }

        public static bool IsStandaloneReport => GetConfigValue("IsStandaloneReport") == "1";

        public static bool IsMBCSite => !string.IsNullOrWhiteSpace(GetConfigValue("SiteLabel"))
            && !GetConfigValue("SiteLabel").Equals("789y", StringComparison.OrdinalIgnoreCase);

        public static bool DisabledVietnamese => IsMBCSite;

        public static string IPV4TopCountries => GetConfigValue("IPInfo:IPV4TopCountries");

        public static int IPV4TopCountriesCacheInMinutes => int.Parse(GetConfigValue("IPInfo:IPV4TopCountriesCacheInMinutes"));

        public static int IPV4CacheInMinutes => int.Parse(GetConfigValue("IPInfo:IPV4CacheInMinutes"));

        public static int IPV6CacheInMinutes => int.Parse(GetConfigValue("IPInfo:IPV6CacheInMinutes"));

        public static string IPInfoCommonCacheRedis
            => GetConfigValue("RedisConfigurationOptions:Alpha.BetListsCommonIPInfo");

        public static string IPInfoIndividualCacheRedis
            => GetConfigValue("RedisConfigurationOptions:Alpha.BetListsIndividualIPInfo");

        public static IEnumerable<string> MonitoringBetsBettypes
        {
            get
            {
                monitoringBetsBettypes = monitoringBetsBettypes ?? GetConfigValue("MonitoringBets:Bettypes").SplitByCommaWithNoneEmptyEntries();

                return monitoringBetsBettypes;
            }
        }

        public static string JsVersion => GetConfigValue("JsVersion");

        public static bool IsEnabledLoggingApplicationStartEnd => GetConfigValue("EnableLogApplicationStartEnd") == "1";

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify()
        {
            Require(StoredProcedurePath, nameof(StoredProcedurePath));
            Require(SiteLabel, nameof(SiteLabel));
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }
    }
}