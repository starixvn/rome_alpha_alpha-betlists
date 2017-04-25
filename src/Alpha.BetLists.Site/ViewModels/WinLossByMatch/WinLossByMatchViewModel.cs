namespace Alpha.BetLists.Site.ViewModels.WinLossByMatch
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Requests.WinLossByMatch;

    public class WinLossByMatchViewModel : BetListBaseViewModel
    {
        private static readonly IDictionary<UserLevel, int> TotalColumnByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 9 },
            { UserLevel.Master, 10 },
            { UserLevel.Super, 11 }
        };

        private static readonly IDictionary<UserLevel, int> FooterStatusColSpanByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 3 },
            { UserLevel.Master, 4 },
            { UserLevel.Super, 5 }
        };

        public WinLossByMatchViewModel(
            WinLossByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
            Title = GetTitle(query).ToUpperInvariant();
            TotalWinLoss = BetListData.Tickets.Where(ticket => ticket.BetTypeId != 500).Sum(ticket => ticket.Winlost);
            TotalCommission = BetListData.Tickets.Sum(ticket => ticket.PlayerComm);
            FooterStatusColSpan = FooterStatusColSpanByViewLevelId[ViewerLevel];
            FooterSubTotalWinLoss = ConvertByBetType.Winloss(TotalWinLoss);
            FooterSubTotalCommission = ConvertByBetType.Comm(TotalCommission);
            FooterTotal = ConvertByBetType.Winloss(TotalWinLoss + TotalCommission);
            TotalColumn = TotalColumnByViewLevelId[ViewerLevel];

            BetType = query.BetType;
            LeagueId = query.LeagueId;
            MatchId = query.MatchId;
            Race = query.Race;
            LiveIndicator = query.LiveIndicator;
            CustLevelId = query.CustLevelId;
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string Title { get; }

        public int FooterSubTotalColSpan { get; } = 6;

        public int FooterStatusColSpan { get; }

        public string FooterSubTotalWinLoss { get; }

        public string FooterSubTotalCommission { get; }

        public string FooterTotal { get; }

        public int TotalColumn { get; }

        protected decimal TotalWinLoss { get; }

        protected decimal TotalCommission { get; }

        public int BetType { get; }

        public int LeagueId { get; }

        public int MatchId { get; }

        public string Race { get; }

        public int LiveIndicator { get; }

        public byte CustLevelId { get; }
    }
}