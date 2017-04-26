namespace Alpha.BetLists.Site.ViewModels.WinLossByMatch
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.WinLossByMatch;
    using WinLoss;

    public class WinLossByMatchViewModel : WinLossViewModel
    {
        public WinLossByMatchViewModel(
            WinLossByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            BetType = query.BetType;
            LeagueId = query.LeagueId;
            MatchId = query.MatchId;
            Race = query.Race;
            LiveIndicator = query.LiveIndicator;
            CustLevelId = query.CustLevelId;
        }

        public int BetType { get; }

        public int LeagueId { get; }

        public int MatchId { get; }

        public string Race { get; }

        public int LiveIndicator { get; }

        public byte CustLevelId { get; }
    }
}