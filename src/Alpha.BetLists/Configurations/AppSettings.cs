namespace Alpha.BetLists.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static string ResultAppPath => GetConfigValue("ResultSiteVirtualPath") ?? "site-results";

        public static string BetListAppPath => GetConfigValue("BetListAppPath") ?? "site-betlists";

        public static string DefaultIpAddress => GetConfigValue("DefaultIpAddress");

        public static string LocalIpAddress => GetConfigValue("LocalIpAddress");

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify()
        {
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
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