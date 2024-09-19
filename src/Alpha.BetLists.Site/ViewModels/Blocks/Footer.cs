namespace Alpha.BetLists.Site.ViewModels.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class Footer
    {
        private static readonly IDictionary<UserLevel, int> FooterStatusColSpanByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 3 },
            { UserLevel.Master, 4 },
            { UserLevel.Super, 5 }
        };

        public Footer(BetListData betListData, UserLevel viewerLevel, bool showMemberName, string[] excludingTicketStatus = null, bool supportRiksAmount = false)
        {
            if (betListData != null)
            {
                TotalWinLoss = betListData.Tickets.Sum(ticket =>
               IsNotCricket(ticket.BetTypeId) &&
               NotContainExcludingStatus(ticket, excludingTicketStatus) ? ticket.Winlost : 0);

                TotalCommission = betListData.Tickets.Sum(ticket =>
                    NotContainExcludingStatus(ticket, excludingTicketStatus) ? ticket.PlayerComm : 0);
            }

            StatusColSpan = FooterStatusColSpanByViewLevelId[viewerLevel] + (supportRiksAmount ? 2 : 0);
            SubTotalWinLoss = ConvertByBetType.Winloss(TotalWinLoss);
            SubTotalCommission = ConvertByBetType.Comm(TotalCommission);
            Total = ConvertByBetType.Winloss(TotalWinLoss + TotalCommission);
            SubTotalColSpan += showMemberName ? 1 : 0;
        }

        public int SubTotalColSpan { get; } = 5;

        public int StatusColSpan { get; }

        public string SubTotalWinLoss { get; }

        public string SubTotalCommission { get; }

        public string Total { get; }

        public decimal TotalWinLoss { get; }

        public decimal TotalCommission { get; }

        private static bool IsNotCricket(int betType) => betType != 500;

        private static bool NotContainExcludingStatus(ITicket ticket, string[] excludingStatus)
        {
            var excludeStatus = excludingStatus ?? new string[] { };

            return !excludeStatus.Contains(ticket.Status.ToLowerInvariant());
        }
    }
}
