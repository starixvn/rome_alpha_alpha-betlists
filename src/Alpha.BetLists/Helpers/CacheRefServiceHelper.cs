namespace Alpha.BetLists.Helpers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Alpha.BetLists.Constants;
    using AlphaPlus.Configurations;
    using Fanex.RemotingRef;
    using System.Linq;

    public static class CacheRefServiceHelper
    {
        private static IRemotingRefService remotingRefService;

        public static string GetBetTypeName(int betTypeId)
            => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public static string GetLeagueName(int leagueId)
            => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

        public static string GetLeagueName(int leagueId, int leagueGroupId)
            => AppSettings.IsSiteLabel777 ?
            Instance.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty) :
            Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

        public static string GetProductName(int productId)
        {
            if (AppSettings.IsSiteLabel777)
            {
                var productNameFor777 = Instance.GetResourceDataByRefIdAndResourceId(
                    "Product_Saba",
                    productId.ToString(),
                    LanguageCode,
                    string.Empty);

                return string.IsNullOrEmpty(productNameFor777) ?
                    Instance.GetResourceDataByRefIdAndResourceId(
                        "Product",
                        productId.ToString(),
                        LanguageCode,
                        string.Empty) :
                    productNameFor777;
            }

            return Instance.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty); ;
        }


        public static string GetProductDetailName(int resourceId)
        {   
            if (AppSettings.IsSiteLabel777)
            {
                var productNameFor777 = Instance.GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", resourceId.ToString(), LanguageCode, string.Empty);

                return string.IsNullOrEmpty(productNameFor777) ? 
                    Instance.GetResourceDataByRefIdAndResourceId("ProductDetail", resourceId.ToString(), LanguageCode, string.Empty) : 
                    productNameFor777;
            }

            return Instance.GetResourceDataByRefIdAndResourceId("ProductDetail", resourceId.ToString(), LanguageCode, string.Empty);
        }

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