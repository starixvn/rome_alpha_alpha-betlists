namespace Alpha.BetLists.Site.ViewModels.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Utils;

    public class Footer
    {
        private static readonly IDictionary<UserLevel, int> FooterStatusColSpanByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 2 },
            { UserLevel.Master, 3 },
            { UserLevel.Super, 4 }
        };

        public Footer(BetListData betListData, UserLevel viewerLevel)
        {
            TotalWinLoss = betListData.Tickets.Where(ticket => ticket.BetTypeId != 500).Sum(ticket => ticket.Winlost);
            TotalCommission = betListData.Tickets.Sum(ticket => ticket.PlayerComm);
            StatusColSpan = FooterStatusColSpanByViewLevelId[viewerLevel];
            SubTotalWinLoss = ConvertByBetType.Winloss(TotalWinLoss);
            SubTotalCommission = ConvertByBetType.Comm(TotalCommission);
            Total = ConvertByBetType.Winloss(TotalWinLoss + TotalCommission);
        }

        public int SubTotalColSpan { get; } = 5;

        public int StatusColSpan { get; }

        public string SubTotalWinLoss { get; }

        public string SubTotalCommission { get; }

        public string Total { get; }

        public decimal TotalWinLoss { get; }

        public decimal TotalCommission { get; }
    }
}