namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Fanex.BetList.Core.Utils.StringExt;
    using Fanex.Common;
    using Fanex.BetList.Core.Utils.TicketExt;

    public class TicketDto : Ticket
    {
        public string Country { get; set; }

        public bool IsInWatchlist { get; set; }

        public bool IsHighlight { get; set; }

        public string BetType { get; set; }

        public int? DeviceType { get; set; }

        public string Platform
        {
            get
            {
                if (DeviceType == null)
                {
                    return Resources.Platform_NA;
                }

                var deviceInfo = Resources.ResourceManager.GetString(string.Format("Platform_{0}", DeviceType.ToString()));

                return deviceInfo ?? Resources.Platform_Uncategorized;
            }
        }

        public decimal? MemberWin
        {
            get
            {
                if (!string.IsNullOrEmpty(ExtraInfo) && ExtraInfo.HasAttributeInXmlTag("p1", "pa") && this.IsRunningWaiting())
                {
                    var payoutXml = ExtraInfo.GetXmlTagAttributeValue("p1", "pa");
                    var payout = Converter.ToDecimal(payoutXml);

                    return payout - ActualStake;
                }

                return null;
            }
        }

        public string MemberWinText
            => MemberWin.HasValue ? ConvertByBetType.Winloss(MemberWin.Value) : Resources.Platform_NA;
    }
}