namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Requests;

    public class WinLossViewModel : BetListBaseViewModel
    {
        private static readonly IDictionary<UserLevel, int> TotalColumnByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 8 },
            { UserLevel.Master, 9 },
            { UserLevel.Super, 10 }
        };

        private static readonly IDictionary<UserLevel, int> FooterStatusColSpanByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 2 },
            { UserLevel.Master, 3 },
            { UserLevel.Super, 4 }
        };

        public WinLossViewModel(BaseQuery query, BetListData betListData, IBetListHTMLBuilder builder, ITicketHelper ticketHelper)
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
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string Title { get; }

        public int FooterSubTotalColSpan { get; } = 5;

        public int FooterStatusColSpan { get; }

        public string FooterSubTotalWinLoss { get; }

        public string FooterSubTotalCommission { get; }

        public string FooterTotal { get; }

        public int TotalColumn { get; }

        protected decimal TotalWinLoss { get; }

        protected decimal TotalCommission { get; }

        private string GetTitle(BaseQuery query)
            => $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";
    }
}