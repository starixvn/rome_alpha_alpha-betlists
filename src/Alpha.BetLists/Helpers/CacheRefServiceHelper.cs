namespace Alpha.BetLists.Helpers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading;
    using AlphaPlus.Configurations;
    using Fanex.RemotingRef;

    public static class CacheRefServiceHelper
    {
        private static IRemotingRefService remotingRefService;

        public static string GetBetTypeName(int betTypeId)
            => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public static string GetLeagueName(int leagueId)
            => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

        public static string GetLeagueName(int leagueId, int leagueGroupId)
            => Instance.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

        public static string GetProductName(int productId)
        {
            var productName = Instance.GetResourceDataByRefIdAndResourceId(
                    "Product",
                    productId.ToString(),
                    LanguageCode,
                    string.Empty);

            if (AppSettings.IsSiteLabel777)
            {
                var sabaWord = Instance.GetResourceDataByRefIdAndResourceId("Saba", "Saba", LanguageCode, "Saba");
                var sabaWords = new List<string> { $" {sabaWord}", $"{sabaWord} ", sabaWord };
                foreach (var word in sabaWords)
                {
                    productName = Regex.Replace(productName, word, string.Empty, RegexOptions.IgnoreCase);
                }
            }

            return productName;
        }
            

        public static string GetProductDetailName(string refId, int resourceId)
            => Instance.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                LanguageCode,
                string.Empty);

        public static string GetSportName(int sportTypeId)
            => Instance.GetSportName(sportTypeId.ToString(), LanguageCode, string.Empty);

        internal static IRemotingRefService Instance
        {
            get
            {
                remotingRefService = remotingRefService ?? new RemotingRefService();

                return remotingRefService;
            }
            set
            {
                remotingRefService = value;
            }
        }

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}