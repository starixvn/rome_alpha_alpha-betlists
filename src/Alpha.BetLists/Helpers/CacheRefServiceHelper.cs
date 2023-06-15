namespace Alpha.BetLists.Helpers
{
    using System.Threading;
    using Fanex.RemotingRef;

    public static class CacheRefServiceHelper
    {
        private static IRemotingRefService remotingRefService;

        public static string GetBetTypeName(int betTypeId)
            => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public static string GetLeagueName(int leagueId)
            => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

        public static string GetProductName(int productId)
            => Instance.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty);

        public static string GetProductDetailName(string refId, int resourceId)
            => Instance.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                LanguageCode,
                string.Empty);

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