namespace Alpha.BetLists.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Extensions;
    using Fanex.Common;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string StoredProcedurePath => GetConfigValue("StoredProcedurePath");

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

        public static bool EnabledStaticData => GetConfigValue("EnabledStaticData") == "1";

        public static bool IsIntegrationTest => GetConfigValue("IsIntegrationTest") == "1";

        public static int RunningMatchCacheTime => Converter.ToInt(GetConfigValue("RunningMatchCacheTime"));

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static bool IsOfferJackpotProductDetails
            => "1" == GetConfigValue("IsOfferJackpotProductDetails");

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify()
        {
            Require(StoredProcedurePath, nameof(StoredProcedurePath));
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