namespace Alpha.BetLists.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;

    public static class FanexDataSettingKeyHelper
    {
        public static string GetWinLossDetailSettingKeySubfix(string siteName)
               => WinLossReportSettingKeys.TryGetValue(siteName, out var subfix) ? subfix : string.Empty;

        private static IDictionary<string, int[]> ProductDetailSettingKeys = new Dictionary<string, int[]>{
            { "Acc_BetList_LC_Detail", new[]{ProductIdConstants.GPILiveCasino} }
        };

        private static IDictionary<string, string> winLossDetailSettingKeys;
        internal static IDictionary<string, string> WinLossReportSettingKeys
        {
            get
            {
                if (winLossDetailSettingKeys == null)
                {
                    winLossDetailSettingKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    (AppSettings.WLDetailSettingKeySites() ?? string.Empty)
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .ForEach(item =>
                        {
                            var siteAndSubfix = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            if (siteAndSubfix.Length == 2)
                            {
                                winLossDetailSettingKeys[siteAndSubfix[0]] = $"_{siteAndSubfix[1]}";
                            }
                        });
                }

                return winLossDetailSettingKeys;
            }
        }

        internal static string GetTicketDetailSettingKey(int productId)
            => ProductDetailSettingKeys.FirstOrDefault(mapper => mapper.Value.Contains(productId)).Key;
    }
}
