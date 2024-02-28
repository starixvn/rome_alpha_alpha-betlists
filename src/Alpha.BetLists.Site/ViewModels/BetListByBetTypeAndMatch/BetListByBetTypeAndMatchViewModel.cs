namespace Alpha.BetLists.Site.ViewModels.BetListByBetTypeAndMatch
{
    using System.Collections.Generic;
    using System.Linq;
    using BetListByBetType;
    using BetLists.Helpers;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetListByBetTypeAndMatch;
    using AlphaPlus.Configurations;

    public class BetListByBetTypeAndMatchViewModel : BetListByBetTypeViewModel
    {
        public BetListByBetTypeAndMatchViewModel(
            BetListByBetTypeAndMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
        {
            MatchId = query.MatchId;
            IsLive = query.IsLive;
            BetTeam = query.BetTeam;
            LeagueId = betListData.Tickets.Any() ? betListData.Tickets.FirstOrDefault().LeagueId : 0;
            LeagueGroupId = betListData.Tickets.Any() ? betListData.Tickets.FirstOrDefault().LeagueGroupId : 0;
            Title = GetTitle(query.BetType).ToUpperInvariant();
        }

        public int MatchId { get; }

        public int IsLive { get; }

        public string BetTeam { get; }

        public int LeagueId { get; }

        public int LeagueGroupId { get; }

        public string GetTitle(int betType)
            => BetTypeHelper.IsOutRight(betType)
            ? CacheRefServiceHelper.GetBetTypeName(betType)
            : LeagueId == 0
            ? CacheRefServiceHelper.GetBetTypeName(betType)
            : $"{CacheRefServiceHelper.GetLeagueName(LeagueId, LeagueGroupId)} {CacheRefServiceHelper.GetBetTypeName(betType)}";

    }
}
