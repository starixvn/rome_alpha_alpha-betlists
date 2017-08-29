namespace Alpha.BetLists.Site.ViewModels.BetListByBetTypeAndMatch
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetListByBetType;
    using BetLists.Helpers;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using RemotingRef;
    using Requests.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchViewModel : BetListByBetTypeViewModel
    {
        public BetListByBetTypeAndMatchViewModel(
            BetListByBetTypeAndMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBettypeIds)
        {
            MatchId = query.MatchId;
            IsLive = query.IsLive;
            BetTeam = query.BetTeam;
            LeagueId = betListData.Tickets.FirstOrDefault().LeagueId;
            Title = GetTitle(query.BetType).ToUpperInvariant();
        }

        public int MatchId { get; }

        public int IsLive { get; }

        public string BetTeam { get; }

        public int LeagueId { get; }

        public string GetTitle(int betType)
            => BetTypeHelper.IsOutRight(betType)
            ? CacheRefFactory.CacheRef.GetBetTypeName(betType)
            : BetTypeHelper.IsHorseRacing(betType)
            ? Resources.horseRacing
            : $"{CacheRefFactory.CacheRef.GetLeagueName(LeagueId)} {CacheRefFactory.CacheRef.GetBetTypeName(betType)}";
    }
}
